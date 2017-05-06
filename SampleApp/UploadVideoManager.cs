using Newtonsoft.Json.Linq;
using SDKLib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.NetworkInformation;
using System.Threading;

namespace SampleApp {

   public class UploadVideoManager : SDKLib.User.Result.UploadVideo.If {

      internal interface Callback {
         void onBeginUpload();
         void onEndUpload();
         void onUploadProgress(float progressPercent, long complete, long max);
         void onPendingItemsChanged();
         void onFailedItemsChanged();
         void onCompletedItemsChanged();
      }

      public abstract class UploadItem {

         protected JObject mJObject = null;

         internal void setJObject(JObject jObject) {
            mJObject = (JObject)jObject.DeepClone();
         }

         internal JObject getJObject() {
            if (null == mJObject) {
               mJObject = new JObject();
            }
            return mJObject;
         }

         internal void setAttr(string attr, object value) {
            JObject jObject = getJObject();
            JToken currentValue = null;

            if (jObject.TryGetValue(attr, out currentValue)) {
               ((JValue)currentValue).Value = value;
            } else {
               jObject.Add(attr, new JValue(value));
            }
         }

         internal object getAttr(string attr) {
            JObject jObject = getJObject();
            JToken value;
            if (jObject.TryGetValue(attr, out value)) {
               return ((JValue)value).Value;
            }
            return null;
         }

         protected string getAsString(string[] fields) {
            string result = null;
            foreach (string field in fields) {
               object value = getAttr(field);
               if (null == value) {
                  continue;
               }
               if (null == result) {
                  result = value.ToString();
               } else {
                  result += ", " + value.ToString();
               }
            }
            return result;
         }

         internal void copyAttrs(UploadItem other, string[] attrs) {
            for (int i = 0; i < attrs.Length; i += 1) {
               setAttr(attrs[i], other.getAttr(attrs[i]));
            }
         }

         internal abstract string getAsString();
      }


      public class PendingUploadItem : UploadItem {

         internal PendingUploadItem() : base() {
         }

         internal PendingUploadItem(JObject jObject) {
            setJObject(jObject);
         }

         internal PendingUploadItem(string fileName, string permission, string title, string description) : base() {
            setAttr("filename", fileName);
            setAttr("permission", permission);
            setAttr("title", title);
            setAttr("description", description);
         }

         internal PendingUploadItem(UploadItem uploadItem) : base() {
            copyAttrs(uploadItem, PendingUploadItemsModel.sAttrs);
         }

         internal override string getAsString() {
            return getAsString(PendingUploadItemsModel.sAttrs);
         }

         internal string getFilename() {
            return (string)getAttr("filename");
         }

         internal string getTitle() {
            return (string)getAttr("title");
         }

         internal string getDescription() {
            return (string)getAttr("description");
         }

         internal string getPermission() {
            return (string)getAttr("permission");
         }

      }

      public class ActiveUploadItem : PendingUploadItem {

         internal static string[] sAttrs = PendingUploadItemsModel.sAttrs;

         internal ActiveUploadItem() : base() {
         }

         internal ActiveUploadItem(UploadItem uploadItem) : base() {
            copyAttrs(uploadItem, PendingUploadItemsModel.sAttrs);
         }

         internal ActiveUploadItem(string fileName, string permission, string title, string description) : 
            base(fileName, permission, title, description) {
         }

         internal ActiveUploadItem(JObject jObject) : base(jObject) {
         }

         private float mProgressPercent;
         private long mComplete, mMax;

         internal void setProgress(float progressPercent, long complete, long max) {
            mProgressPercent = progressPercent;
            mComplete = complete;
            mMax = max;
         }

         internal float getProgressPercent() {
            return mProgressPercent;
         }

         internal long getComplete() {
            return mComplete;
         }

         internal long getMax() {
            return mMax;
         }

         internal override string getAsString() {
            return getAsString(ActiveUploadItem.sAttrs);
         }
      }

      public class FailedUploadItem : PendingUploadItem {

         internal FailedUploadItem() : base() {
         }

         internal FailedUploadItem(string fileName, string permission, string title, string description, string reason) : 
            base(fileName, permission, title, description) {
            setAttr("reason", reason);
         }

         internal FailedUploadItem(ActiveUploadItem item, string reason) : base() {
            copyAttrs(item, PendingUploadItemsModel.sAttrs);
            setAttr("reason", reason);
         }

         internal override string getAsString() {
            return getAsString(FailedUploadItemsModel.sAttrs);
         }
      }

      public class CompletedUploadItem : PendingUploadItem {

         internal CompletedUploadItem() : base() {
         }

         internal CompletedUploadItem(string fileName, string permission, string title, string description) :
            base(fileName, permission, title, description) {
         }

         internal CompletedUploadItem(ActiveUploadItem item) : base() {
            copyAttrs(item, PendingUploadItemsModel.sAttrs);
         }

         internal override string getAsString() {
            return getAsString(PendingUploadItemsModel.sAttrs);
         }
      }

      internal abstract class ItemsModel<T> where T : UploadItem {

         internal readonly List<T> mItems = new List<T>();
         protected readonly UploadVideoManager mUploadVideoManager;

         internal ItemsModel(UploadVideoManager uploadVideoManager) {
            mUploadVideoManager = uploadVideoManager;
         }

         internal bool loadItemsFrom(string data) {
            if (string.Empty.Equals(data)) {
               return false;
            }
            mItems.Clear();
            try {
               JObject jObject = JObject.Parse(data);
               JArray jItems = (JArray)jObject.GetValue("items");
               for (int i = 0; i < jItems.Count; i += 1) {
                  JObject jItem = jItems.Value<JObject>(i);
                  T nItem = newItem();
                  nItem.setJObject(jItem);
                  addItem(nItem);
               }
            } catch (Exception ex) {
               return false;
            }
            onChanged();
            return true;
         }

         internal abstract T newItem();
         internal abstract string[] getAttrs();

         internal virtual void cloneAndAddItem(T item) {
            T clone = newItem();
            clone.copyAttrs(item, getAttrs());
            addItem(clone);
         }

         internal virtual void addItem(T item) {
            mItems.Add(item);
            onChanged();
         }

         internal void removeAllItems() {
            mItems.Clear();
            onChanged();
         }

         internal T getItemAt(int index) {
            if (mItems.Count > 0 && index >= 0 && index < mItems.Count) {
               T item = mItems[index];
               return item;
            }
            return null;
         }

         internal T removeItemAt(int index) {
            if (mItems.Count > 0 && index >= 0 && index < mItems.Count) {
               T item = mItems[index];
               mItems.RemoveAt(index);
               onChanged();
               return item;
            }
            return null;
         }

         internal bool moveItemUp(int index) {
            if (mItems.Count > 0 && index > 0 && index < mItems.Count) {
               T item = mItems[index];
               mItems.RemoveAt(index);
               index -= 1;
               mItems.Insert(index, item);
               onChanged();
               return true;
            }
            return false;
         }

         internal bool moveItemDown(int index) {
            if (mItems.Count > 0 && index >= 0 && index < mItems.Count - 1) {
               T item = mItems[index];
               mItems.RemoveAt(index);
               index += 1;
               mItems.Insert(index, item);
               onChanged();
               return true;
            }
            return false;
         }

         internal string getAsString() {
            JArray jItems = new JArray();

            foreach (T item in mItems) {
               jItems.Add(item.getJObject());
            }
            JObject jObject = new JObject();
            jObject.Add("items", jItems);
            return jObject.ToString();
         }

         abstract internal void save();
         abstract internal void onLoggedIn();
         abstract internal void onLoggedOut();

         virtual internal void onChanged() {
            save();
         }
      }

      private void onPendingItemsChanged() {
         foreach (Callback callback in mCallbacks) {
            callback.onPendingItemsChanged();
         }
      }

      private void onFailedItemsChanged() {
         foreach (Callback callback in mCallbacks) {
            callback.onFailedItemsChanged();
         }
      }

      private void onCompletedItemsChanged() {
         foreach (Callback callback in mCallbacks) {
            callback.onCompletedItemsChanged();
         }
      }

      internal class PendingUploadItemsModel : ItemsModel<PendingUploadItem> {

         internal static string[] sAttrs = { "title", "filename", "permission", "description" };

         internal override string[] getAttrs() {
            return sAttrs;
         }

         internal PendingUploadItemsModel(UploadVideoManager uploadVideoManager) : base(uploadVideoManager) {
         }

         internal override void onLoggedIn() {
            loadItemsFrom(AppSettings.Default.pendingUploadData);
         }

         internal override PendingUploadItem newItem() {
            return new PendingUploadItem();
         }

         internal override void save() {
            AppSettings.Default.pendingUploadData = getAsString();
            AppSettings.Default.Save(); 
         }

         internal override void addItem(PendingUploadItem item) {
            if (mUploadVideoManager.tryUpload(item)) {
               return;
            }
            base.addItem(item);
         }

         internal override void onChanged() {
            base.onChanged();
            mUploadVideoManager.onPendingItemsChanged();
         }

         internal override void onLoggedOut() {
            mItems.Clear();
            mUploadVideoManager.onPendingItemsChanged();
         }
      }

      internal class FailedUploadItemsModel : ItemsModel<FailedUploadItem> {

         internal static string[] sAttrs = { "reason", "title", "filename", "permission", "description" };

         internal override string[] getAttrs() {
            return sAttrs;
         }

         internal FailedUploadItemsModel(UploadVideoManager uploadVideoManager) : base(uploadVideoManager) {
         }

         internal override void save() {
            AppSettings.Default.failedUploadData = getAsString();
            AppSettings.Default.Save();
         }

         internal override void onChanged() {
            base.onChanged();
            mUploadVideoManager.onFailedItemsChanged();
         }

         internal override FailedUploadItem newItem() {
            return new FailedUploadItem();
         }

         internal override void onLoggedIn() {
            loadItemsFrom(AppSettings.Default.failedUploadData);
         }

         internal override void onLoggedOut() {
            mItems.Clear();
            mUploadVideoManager.onFailedItemsChanged();
         }


      }

      internal class CompletedUploadItemsModel : ItemsModel<CompletedUploadItem> {

         internal static string[] sAttrs = { "reason", "title", "filename", "permission", "description" };

         internal override string[] getAttrs() {
            return sAttrs;
         }

         internal CompletedUploadItemsModel(UploadVideoManager uploadVideoManager) : base(uploadVideoManager) {
         }

         internal override void save() {
            AppSettings.Default.completedUploadData = getAsString();
            AppSettings.Default.Save();
         }

         internal override void onChanged() {
            base.onChanged();
            mUploadVideoManager.onCompletedItemsChanged();
         }


         internal override CompletedUploadItem newItem() {
            return new CompletedUploadItem();
         }

         internal override void onLoggedIn() {
            loadItemsFrom(AppSettings.Default.completedUploadData);
         }

         internal override void onLoggedOut() {
            mItems.Clear();
            mUploadVideoManager.onCompletedItemsChanged();
         }

      }
      private ActiveUploadItem mActiveUpload = null;

      internal readonly PendingUploadItemsModel mPendingUploads;
      internal readonly FailedUploadItemsModel mFailedUploads;
      internal readonly CompletedUploadItemsModel mCompletedUploads;

      internal PendingUploadItemsModel getPendingUploadsModel() {
         return mPendingUploads;
      }

      internal CompletedUploadItemsModel getCompletedUploadsModel() {
         return mCompletedUploads;
      }

      internal FailedUploadItemsModel getFailedUploadsModel() {
         return mFailedUploads;
      }

      private bool mIsNetworkAvailable = false;
      private Thread mConnectivityCheckerThread;

      void onNetworkAvailabilityChanged(object sender, NetworkAvailabilityEventArgs e) {
         mIsNetworkAvailable = e.IsAvailable;
      }

      private bool mThreadCanContinue = true;
      private bool mIsVRReachable = false;

      private static readonly string TAG = Util.getLogTag(typeof(UploadVideoManager));

      void connectivityCheckerThreadProc() {
         try { 
            while (mThreadCanContinue) {
               Log.d(TAG, "Checking VR connectivity");
               Thread.Sleep(5000);
                
               if (!mIsNetworkAvailable) {
                  mIsVRReachable = false;
                  continue;
               }
               string endPoint = SDKLib.VR.getEndPoint();
               Log.d(TAG, "VR reachable: " + mIsVRReachable + " network reachable: " + mIsNetworkAvailable + " ep: " + endPoint);
               if (null == endPoint) {
                  mIsVRReachable = false;
                  continue;
               }
               char[] ch = endPoint.ToCharArray();
               int index;
               for (index = ch.Length - 1; index >= 0; index -= 1) {
                  if (ch[index] == '/') {
                     break;
                  }
               }
               if (-1 != index) {
                  string temp = endPoint.Remove(index) + "/ccheck";
                  Log.d(TAG, "VR reachable check endpoint: " + temp);
                  try {
                     WebRequest request = WebRequest.Create(temp);
                     HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                  
                     if (response != null) {
                        HttpStatusCode stc = response.StatusCode;
                        response.Close();
                        if (HttpStatusCode.OK == stc) {
                           mIsVRReachable = true;
                        }
                        continue;
                     }
                  } catch (Exception ex) {
                     continue;
                  }
               }
               mIsVRReachable = false;
            }
         } catch (ThreadInterruptedException ex) {
            Log.d(TAG, "Connectivity checker exception: " + ex);
         }
         Log.d(TAG, "Ending connectivity checker");
      }

      public void destroy() {
         mThreadCanContinue = false;
         mConnectivityCheckerThread.Abort();
         mConnectivityCheckerThread.Join();
      }

      private readonly App mApp;

      internal UploadVideoManager(App app) {

         NetworkChange.NetworkAvailabilityChanged += onNetworkAvailabilityChanged;
         mIsNetworkAvailable = System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable();
         mConnectivityCheckerThread = new Thread(new ThreadStart(connectivityCheckerThreadProc));
         mConnectivityCheckerThread.Start();

         mApp = app;

         mFailedUploads = new FailedUploadItemsModel(this);
         mPendingUploads = new PendingUploadItemsModel(this);
         mCompletedUploads = new CompletedUploadItemsModel(this);
      }

      private readonly List<Callback> mCallbacks = new List<Callback>();

      internal void addCallback(Callback callback) {
         mCallbacks.Add(callback);
      }

      internal void removeCallback(Callback callback) {
         mCallbacks.Remove(callback);
      }

      private UserVideo.If mVideo = null;
      private Stream mSource;

      public void onVideoIdAvailable(object closure, SDKLib.UserVideo.If video) {
         mVideo = video;
         saveInProgressVideo();
      }

      public void onCancelled(object closure) {
         mFailedUploads.addItem(new FailedUploadItem(mActiveUpload, ResourceStrings.uploadCancelled));
         onUploadComplete();
      }

      private void onUploadComplete() {
         foreach (Callback callback in mCallbacks) {
            callback.onEndUpload();
         }
         mVideo = null;
         mActiveUpload = null;
         saveInProgressVideo();
         if (null != mSource) {
            mSource.Close();
            mSource = null;
         }
         PendingUploadItem nextItem = mPendingUploads.getItemAt(0);
         if (null != nextItem && tryUpload(nextItem)) {
            mPendingUploads.removeItemAt(0);
         }
      }

      public void onException(object closure, Exception ex) {
         mFailedUploads.addItem(new FailedUploadItem(mActiveUpload, string.Format(ResourceStrings.uploadFailedWithException, ex.ToString())));
         onUploadComplete();
      }

      public void onFailure(object closure, int status) {
         mFailedUploads.addItem(new FailedUploadItem(mActiveUpload, string.Format(ResourceStrings.uploadFailedWithStatus, status)));
         onUploadComplete();
      }

      public void onProgress(object closure, float progressPercent, long complete, long max) {
         if (null != mActiveUpload) {
            mActiveUpload.setProgress(progressPercent, complete, max);
            saveInProgressVideo();

            foreach (Callback callback in mCallbacks) {
               callback.onUploadProgress(progressPercent, complete, max);
            }
         }
      }

      public void onProgress(object closure, long complete) {
         onProgress(closure, -1, complete, -1);
      }

      public void onSuccess(object closure) {
         mCompletedUploads.addItem(new CompletedUploadItem(mActiveUpload));
         onUploadComplete();
      }


      private bool tryUpload(PendingUploadItem item) {
         return tryUpload(new ActiveUploadItem(item), null);
      }

      private bool tryUpload(ActiveUploadItem aui, JObject userVideo) {
         User.If user = mApp.getUser();
         if (null == user || null != mActiveUpload) {
            return false;
         }

         string fileName = aui.getFilename();
         if (string.IsNullOrEmpty(fileName)) {
            FailedUploadItem failedItem = new FailedUploadItem(aui, ResourceStrings.uploadFileDoesNotExist);
            mFailedUploads.addItem(failedItem);
            return true;
         }
         FileInfo fileInfo = new FileInfo(fileName);
         if (!fileInfo.Exists) {
            mFailedUploads.addItem(new FailedUploadItem(aui, ResourceStrings.uploadFileDoesNotExist));
            return true;
         }
         FileStream stream = null;
         try {
            stream = fileInfo.OpenRead();
         } catch (Exception) {
            mFailedUploads.addItem(new FailedUploadItem(aui, ResourceStrings.uploadFileOpenFailed));
            return true;
         }
         long length = fileInfo.Length;
         bool uploadQueued = false;

         if (null == userVideo) {
            SDKLib.UserVideo.Permission permission = SDKLib.UserVideo.fromString(aui.getPermission());
            uploadQueued = user.uploadVideo(stream, length, aui.getTitle(), aui.getDescription(), 
               permission, this, App.getInstance().getHandler(), this);
         } else {
            uploadQueued = user.uploadVideo(stream, length, userVideo, this, App.getInstance().getHandler(), this);
         }
         if (uploadQueued) {
            mActiveUpload = aui;
            mSource = stream;

            saveInProgressVideo();

            foreach (Callback callback in mCallbacks) {
               callback.onBeginUpload();
            }
            return true;
         }
         mFailedUploads.addItem(new FailedUploadItem(aui, ResourceStrings.unableToQueueRequest));
         return true;
      }

      internal ActiveUploadItem getActiveUpload() {
         return mActiveUpload;
      }

      internal bool cancelActiveUpload() {
         if (null != mVideo) {
            return mVideo.cancelUpload(this);
         }
         SDKLib.User.If user = mApp.getUser();
         if (null == user) {
            return false;
         }
         return user.cancelUploadVideo(this);
      }

      internal void onLoggedIn() {
         try {
            JObject value = JObject.Parse(AppSettings.Default.inProgressUpload);
            if (null != value) {
               JToken file;
               if (value.TryGetValue("file", out file)) {
                  ActiveUploadItem aui = new ActiveUploadItem((JObject)file);
                  JToken video;
                  if (value.TryGetValue("video", out video)) {
                     tryUpload(aui, (JObject)video);
                  }
               }               
            }
         } catch (Exception ex) {
         }

         mFailedUploads.onLoggedIn();
         mCompletedUploads.onLoggedIn();
         mPendingUploads.onLoggedIn();
      }

      internal void onLoggedOut() {
         mFailedUploads.onLoggedOut();
         mCompletedUploads.onLoggedOut();
         mPendingUploads.onLoggedOut();
      }

      private void saveInProgressVideo() {
         JObject value = new JObject();
         if (null != mActiveUpload) {
            value.Add("file", mActiveUpload.getJObject());
         }
         if (null != mVideo) {
            value.Add("video", mVideo.toJObject(null));
         }
         string result = value.ToString();
         Log.d(TAG, "Save in progress video: " + result);
         AppSettings.Default.inProgressUpload = result;
         AppSettings.Default.Save();
      }

      internal bool moveFailedToPending(int index) {
         if (mFailedUploads.mItems.Count > 0 && index >= 0 && index < mFailedUploads.mItems.Count) {
            FailedUploadItem item = mFailedUploads.removeItemAt(index);
            if (null != item) { 
               mPendingUploads.cloneAndAddItem(item);
            }
         }
         return true;
      }

      internal bool moveCompletedToPending(int index) {
         if (mCompletedUploads.mItems.Count > 0 && index >= 0 && index < mCompletedUploads.mItems.Count) {
            CompletedUploadItem item = mCompletedUploads.removeItemAt(index);
            if (null != item) {
               mPendingUploads.cloneAndAddItem(item);
            }
         }
         return true;
      }
   }
}
