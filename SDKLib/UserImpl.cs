using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json.Linq;
using System.Threading;
using System.IO;
using System.Net;
using System.Security.Cryptography;

namespace SDKLib {


   internal class UserImpl : ContainedContainer.BaseImpl<User.Observer>, User.If {

      private static readonly String TAG = Util.getLogTag(typeof(UserImpl));

      public static readonly string HEADER_SESSION_TOKEN = "X-SESSION-TOKEN";
      private static readonly string PROP_USER_ID = "user_id", PROP_NAME = "name", PROP_EMAIL = "email",
         PROP_SESSION_TOKEN = "session_token", PROP_PROFILE_PIC = "profile_pic";

      public class CType : Contained.CType {

         public CType()
            : base(new string[] { PROP_USER_ID, PROP_NAME, PROP_EMAIL, PROP_SESSION_TOKEN, PROP_PROFILE_PIC }) {
         }

         public override SDKLib.Contained.If newInstance(Container.If container, JObject jsonObject) {
            return new UserImpl((APIClientImpl)container, jsonObject);
         }

         public override object getContainedId(JObject jsonObject) {
            return jsonObject.GetValue(PROP_USER_ID);
         }

         public override void notifyCreate(object callback, Container.If container, SDKLib.Contained.If contained) {
         }

         public override void notifyUpdate(object callback, Container.If container, SDKLib.Contained.If contained) {
         }

         public override void notifyDelete(object callback, Container.If container, SDKLib.Contained.If contained) {
         }

         public override void notifyQueried(object callback, Container.If container, SDKLib.Contained.If contained) {

         }

         public override void notifyListQueried(object callback, Container.If container, List<SDKLib.Contained.If> contained) {
         }

         public override object validateValue(string key, JToken token, object rawValue) {
            if (null == rawValue) {
               return null;
            }
            if (PROP_USER_ID.Equals(key) || PROP_NAME.Equals(key) || PROP_EMAIL.Equals(key) ||
               PROP_SESSION_TOKEN.Equals(key) || PROP_PROFILE_PIC.Equals(key)) {
               return (string)rawValue;
            }
            return null;
         }
      }

      public static readonly CType sType = new CType();
      private MD5 mMD5Digest = null;

      private UserImpl(APIClientImpl apiClient, JObject jsonObject)
         : base(new CType(), apiClient, jsonObject, false) {
         mMD5Digest = MD5.Create();
      }

      public MD5 getMD5Digest() {
         return mMD5Digest;
      }


      public override object containedGetIdLocked() {
         return getLocked(PROP_USER_ID);
      }


      public override void containedOnCreateInServiceLocked() {
      }

      public override void containedOnDeleteFromServiceLocked() {
      }

      public override void containedOnUpdateToServiceLocked() {
      }

      public override bool containedOnQueryFromServiceLocked(JObject jsonObject) {
         return processQueryFromServiceLocked(jsonObject);
      }

      public string getSessionToken() {
         return (string)getLocked(PROP_SESSION_TOKEN);
      }

      public string getName() {
         return (string)getLocked(PROP_NAME);
      }

      public string getEmail() {
         return (string)getLocked(PROP_EMAIL);
      }

      public string getProfilePicUrl() {
         return (string)getLocked(PROP_PROFILE_PIC);
      }

      public string getUserId() {
         return (string)getLocked(PROP_USER_ID);
      }


      public bool createLiveEvent(string title, string description, UserVideo.Permission permission, UserLiveEvent.Source source,
        UserVideo.VideoStereoscopyType videoStereoscopyType, User.Result.CreateLiveEvent.If callback,
                                     SynchronizationContext handler, object closure) {
         APIClientImpl apiClient = getContainer() as APIClientImpl;
         AsyncWorkQueue workQueue = apiClient.getAsyncUploadQueue();

         WorkItemCreateLiveEvent workItem = (WorkItemCreateLiveEvent)workQueue.obtainWorkItem(WorkItemCreateLiveEvent.TYPE);
         workItem.set(this, title, description, permission, source, videoStereoscopyType, callback, handler, closure);
         return workQueue.enqueue(workItem);
      }

      public bool queryLiveEvents(User.Result.QueryLiveEvents.If callback, SynchronizationContext handler, object closure) {
         APIClientImpl apiClient = getContainer() as APIClientImpl;
         AsyncWorkQueue workQueue = apiClient.getAsyncUploadQueue();

         WorkItemQueryLiveEvents workItem = (WorkItemQueryLiveEvents)workQueue.obtainWorkItem(WorkItemQueryLiveEvents.TYPE);
         workItem.set(this, callback, handler, closure);
         return workQueue.enqueue(workItem);
      }

      public bool queryLiveEvent(string liveEventId, UserLiveEvent.Result.Query.If callback, SynchronizationContext handler, object closure) {
         APIClientImpl apiClient = getContainer() as APIClientImpl;
         AsyncWorkQueue workQueue = apiClient.getAsyncUploadQueue();

         UserLiveEventImpl.WorkItemQuery workItem = (UserLiveEventImpl.WorkItemQuery)workQueue.obtainWorkItem(UserLiveEventImpl.WorkItemQuery.TYPE);
         workItem.set(this, liveEventId, null, callback, handler, closure);
         return workQueue.enqueue(workItem);
      }

      public bool uploadVideo(System.IO.Stream source, long length, string title, string description,
         UserVideo.Permission permission, User.Result.UploadVideo.If callback, System.Threading.SynchronizationContext handler, object closure) {

         APIClientImpl apiClient = getContainer() as APIClientImpl;
         AsyncWorkQueue workQueue = apiClient.getAsyncUploadQueue();

         WorkItemNewVideoUpload workItem = (WorkItemNewVideoUpload)workQueue.obtainWorkItem(WorkItemNewVideoUpload.TYPE);
         workItem.set(this, source, length, title, description, permission, callback, handler, closure);
         return workQueue.enqueue(workItem);
      }

      private class UploadVideoCanceller : AsyncWorkQueue.IterationObserver {

         private readonly object mClosure;
         private bool mFound;

         public UploadVideoCanceller(object closure) {
            mClosure = closure;
            mFound = false;
         }

         public bool onIterate(AsyncWorkItem workItem, object args) {
            AsyncWorkItemType type = workItem.getType();
            if (UserImpl.WorkItemNewVideoUpload.TYPE == type ||
                    UserVideoImpl.WorkItemVideoContentUpload.TYPE == type) {
               ClientWorkItem cWorkItem = workItem as ClientWorkItem;
               object uploadClosure = cWorkItem.getClosure();
               Log.d(TAG, "Found video upload related work item " + workItem +
                       " closure: " + uploadClosure);
               if (Util.checkEquals(mClosure, uploadClosure)) {
                  workItem.cancel();
                  mFound = true;
                  Log.d(TAG, "Cancelled video upload related work item " + workItem);
               }
            }
            return true;
         }

         public bool wasFound() {
            return mFound;
         }

      }

      public bool cancelUploadVideo(object closure) {
         Log.d(TAG, "Cancelled video upload requested with closure: " + closure);
         APIClientImpl apiClient = getContainer() as APIClientImpl;
         AsyncWorkQueue workQueue = apiClient.getAsyncUploadQueue();

         UploadVideoCanceller canceller = new UploadVideoCanceller(closure);
         workQueue.iterateWorkItems(canceller, null);
         bool ret = canceller.wasFound();
         Log.d(TAG, "Cancelled video upload result: " + ret + " closure: " + closure);
         return ret;
      }

      public override List<U> containerOnQueryListOfContainedFromServiceLocked<U>(Contained.CType type, JObject jsonObject) {
         if (type == UserLiveEventImpl.sType) {
            JToken temp;
            if (!jsonObject.TryGetValue("videos", out temp)) {
               return null;
            }
            JArray jsonItems = temp as JArray;
            if (null == temp) {
               return null;
            }
            return mContainerImpl.processQueryListOfContainedFromServiceLocked<U>(type, jsonItems, null);
         }

         return null;
      }

      public override U containerOnQueryOfContainedFromServiceLocked<U>(Contained.CType type, U contained, JObject jsonObject) {
         if (UserLiveEventImpl.sType == type) {
            return mContainerImpl.processQueryOfContainedFromServiceLocked<U>(type, contained, jsonObject, false);
         }
         return default(U);
      }

      public override U containerOnDeleteOfContainedFromServiceLocked<U>(Contained.CType type, U contained) {
         if (UserLiveEventImpl.sType == type) {
            return mContainerImpl.processDeleteOfContainedFromServiceLocked<U>(type, contained);
         }
         return default(U);
      }

      public override U containerOnCreateOfContainedInServiceLocked<U>(Contained.CType type, JObject jsonObject) {
         if (UserLiveEventImpl.sType == type) {
            return mContainerImpl.processCreateOfContainedInServiceLocked<U>(type, jsonObject, false);
         }
         return default(U);
      }

      public override U containerOnUpdateOfContainedToServiceLocked<U>(Contained.CType type, U contained) {
         if (UserLiveEventImpl.sType == type) {
            return mContainerImpl.processUpdateOfContainedToServiceLocked<U>(type, contained);
         }
         return default(U);
      }

      internal abstract class WorkItemVideoUploadBase : ClientWorkItem {

         private ObjectHolder<bool> mCancelHolder;

         public WorkItemVideoUploadBase(APIClientImpl apiClient, AsyncWorkItemType type)
            : base(apiClient, type) {
         }

         protected void set(ObjectHolder<bool> cancelHolder, User.Result.UploadVideo.If callback, SynchronizationContext handler,
                  object closure) {
            base.set(callback, handler, closure);
            mCancelHolder = cancelHolder;
         }

         protected override void recycle() {
            base.recycle();
            mCancelHolder = null;
         }

         public ObjectHolder<bool> getCancelHolder() {
            return mCancelHolder;
         }

         public override void cancel() {
            lock (this) {
               base.cancel();
               if (null != mCancelHolder) {
                  mCancelHolder.set(true);
               }
            }
         }

         public override bool isCancelled() {
            lock (this) {
               bool cancelA = (null != mCancelHolder && mCancelHolder.get());
               bool cancelB = base.isCancelled();
               return cancelA || cancelB;
            }
         }
      }

      internal class WorkItemNewVideoUpload : WorkItemVideoUploadBase {

         private class VideoIdAvailableCallbackNotifier : Util.CallbackNotifier {

            private readonly UserVideo.If mUserVideo;

            public VideoIdAvailableCallbackNotifier(ResultCallbackHolder callbackHolder, UserVideo.If userVideo)
               : base(callbackHolder) {
               mUserVideo = userVideo;
            }

            protected override void notify(object callback, object closure) {
               User.Result.UploadVideo.If tCallback = callback as User.Result.UploadVideo.If;
               tCallback.onVideoIdAvailable(closure, mUserVideo);
            }
         }

         private class WorkItemTypeNewVideoUpload : AsyncWorkItemType {

            public AsyncWorkItem newInstance(APIClientImpl apiClient) {
               return new WorkItemNewVideoUpload(apiClient);
            }
         }

         public static readonly AsyncWorkItemType TYPE = new WorkItemTypeNewVideoUpload();

         WorkItemNewVideoUpload(APIClientImpl apiClient)
            : base(apiClient, TYPE) {
         }

         private Stream mSource;
         private long mLength;
         private String mTitle, mDescription;
         private UserImpl mUser;
         private UserVideo.Permission mPermission;

         public WorkItemNewVideoUpload set(UserImpl user,
                 Stream source, long length, string title, string description,
                 UserVideo.Permission permission, User.Result.UploadVideo.If callback, SynchronizationContext handler,
                 object closure) {

            set(new ObjectHolder<bool>(false), callback, handler, closure);
            mUser = user;
            mTitle = title;
            mDescription = description;
            mSource = source;
            mLength = length;
            mPermission = permission;
            return this;
         }


         protected override void recycle() {
            base.recycle();
            mSource = null;
            mTitle = null;
            mDescription = null;
         }

         private static readonly string TAG = Util.getLogTag(typeof(WorkItemNewVideoUpload));

         protected override void onRun() {

            long length = mLength;

            string[,] headers0 = new string[,] {
                    {UserImpl.HEADER_SESSION_TOKEN, mUser.getSessionToken()},
                    {APIClientImpl.HEADER_API_KEY, mAPIClient.getApiKey()},
            };

            string[,] headers1 = {
                    {HEADER_CONTENT_LENGTH, "0"},
                    {HEADER_CONTENT_TYPE, "application/json" + ClientWorkItem.CONTENT_TYPE_CHARSET_SUFFIX_UTF8},
                    {headers0[0, 0], headers0[0, 1]},
                    {headers0[1, 0], headers0[1, 1]}
            };

            JObject jsonParam = new JObject();

            jsonParam.Add("title", mTitle);
            jsonParam.Add("description", mDescription);
            jsonParam.Add("length", length);
            jsonParam.Add("permission", UserVideo.toString(mPermission));

            HttpPlugin.PostRequest request = null;
            string videoId = null;
            string uploadId = null;
            string signedUrl = null;
            int chunkSize = 0;
            int numChunks = 0;

            try {
               string jsonStr = jsonParam.ToString(Newtonsoft.Json.Formatting.None);
               byte[] data = System.Text.Encoding.UTF8.GetBytes(jsonStr);

               headers1[0, 1] = data.Length.ToString();
               request = newPostRequest(string.Format("user/{0}/video", mUser.getUserId()), headers1);
               if (null == request) {
                  dispatchFailure(VR.Result.STATUS_HTTP_PLUGIN_NULL_CONNECTION);
                  return;
               }

               writeBytes(request, data, jsonStr);

               if (isCancelled()) {
                  dispatchCancelled();
                  return;
               }

               HttpStatusCode rsp = getResponseCode(request);
               string data4 = readHttpStream(request, "code: " + rsp);
               if (null == data4) {
                  dispatchFailure(VR.Result.STATUS_HTTP_PLUGIN_STREAM_READ_FAILURE);
                  return;
               }
               JObject jsonObject = JObject.Parse(data4);

               if (!isHTTPSuccess(rsp)) {
                  int status = Util.jsonOpt(jsonObject, "status", VR.Result.STATUS_SERVER_RESPONSE_NO_STATUS_CODE);
                  dispatchFailure(status);
                  return;
               }

               if (isCancelled()) {
                  dispatchCancelled();
                  return;
               }

               videoId = Util.jsonGet<string>(jsonObject, "video_id");
               uploadId = Util.jsonGet<string>(jsonObject, "upload_id");
               signedUrl = Util.jsonGet<string>(jsonObject, "signed_url");
               chunkSize = Util.jsonGet<int>(jsonObject, "chunk_size");
               numChunks = Util.jsonGet<int>(jsonObject, "chunks");

               UserVideoImpl userVideo = new UserVideoImpl(mUser, mTitle, mDescription, mPermission,
                  videoId, uploadId, signedUrl, chunkSize, numChunks);
               VideoIdAvailableCallbackNotifier notifier = new VideoIdAvailableCallbackNotifier(mCallbackHolder, userVideo);

               if (!userVideo.uploadContent(getCancelHolder(), mSource, mSource.Length, mCallbackHolder)) {
                  dispatchUncounted(notifier);
                  dispatchFailure(User.Result.UploadVideo.STATUS_CONTENT_UPLOAD_SCHEDULING_FAILED);
               } else {
                  dispatchCounted(notifier);
               }

            } finally {
               destroy(request);
            }

         }
      }

      /*
       * Create
       */

      internal class WorkItemCreateLiveEvent : ClientWorkItem {

         private class WorkItemTypeCreateLiveEvent : AsyncWorkItemType {

            public AsyncWorkItem newInstance(APIClientImpl apiClient) {
               return new WorkItemCreateLiveEvent(apiClient);
            }
         }

         public static readonly AsyncWorkItemType TYPE = new WorkItemTypeCreateLiveEvent();

         WorkItemCreateLiveEvent(APIClientImpl apiClient)
            : base(apiClient, TYPE) {
         }

         private string mTitle, mDescription;
         UserLiveEvent.Source mSource;
         private UserVideo.Permission mPermission;

         UserVideo.VideoStereoscopyType mVideoStereoscopyType;
         private UserImpl mUser;

         public WorkItemCreateLiveEvent set(UserImpl user, string title, string description,
             UserVideo.Permission permission, UserLiveEvent.Source source,
             UserVideo.VideoStereoscopyType videoStereoscopyType, User.Result.CreateLiveEvent.If callback,
             SynchronizationContext handler, object closure) {

            base.set(callback, handler, closure);
            mUser = user;
            mTitle = title;
            mPermission = permission;
            mDescription = description;
            mSource = source;
            mVideoStereoscopyType = videoStereoscopyType;
            return this;
         }

         protected override void recycle() {
            base.recycle();
            mDescription = null;
            mTitle = null;
            mUser = null;
         }

         private static readonly string TAG = Util.getLogTag(typeof(WorkItemCreateLiveEvent));

         protected override void onRun() {
            HttpPlugin.PostRequest request = null;

            try {

               JObject jsonParam = new JObject();

               String userId = mUser.getUserId();

               jsonParam.Add("title", mTitle);
               jsonParam.Add("description", mDescription);
               jsonParam.Add("permission", Util.enum2String(mPermission));
               switch (mVideoStereoscopyType) {
                  case UserVideo.VideoStereoscopyType.TOP_BOTTOM_STEREOSCOPIC:
                     jsonParam.Add("stereoscopic_type", "top-bottom");
                     break;
                  case UserVideo.VideoStereoscopyType.LEFT_RIGHT_STEREOSCOPIC:
                     jsonParam.Add("stereoscopic_type", "left-right");
                     break;
                  case UserVideo.VideoStereoscopyType.DUAL_FISHEYE:
                     jsonParam.Add("stereoscopic_type", "dual-fisheye");
                     break;

               }
               jsonParam.Add("source", Util.enum2String(mSource));

               string jsonStr = jsonParam.ToString(Newtonsoft.Json.Formatting.None);
               byte[] data = System.Text.Encoding.UTF8.GetBytes(jsonStr);

               string[,] headers = {
                        {HEADER_CONTENT_LENGTH, data.Length.ToString()},
                        {UserImpl.HEADER_SESSION_TOKEN, mUser.getSessionToken()},
                        {APIClientImpl.HEADER_API_KEY, mAPIClient.getApiKey()},
                        {HEADER_CONTENT_TYPE, "application/json" + ClientWorkItem.CONTENT_TYPE_CHARSET_SUFFIX_UTF8}
                };
               string url = string.Format("user/{0}/video", userId);
               request = newPostRequest(url, headers);
               if (null == request) {
                  dispatchFailure(VR.Result.STATUS_HTTP_PLUGIN_NULL_CONNECTION);
                  return;
               }

               writeBytes(request, data, jsonStr);

               if (isCancelled()) {
                  dispatchCancelled();
                  return;
               }

               HttpStatusCode rsp = getResponseCode(request);
               string data2 = readHttpStream(request, "code: " + rsp);
               if (null == data2) {
                  dispatchFailure(VR.Result.STATUS_HTTP_PLUGIN_STREAM_READ_FAILURE);
                  return;
               }
               JObject jsonObject = JObject.Parse(data2);

               if (isHTTPSuccess(rsp)) {
                  JObject liveEvent = jsonObject;

                  string videoId = Util.jsonOpt<string>(jsonObject, "video_id", null);
                  string ingestUrl = Util.jsonOpt<string>(jsonObject, "ingest_url", null);
                  string viewUrl = Util.jsonOpt<string>(jsonObject, "view_url", null);

                  if (null != videoId) {
                     UserLiveEventImpl eventObj = new UserLiveEventImpl(mUser, videoId, mTitle, mPermission,
                        mSource, mDescription, mVideoStereoscopyType, ingestUrl, viewUrl);
                     dispatchSuccessWithResult<UserLiveEvent.If>(eventObj);
                     return;
                  }
               }

               int status = Util.jsonOpt(jsonObject, "status", VR.Result.STATUS_SERVER_RESPONSE_NO_STATUS_CODE);
               dispatchFailure(status);

            } finally {
               destroy(request);
            }

         }
      }

      internal class WorkItemQueryLiveEvents : ClientWorkItem {

         private class WorkItemTypeQueryLiveEvents : AsyncWorkItemType {

            public AsyncWorkItem newInstance(APIClientImpl apiClient) {
               return new WorkItemQueryLiveEvents(apiClient);
            }
         }

         public static readonly AsyncWorkItemType TYPE = new WorkItemTypeQueryLiveEvents();

         WorkItemQueryLiveEvents(APIClientImpl apiClient)
            : base(apiClient, TYPE) {
         }

         private UserImpl mUser;

         public WorkItemQueryLiveEvents set(UserImpl user, User.Result.QueryLiveEvents.If callback,
             SynchronizationContext handler, object closure) {
            base.set(callback, handler, closure);
            mUser = user;
            return this;
         }

         protected override void recycle() {
            base.recycle();
            mUser = null;
         }

         private static readonly string TAG = Util.getLogTag(typeof(WorkItemQueryLiveEvents));

         protected override void onRun() {
            HttpPlugin.GetRequest request = null;

            string[,] headers = {
                    {UserImpl.HEADER_SESSION_TOKEN, mUser.getSessionToken()},
                    {APIClientImpl.HEADER_API_KEY, mAPIClient.getApiKey()}
            };

            try {

               string userId = mUser.getUserId();

               request = newGetRequest(string.Format("user/{0}/video?source=live", userId), headers);

               if (null == request) {
                  dispatchFailure(VR.Result.STATUS_HTTP_PLUGIN_NULL_CONNECTION);
                  return;

               }

               if (isCancelled()) {
                  return;
               }

               HttpStatusCode rsp = getResponseCode(request);
               String data = readHttpStream(request, "code: " + rsp);
               if (null == data) {
                  dispatchFailure(VR.Result.STATUS_HTTP_PLUGIN_STREAM_READ_FAILURE);
                  return;
               }
               JObject jsonObject = JObject.Parse(data);
               if (isHTTPSuccess(rsp)) {
                  List<UserLiveEventImpl> resultTmp = mUser.containerOnQueryListOfContainedFromServiceLocked<UserLiveEventImpl>(UserLiveEventImpl.sType, jsonObject);
                  if (null != resultTmp) {
                     List<UserLiveEvent.If> result = new List<UserLiveEvent.If>();
                     foreach (object temp in resultTmp) {
                        result.Add(temp as UserLiveEvent.If);
                     }
                     dispatchSuccessWithResult<List<UserLiveEvent.If>>(result);
                  } else {
                     dispatchFailure(VR.Result.STATUS_SERVER_RESPONSE_INVALID);
                  }
                  return;
               }

               int status = Util.jsonOpt(jsonObject, "status", VR.Result.STATUS_SERVER_RESPONSE_NO_STATUS_CODE);
               dispatchFailure(status);
            } finally {
               destroy(request);
            }

         }
      }


   }
}
