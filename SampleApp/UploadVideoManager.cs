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
         void onUploadProgress(float progressPercent, long complete, long max);
         void onPendingItemsChanged();
         void onFailedItemsChanged();
      }

      public abstract class UploadItem {

         protected JObject mJObject;

         internal UploadItem(JObject jObject) {
            mJObject = jObject;
         }

         internal UploadItem() {
            mJObject = new JObject();
         }

         internal JObject getJObject() {
            return mJObject;
         }

         protected void setAttr(String attr, String value) {
            mJObject.Add(attr, new JValue(value));
         }

         protected String getAttr(String attr) {
            JToken value;
            if (mJObject.TryGetValue(attr, out value)) {
               return value.ToString();
            }
            return null;
         }

         protected String getAsString(params String[] fields) {
            String result = null;
            foreach (String field in fields) {
               if (null == result) {
                  result = getAttr(field);
               } else {
                  result += ", " + getAttr(field);
               }
            }
            return result;
         }

         internal abstract String getAsString();
      }


      public class PendingUploadItem : UploadItem {

         internal PendingUploadItem(JObject item) : base(item) {
         }

         internal PendingUploadItem(String fileName, String permission, String title, String description) : base() {
            setAttr("filename", fileName);
            setAttr("permission", permission);
            setAttr("title", title);
            setAttr("description", description);
         }

         internal override String getAsString() {
            return getAsString("title", "filename");
         }

         internal String getFilename() {
            return getAttr("filename");
         }

         internal String getTitle() {
            return getAttr("title");
         }

         internal String getDescription() {
            return getAttr("description");
         }

         internal String getPermission() {
            return getAttr("permission");
         }

      }

      public class ActiveUploadItem : PendingUploadItem {

         internal ActiveUploadItem(JObject item) : base(item) {
         }

         internal ActiveUploadItem(String fileName, String permission, String title, String description) : base(fileName, permission, title, description) {
         }

         internal ActiveUploadItem(PendingUploadItem item) : this(
            item.getFilename(), item.getPermission(), item.getTitle(), item.getDescription()) {
         }

      }

      public class FailedUploadItem : PendingUploadItem {

         internal FailedUploadItem(JObject item) : base(item) {
         }

         internal FailedUploadItem(String fileName, String permission, String title, String description, String reason) : base(fileName, permission, title, description) {
            setAttr("reason", reason);
         }

         internal FailedUploadItem(ActiveUploadItem item, String reason) : this(item.getFilename(), item.getPermission(), item.getTitle(), item.getDescription(), reason) {
         }

         internal override String getAsString() {
            return getAsString("title", "filename", "reason");
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
                  T newItem = newItemFromJObject(jItem);
                  addItem(newItem);
               }
            } catch (Exception ex) {
               return false;
            }
            onChanged();
            return true;
         }

         internal abstract T newItemFromJObject(JObject jObject);

         internal virtual void addItem(T item) {
            mItems.Add(item);
            onChanged();
         }

         internal void removeAllItems() {
            mItems.Clear();
            onChanged();
         }


         internal void removeItemAt(int index) {
            if (mItems.Count > 0 && index >= 0 && index < mItems.Count) {
               mItems.RemoveAt(index);
               onChanged();
            }
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

      internal class PendingUploadItemsModel : ItemsModel<PendingUploadItem> {

         internal PendingUploadItemsModel(UploadVideoManager uploadVideoManager) : base(uploadVideoManager) {
            loadItemsFrom(AppSettings.Default.pendingUploadData);
         }

         internal override PendingUploadItem newItemFromJObject(JObject jObject) {
            return new PendingUploadItem(jObject);
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

         internal FailedUploadItemsModel(UploadVideoManager uploadVideoManager) : base(uploadVideoManager) {
            loadItemsFrom(AppSettings.Default.failedUploadData);
         }

         internal override FailedUploadItem newItemFromJObject(JObject jObject) {
            return new FailedUploadItem(jObject);
         }

         internal override void save() {
            AppSettings.Default.failedUploadData = getAsString();
            AppSettings.Default.Save();
         }

         internal override void onChanged() {
            base.onChanged();
            mUploadVideoManager.onFailedItemsChanged();
         }
      }

      private ActiveUploadItem mActiveUpload = null;

      private readonly PendingUploadItemsModel mPendingUploads;
      private readonly FailedUploadItemsModel mFailedUploads;

      internal PendingUploadItemsModel getPendingUploadsModel() {
         return mPendingUploads;
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
         onUploadComplete();
         //ctrlUploadStatus.Text = ResourceStrings.uploadCancelled;
      }

      private void onUploadComplete() {
         mVideo = null;
         if (null != mSource) {
            mSource.Close();
            mSource = null;
         }
      }

      public void onException(object closure, Exception ex) {
      }

      public void onFailure(object closure, int status) {
      }

      public void onProgress(object closure, float progressPercent, long complete, long max) {
         foreach (Callback callback in mCallbacks) {
            callback.onUploadProgress(progressPercent, complete, max);
         }
      }

      public void onProgress(object closure, long complete) {
         foreach (Callback callback in mCallbacks) {
            callback.onUploadProgress(-1, complete, -1);
         }
      }

      public void onSuccess(object closure) {
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

   }
}
