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

      public enum UploadState {
         BEGIN,
         PAUSED,
         RESUMED,
         END
      }
      
      internal interface Callback {
         void onServiceReachable(bool reachable);
         void onUploadStateChanged(UploadState state);
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
         internal readonly UploadVideoManager mUploadManager;
         internal readonly User.If mUser;

         internal ActiveUploadItem(User.If user, UploadVideoManager manager) : base() {
            mUploadManager = manager;
            mUser = user;
         }

         internal ActiveUploadItem(User.If user, UploadVideoManager videoManager, UploadItem uploadItem) : this(user, videoManager) {
            copyAttrs(uploadItem, PendingUploadItemsModel.sAttrs);
         }

         internal ActiveUploadItem(User.If user, UploadVideoManager uploadManager,
               string fileName, string permission, string title, string description) : 
            base(fileName, permission, title, description) {
            mUser = user;
            mUploadManager = uploadManager;
         }

         internal ActiveUploadItem(User.If user, UploadVideoManager videoManager, JObject jObject) : this(user, videoManager) {
            JToken file;
            if (jObject.TryGetValue("file", out file)) {
               setJObject((JObject)file);
               JToken video;
               if (jObject.TryGetValue("video", out video)) {
                  mVideoJson = (JObject)video;
               }
            }
         }

         private float mProgressPercent;
         private long mComplete, mMax;
         private JObject mVideoJson;
         private int mRetryCount = 5;

         internal void destroy() {
            if (null != mSource) {
               mSource.Close();
               mSource = null;
            }
         }

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

         private UserVideo.If mVideo = null;
         private Stream mSource = null;
         private long mSourceLength;

         internal void onVideoIdAvailable(SDKLib.UserVideo.If video) {
            mVideo = video;
         }

         internal FailedUploadItem tryUpload() {
            if (mRetryCount < 0) {
               return new FailedUploadItem(this, ResourceStrings.uploadFailedTooManyAttempts);
            }
            mRetryCount -= 1;
            SynchronizationContext handler = App.getInstance().getHandler();
            SDKLib.UserVideo.Permission permission = SDKLib.UserVideo.fromString(getPermission());
            User.If user = mUser;

            if (null != mSource) {
               if (null != mVideo) {
                  if (mVideo.retryUpload(mSource, mSourceLength, mUploadManager, handler, this)) {
                     return null;
                  }
                  return new FailedUploadItem(this, ResourceStrings.unableToQueueRequest);
               }
               if (null != mVideoJson) {
                  if (user.uploadVideo(mSource, mSourceLength, mVideoJson, mUploadManager, handler, this)) {
                     return null;
                  }
                  return new FailedUploadItem(this, ResourceStrings.unableToQueueRequest);
               }
               
               if (user.uploadVideo(mSource, mSourceLength, getTitle(), getDescription(),
                     permission, mUploadManager, handler, this)) {
                  return null;
               }
               return new FailedUploadItem(this, ResourceStrings.unableToQueueRequest);
            }

            string fileName = getFilename();
            if (string.IsNullOrEmpty(fileName)) {
               return new FailedUploadItem(this, ResourceStrings.uploadFileDoesNotExist);
            }
            FileInfo fileInfo = new FileInfo(fileName);
            if (!fileInfo.Exists) {
               return new FailedUploadItem(this, ResourceStrings.uploadFileDoesNotExist);
            }
            try {
               mSource = fileInfo.OpenRead();
            } catch (Exception) {
               mSource = null;
               return new FailedUploadItem(this, ResourceStrings.uploadFileOpenFailed);
            }
            mSourceLength = fileInfo.Length;
            if (user.uploadVideo(mSource, mSourceLength, getTitle(), getDescription(),
                     permission, mUploadManager, handler, this)) {
               return null;
            }
            return new FailedUploadItem(this, ResourceStrings.uploadRetryFailed);
         }

         internal bool cancelUpload() {
            if (null != mVideo) {
               return mVideo.cancelUpload(this);
            }
            if (null == mUser) {
               return false;
            }
            return mUser.cancelUploadVideo(this);
         }

         internal JObject getPersistableJObject() {
            JObject result = new JObject();
            result.Add("time", new JValue(DateTime.Now));
            result.Add("file", getJObject());
            if (null != mVideo) {
               result.Add("video", mVideo.toJObject(null));
            }
            return result;
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

      private volatile bool mThreadCanContinue = true;

      private static readonly string TAG = Util.getLogTag(typeof(UploadVideoManager));

      public virtual void connectivityChecksBegin(object args) {
         Log.d(TAG, "Connectivity checks begin");
      }

      public virtual void connectivityChecksEnd(object args) {
         Log.d(TAG, "Connectivity checks end");
      }

      private void clearFailedUploadFlags() {
         mUploadException = null;
         mUploadFailureStatus = -1;
      }

      private bool mIsServiceReachable = false;

      internal bool isServiceReachable() {
         return mIsServiceReachable;
      }

      public virtual void connectivityNotification(object args) {
         mIsServiceReachable = (bool)args;
         if (DEBUG) {
            Log.d(TAG, "VR reachable: " + mIsServiceReachable + " ep: " + VR.getEndPoint() 
               + " ex: " + mUploadException + " status: " + mUploadFailureStatus + " active: " + mActiveUpload);
         }
         foreach (Callback callback in mCallbacks) {
            callback.onServiceReachable(mIsServiceReachable);
         }
         if (!mIsServiceReachable || null == mActiveUpload || (null == mUploadException && -1 == mUploadFailureStatus)) {
            return;
         }
         if ((User.Result.UploadVideo.STATUS_OUT_OF_UPLOAD_QUOTA == mUploadFailureStatus) ||
            (User.Result.UploadVideo.STATUS_BAD_FILE_LENGTH == mUploadFailureStatus) ||
            (User.Result.UploadVideo.STATUS_FILE_LENGTH_TOO_LONG == mUploadFailureStatus) ||
            (User.Result.UploadVideo.STATUS_INVALID_STEREOSCOPIC_TYPE == mUploadFailureStatus) ||
            (User.Result.UploadVideo.STATUS_INVALID_AUDIO_TYPE == mUploadFailureStatus)) {
            mFailedUploads.addItem(new FailedUploadItem(mActiveUpload, 
               string.Format(ResourceStrings.uploadFailedWithStatus, mUploadFailureStatus)));
            onEndUpload();
            return;
         }
         FailedUploadItem failure = mActiveUpload.tryUpload();
         if (null == failure) {
            onResumeUpload();
            return;
         }
         if (null != mUploadException) {
            mFailedUploads.addItem(new FailedUploadItem(mActiveUpload, string.Format(ResourceStrings.uploadFailedWithException, 
               mUploadException.ToString())));
         } else if (-1 != mUploadFailureStatus) {
            mFailedUploads.addItem(new FailedUploadItem(mActiveUpload, string.Format(ResourceStrings.uploadFailedWithStatus, mUploadFailureStatus)));
         }
         onEndUpload();
      }

      internal void connectivityCheckerThreadProc() {
         SynchronizationContext handler = mApp.getHandler();
         handler.Post(connectivityChecksBegin, null);
         try { 
            while (mThreadCanContinue) {
               bool reachable = false;

               string endPoint = SDKLib.VR.getEndPoint();
               if (null != endPoint) {
                  char[] ch = endPoint.ToCharArray();
                  int index;
                  for (index = ch.Length - 1; index >= 0; index -= 1) {
                     if (ch[index] == '/') {
                        break;
                     }
                  }
                  if (-1 != index) {
                     string temp = endPoint.Remove(index) + "/ccheck";
                     try {
                        WebRequest request = WebRequest.Create(temp);
                        request.Timeout = 1500;

                        if (request is HttpWebRequest) {
                           HttpWebRequest httpWebRequest = (HttpWebRequest)request;
                           httpWebRequest.ReadWriteTimeout = request.Timeout;
                           httpWebRequest.AllowWriteStreamBuffering = false;
                           httpWebRequest.AllowAutoRedirect = false;
                           httpWebRequest.KeepAlive = false;
                        }
                        WebResponse response = request.GetResponse();
                        if (null != response) {
                           HttpStatusCode stc = HttpStatusCode.NoContent;
                           if (response is HttpWebResponse) {
                              HttpWebResponse httpResponse = (HttpWebResponse)response;
                              stc = httpResponse.StatusCode;
                           }
                           response.Close();
                           if (HttpStatusCode.OK == stc) {
                              reachable = true;
                           }
                        }
                     } catch (Exception ex) {
                        Log.d(TAG, "VR reachable check exception: " + DateTime.Now.ToLongTimeString() + " " + ex);
                     }
                  }
               }
               handler.Post(connectivityNotification, reachable);
               Thread.Sleep(reachable ? 5000 : 1000);
            }
         } catch (ThreadInterruptedException ex) {
            Log.d(TAG, "Connectivity checker exception: " + ex);
         }
         handler.Post(connectivityChecksEnd, null);
         Log.d(TAG, "Ending connectivity checker");
      }

      public void destroy() {
         mThreadCanContinue = false;
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
      private Exception mUploadException = null;
      private int mUploadFailureStatus = -1;

      internal void addCallback(Callback callback) {
         mCallbacks.Add(callback);
      }

      internal void removeCallback(Callback callback) {
         mCallbacks.Remove(callback);
      }

      public void onException(object closure, Exception ex) {
         mUploadException = ex;
         setUploadState(UploadState.PAUSED);
      }

      public void onFailure(object closure, int status) {
         mUploadFailureStatus = status;
         setUploadState(UploadState.PAUSED);
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
         onEndUpload();
      }

      public void onVideoIdAvailable(object closure, SDKLib.UserVideo.If video) {
         mActiveUpload.onVideoIdAvailable(video);
         saveInProgressVideo();
      }

      public void onCancelled(object closure) {
         mFailedUploads.addItem(new FailedUploadItem(mActiveUpload, ResourceStrings.uploadCancelled));
         onEndUpload();
      }

      internal UploadState getUploadState() {
         return mUploadState;
      }

      private UploadState mUploadState = UploadState.END;

      private void setUploadState(UploadState state) {
         if (state == mUploadState) {
            return;
         }
         mUploadState = state;
         foreach (Callback callback in mCallbacks) {
            callback.onUploadStateChanged(mUploadState);
         }
      }

      private void onEndUpload() {
         clearFailedUploadFlags();
         setUploadState(UploadState.END);
         if (null != mActiveUpload) {
            mActiveUpload.destroy();
            mActiveUpload = null;
            saveInProgressVideo();
         }
         PendingUploadItem nextItem = mPendingUploads.getItemAt(0);
         while (null != nextItem && null == mActiveUpload && tryUpload(nextItem)) {
            mPendingUploads.removeItemAt(0);
            nextItem = mPendingUploads.getItemAt(0);
         }
      }

      private void onResumeUpload() {
         setUploadState(UploadState.RESUMED);
         clearFailedUploadFlags();
      }

      internal void onBeginUpload() {
         clearFailedUploadFlags();
         setUploadState(UploadState.BEGIN);
      }

      private bool tryUpload(PendingUploadItem item) {
         return tryUpload(new ActiveUploadItem(mApp.getUser(), this, item));
      }

      private bool tryUpload(ActiveUploadItem aui) {
         if (null != mActiveUpload) {
            return false;
         }
         User.If user = mApp.getUser();
         if (null == user || null == aui) {
            return false;
         }
         FailedUploadItem failure = aui.tryUpload();
         if (null == failure) {
            mActiveUpload = aui;
            saveInProgressVideo();
            onBeginUpload();
            return true;
         }
         mFailedUploads.addItem(failure);
         return true;
      }

      internal ActiveUploadItem getActiveUpload() {
         return mActiveUpload;
      }

      internal bool cancelActiveUpload() {
         if (null != mActiveUpload) {
            if (!mActiveUpload.cancelUpload()) {
               onCancelled(this);
            }
         }
         return false;
      }

      internal void onLoggedIn() {
         try {
            JObject value = JObject.Parse(AppSettings.Default.inProgressUpload);
            JToken check = null;
            if (null != value && value.TryGetValue("time", out check)) {
               ActiveUploadItem aui = new ActiveUploadItem(mApp.getUser(), this, value);
               tryUpload(aui);
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

      private const bool DEBUG = false;

      private void saveInProgressVideo() {
         JObject value = null;
         if (null != mActiveUpload) {
            value = mActiveUpload.getPersistableJObject();
         } else {
            value = null;
         }
         string result = null;
         if (null != value) {
            result = value.ToString();
         }
         if (DEBUG) {
            Log.d(TAG, "Save in progress video: " + result);
         }
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
