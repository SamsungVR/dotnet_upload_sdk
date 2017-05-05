using Newtonsoft.Json.Linq;
using SDKLib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.NetworkInformation;
using System.Threading;
using System.Windows.Forms;

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

         internal void setAttr(String attr, object value) {
            JObject jObject = getJObject();
            JToken currentValue = null;

            if (jObject.TryGetValue(attr, out currentValue)) {
               ((JValue)currentValue).Value = value;
            } else {
               jObject.Add(attr, new JValue(value));
            }
         }

         internal object getAttr(String attr) {
            JObject jObject = getJObject();
            JToken value;
            if (jObject.TryGetValue(attr, out value)) {
               return ((JValue)value).Value;
            }
            return null;
         }

         protected String getAsString(String[] fields) {
            String result = null;
            foreach (String field in fields) {
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

         internal void copyAttrs(UploadItem other, String[] attrs) {
            for (int i = 0; i < attrs.Length; i += 1) {
               setAttr(attrs[i], other.getAttr(attrs[i]));
            }
         }

         internal abstract String getAsString();
      }


      public class PendingUploadItem : UploadItem {

         internal PendingUploadItem() : base() {
         }

         internal PendingUploadItem(String fileName, String permission, String title, String description) : base() {
            setAttr("filename", fileName);
            setAttr("permission", permission);
            setAttr("title", title);
            setAttr("description", description);
         }

         internal PendingUploadItem(UploadItem uploadItem) : base() {
            copyAttrs(uploadItem, PendingUploadItemsModel.sAttrs);
         }

         internal override String getAsString() {
            return getAsString(PendingUploadItemsModel.sAttrs);
         }

         internal String getFilename() {
            return (String)getAttr("filename");
         }

         internal String getTitle() {
            return (String)getAttr("title");
         }

         internal String getDescription() {
            return (String)getAttr("description");
         }

         internal String getPermission() {
            return (String)getAttr("permission");
         }

      }

      public class ActiveUploadItem : PendingUploadItem {

         internal static String[] sAttrs = { "title", "filename", "permission", "description", "chunk_complete", "num_chunks" };

         internal ActiveUploadItem() : base() {
         }

         internal ActiveUploadItem(UploadItem uploadItem) : base() {
            copyAttrs(uploadItem, PendingUploadItemsModel.sAttrs);
         }

         internal ActiveUploadItem(String fileName, String permission, String title, String description) : 
            base(fileName, permission, title, description) {
         }

         private float mProgressPercent;
         private long mComplete, mMax;

         internal void setProgress(float progressPercent, long complete, long max) {
            setAttr("chunk_complete", complete);
            setAttr("num_chunks", max);
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

         internal override String getAsString() {
            return getAsString(ActiveUploadItem.sAttrs);
         }
      }

      public class FailedUploadItem : PendingUploadItem {

         internal FailedUploadItem() : base() {
         }

         internal FailedUploadItem(String fileName, String permission, String title, String description, String reason) : 
            base(fileName, permission, title, description) {
            setAttr("reason", reason);
         }

         internal FailedUploadItem(ActiveUploadItem item, String reason) : base() {
            copyAttrs(item, PendingUploadItemsModel.sAttrs);
            setAttr("reason", reason);
         }

         internal override String getAsString() {
            return getAsString(FailedUploadItemsModel.sAttrs);
         }

      }

      public class CompletedUploadItem : PendingUploadItem {

         internal CompletedUploadItem() : base() {
         }

         internal CompletedUploadItem(String fileName, String permission, String title, String description) :
            base(fileName, permission, title, description) {
         }

         internal CompletedUploadItem(ActiveUploadItem item) : base() {
            copyAttrs(item, PendingUploadItemsModel.sAttrs);
         }

         internal override String getAsString() {
            return getAsString(PendingUploadItemsModel.sAttrs);
         }

      }

      internal abstract class ItemsModel<T> where T : UploadItem {

         internal readonly List<T> mItems = new List<T>();
         protected readonly UploadVideoManager mUploadVideoManager;

         internal ItemsModel(UploadVideoManager uploadVideoManager) {
            mUploadVideoManager = uploadVideoManager;
         }

         internal bool loadItemsFrom(String data) {
            if (String.Empty.Equals(data)) {
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
         internal abstract String[] getAttrs();

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

         internal String getAsString() {
            JArray jItems = new JArray();

            foreach (T item in mItems) {
               jItems.Add(item.getJObject());
            }
            JObject jObject = new JObject();
            jObject.Add("items", jItems);
            return jObject.ToString();
         }

         abstract internal void save();

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

         internal static String[] sAttrs = { "title", "filename", "permission", "description" };

         internal override String[] getAttrs() {
            return sAttrs;
         }

         internal PendingUploadItemsModel(UploadVideoManager uploadVideoManager) : base(uploadVideoManager) {
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

      }

      internal class FailedUploadItemsModel : ItemsModel<FailedUploadItem> {

         internal static String[] sAttrs = { "reason", "title", "filename", "permission", "description" };

         internal override String[] getAttrs() {
            return sAttrs;
         }

         internal FailedUploadItemsModel(UploadVideoManager uploadVideoManager) : base(uploadVideoManager) {
            loadItemsFrom(AppSettings.Default.failedUploadData);
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
            throw new NotImplementedException();
         }
      }

      internal class CompletedUploadItemsModel : ItemsModel<CompletedUploadItem> {

         internal static String[] sAttrs = { "reason", "title", "filename", "permission", "description" };

         internal override String[] getAttrs() {
            return sAttrs;
         }

         internal CompletedUploadItemsModel(UploadVideoManager uploadVideoManager) : base(uploadVideoManager) {
            loadItemsFrom(AppSettings.Default.completedUploadData);
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
               String endPoint = SDKLib.VR.getEndPoint();
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
                  String temp = endPoint.Remove(index) + "/ccheck";
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
         mApp = app;
         mFailedUploads = new FailedUploadItemsModel(this);
         mPendingUploads = new PendingUploadItemsModel(this);
         mCompletedUploads = new CompletedUploadItemsModel(this);

         NetworkChange.NetworkAvailabilityChanged += onNetworkAvailabilityChanged;
         mIsNetworkAvailable = System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable();
         mConnectivityCheckerThread = new Thread(new ThreadStart(connectivityCheckerThreadProc));
         mConnectivityCheckerThread.Start();
      }

      private readonly List<Callback> mCallbacks = new List<Callback>();

      internal void addCallback(Callback callback) {
         mCallbacks.Add(callback);
      }

      internal void removeCallback(Callback callback) {
         mCallbacks.Remove(callback);
      }

      private SDKLib.UserVideo.If mVideo;
      private Stream mSource;
      private long mLength;

      public void onVideoIdAvailable(object closure, SDKLib.UserVideo.If video) {
         mVideo = video;
      }

      public void onCancelled(object closure) {
         mFailedUploads.addItem(new FailedUploadItem(mActiveUpload, ResourceStrings.uploadCancelled));
         onUploadComplete();
      }

      private void onUploadComplete() {
         foreach (Callback callback in mCallbacks) {
            callback.onEndUpload();
         }
         AppSettings.Default.inProgressUpload = new JObject().ToString();
         AppSettings.Default.Save();

         mVideo = null;
         mActiveUpload = null;
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
            AppSettings.Default.inProgressUpload = mActiveUpload.getJObject().ToString();
            AppSettings.Default.Save();

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
         if (null != mActiveUpload) {
            return false;
         }
         SDKLib.User.If user = mApp.getUser();
         if (null == user) {
            return false;
         }
         ActiveUploadItem aui = new ActiveUploadItem(item);
         String fileName = aui.getFilename();
         if (String.IsNullOrEmpty(fileName)) {
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
         SDKLib.UserVideo.Permission permission = SDKLib.UserVideo.fromString(aui.getPermission());
         if (user.uploadVideo(stream, length, aui.getTitle(), aui.getDescription(), 
            permission, this, App.getInstance().getHandler(), this)) {
            mActiveUpload = aui;
            mSource = stream;
            mLength = length;
            AppSettings.Default.inProgressUpload = mActiveUpload.getJObject().ToString();
            AppSettings.Default.Save();
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
         PendingUploadItem nextItem = mPendingUploads.getItemAt(0);
         if (null != nextItem && tryUpload(nextItem)) {
            mPendingUploads.removeItemAt(0);
         }
      }

      internal void onLoggedOut() {

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
