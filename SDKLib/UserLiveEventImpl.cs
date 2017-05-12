using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json.Linq;
using System.Threading;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using static SDKLib.UserImpl;

namespace SDKLib {


   internal class UserLiveEventImpl : Contained.BaseImpl<UserImpl>, UserLiveEvent.If {

      private static readonly string TAG = Util.getLogTag(typeof(UserImpl));
      private static bool DEBUG = Util.DEBUG;

      public static readonly string HEADER_SESSION_TOKEN = "X-SESSION-TOKEN";
      private static readonly string PROP_ID = "id", PROP_TITLE = "title", PROP_PERMISSION = "permission",
         PROP_SOURCE = "source", PROP_STEREOSCOPIC_TYPE = "stereoscopic_type", PROP_DESCRIPTION = "description",
         PROP_INGEST_URL = "ingest_url", PROP_VIEW_URL = "view_url", PROP_STATE = "state", PROP_THUMBNAIL_URL = "thumbnail_url",
         PROP_VIEWER_COUNT = "viewer_count", PROP_LIVE_STARTED = "live_started", PROP_LIVE_STOPPED = "live_stopped",
         PROP_METADATA_STEREOSCOPIC_TYPE = "metadata_stereoscopic_type";

      public class CType : Contained.CType {

         public CType()
            : base(new string[] { PROP_METADATA_STEREOSCOPIC_TYPE,
               PROP_ID, PROP_TITLE, PROP_DESCRIPTION, PROP_INGEST_URL, PROP_VIEW_URL, PROP_THUMBNAIL_URL,
               PROP_VIEWER_COUNT, PROP_LIVE_STARTED, PROP_LIVE_STOPPED,
               PROP_STEREOSCOPIC_TYPE,
               PROP_PERMISSION, PROP_SOURCE, PROP_STATE}) {
         }

         public override SDKLib.Contained.If newInstance(Container.If container, JObject jsonObject) {
            return new UserLiveEventImpl((UserImpl)container, jsonObject);
         }

         public override object getContainedId(JObject jsonObject) {
            return jsonObject.GetValue(PROP_ID) + "";
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

         public override bool validateValue(out string outKey, out object outValue, string inKey, JObject jsonObject) {
            if (PROP_METADATA_STEREOSCOPIC_TYPE.Equals(inKey)) {
               JToken metadata = null;
               outValue = null;
               outKey = inKey;

               if (!jsonObject.TryGetValue("metadata", out metadata)) {
                  return false;
               }
               string st_type = (string)metadata["stereoscopic_type"];
               if (null == st_type) {
                  return false;
               }
               UserVideo.VideoStereoscopyType temp;

               if (Util.string2Enum<UserVideo.VideoStereoscopyType>(out temp, st_type)) {
                  outValue = temp;
                  return true;
               }
               return false;
            }
            return base.validateValue(out outKey, out outValue, inKey, jsonObject);
         }

         public override object validateValue(string key, JToken token, object rawValue) {
            if (PROP_ID.Equals(key) || PROP_TITLE.Equals(key) || PROP_DESCRIPTION.Equals(key) ||
               PROP_INGEST_URL.Equals(key) || PROP_VIEW_URL.Equals(key) || PROP_THUMBNAIL_URL.Equals(key)) {
               return rawValue;
            }
            if (PROP_VIEWER_COUNT.Equals(key) || PROP_LIVE_STARTED.Equals(key) || PROP_LIVE_STOPPED.Equals(key)) {
               return long.Parse(rawValue.ToString());
            }
            if (PROP_STEREOSCOPIC_TYPE.Equals(key)) {
               UserVideo.VideoStereoscopyType temp;
               if (Util.string2Enum<UserVideo.VideoStereoscopyType>(out temp, rawValue as string)) {
                  return temp;
               }
               return null;
            }
            if (PROP_PERMISSION.Equals(key)) {
               UserVideo.Permission temp;
               if (Util.string2Enum<UserVideo.Permission>(out temp, rawValue as string)) {
                  return temp;
               }
               return null;
            }
            if (PROP_SOURCE.Equals(key)) {
               UserLiveEvent.Source temp;
               if (Util.string2Enum<UserLiveEvent.Source>(out temp, rawValue as string)) {
                  return temp;
               }
               return null;
            }
            if (PROP_STATE.Equals(key)) {
               UserLiveEvent.State temp;
               if (Util.string2Enum<UserLiveEvent.State>(out temp, rawValue as string)) {
                  return temp;
               }
               return null;
            }
            return null;
         }
      }

      public static readonly CType sType = new CType();

      private UserLiveEventImpl(UserImpl user, JObject jsonObject)
         : base(new CType(), user, jsonObject) {
      }

      public UserLiveEventImpl(UserImpl container, string id, string title,
         UserVideo.Permission permission, UserLiveEvent.Source source, string description, string ingestUrl, string viewUrl,
         UserVideo.VideoStereoscopyType videoStereoscopyType, UserLiveEvent.State state,
         long viewerCount, long startedTime, long finishedTime)
         : this(container, null) {

         setNoLock(PROP_ID, id);
         setNoLock(PROP_TITLE, title);
         setNoLock(PROP_SOURCE, source);
         setNoLock(PROP_DESCRIPTION, description);
         setNoLock(PROP_INGEST_URL, ingestUrl);
         setNoLock(PROP_VIEW_URL, viewUrl);
         setNoLock(PROP_PERMISSION, permission);
         setNoLock(PROP_STATE, state);
         setNoLock(PROP_VIEWER_COUNT, viewerCount);
         setNoLock(PROP_LIVE_STARTED, startedTime);
         setNoLock(PROP_LIVE_STOPPED, finishedTime);
         setNoLock(PROP_STEREOSCOPIC_TYPE, videoStereoscopyType);
      }

      public UserLiveEventImpl(UserImpl container, string id, string title, UserVideo.Permission permission,
         UserLiveEvent.Source source, string description, UserVideo.VideoStereoscopyType videoStereoscopyType,
         string ingestUrl, string viewUrl)
         : this(container, id, title, permission, source,
            description, ingestUrl, viewUrl,
            videoStereoscopyType, UserLiveEvent.State.UNKNOWN, 0L, 0L, 0L) {
      }

      public override object containedGetIdLocked() {
         return getLocked(PROP_ID);
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

      public string getId() {
         return getLocked(PROP_ID) as string;
      }

      public string getTitle() {
         return getLocked(PROP_TITLE) as string;
      }


      public string getDescription() {
         return getLocked(PROP_DESCRIPTION) as string;
      }

      public string getProducerUrl() {
         return (string)getLocked(PROP_INGEST_URL);
      }

      public string getViewUrl() {
         return (string)getLocked(PROP_VIEW_URL);
      }

      public UserVideo.VideoStereoscopyType getVideoStereoscopyType() {
         object result = getLocked(PROP_STEREOSCOPIC_TYPE);
         if (null != result) {
            return (UserVideo.VideoStereoscopyType)result;
         }
         result = getLocked(PROP_METADATA_STEREOSCOPIC_TYPE);
         if (null != result) {
            return (UserVideo.VideoStereoscopyType)result;
         }
         return UserVideo.VideoStereoscopyType.MONOSCOPIC;
      }

      public UserLiveEvent.State getState() {
         return (UserLiveEvent.State)getLocked(PROP_STATE);
      }


      public long getViewerCount() {
         return (long)getLocked(PROP_VIEWER_COUNT);
      }


      public UserLiveEvent.Source getSource() {
         return (UserLiveEvent.Source)getLocked(PROP_SOURCE);
      }

      public UserVideo.Permission getPermission() {
         return (UserVideo.Permission)getLocked(PROP_PERMISSION);
      }

      public long getStartedTime() {
         return (long)getLocked(PROP_LIVE_STARTED);
      }

      public long getFinishedTime() {
         return (long)getLocked(PROP_LIVE_STOPPED);
      }

      public User.If getUser() {
         return getContainer() as User.If;
      }


      public string getThumbnailUrl() {
         return (string)getLocked(PROP_THUMBNAIL_URL);
      }


      public bool query(UserLiveEvent.Result.Query.If callback, SynchronizationContext handler, object closure) {
         UserImpl user = getContainer() as UserImpl;
         APIClientImpl apiClient = user.getContainer() as APIClientImpl;

         AsyncWorkQueue workQueue = apiClient.getAsyncWorkQueue();

         WorkItemQuery workItem = (WorkItemQuery)workQueue.obtainWorkItem(WorkItemQuery.TYPE);
         workItem.set(user, getId(), this, callback, handler, closure);
         return workQueue.enqueue(workItem);
      }

      public bool finish(UserLiveEvent.Result.Finish.If callback, SynchronizationContext handler, object closure) {
         UserImpl user = getContainer() as UserImpl;
         APIClientImpl apiClient = user.getContainer() as APIClientImpl;

         AsyncWorkQueue workQueue = apiClient.getAsyncWorkQueue();


         WorkItemFinish workItem = (WorkItemFinish)workQueue.obtainWorkItem(WorkItemFinish.TYPE);
         workItem.set(this, callback, handler, closure);
         return workQueue.enqueue(workItem);
      }


      public bool delete(UserLiveEvent.Result.Delete.If callback, SynchronizationContext handler, object closure) {
         UserImpl user = getContainer() as UserImpl;
         APIClientImpl apiClient = user.getContainer() as APIClientImpl;
         AsyncWorkQueue workQueue = apiClient.getAsyncWorkQueue();
         WorkItemDelete workItem = (WorkItemDelete)workQueue.obtainWorkItem(WorkItemDelete.TYPE);
         workItem.set(this, callback, handler, closure);
         return workQueue.enqueue(workItem);
      }

      private class UploadSegmentCanceller : AsyncWorkQueue.IterationObserver {

         private readonly object mClosure;
         private bool mFound;

         public UploadSegmentCanceller(object closure) {
            mClosure = closure;
            mFound = false;
         }

         public bool onIterate(AsyncWorkItem workItem, object args) {
            AsyncWorkItemType type = workItem.getType();


            if (WorkItemNewSegmentUpload.TYPE == type || UserLiveEventSegmentImpl.WorkItemSegmentContentUpload.TYPE == type) {
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


      private int mSegmentId = -1;

      public bool uploadSegmentFromFD(Stream source, long length, UserLiveEvent.Result.UploadSegment.If callback, SynchronizationContext handler, object closure) {
         UserImpl user = getContainer() as UserImpl;
         APIClientImpl apiClient = user.getContainer() as APIClientImpl;
         AsyncWorkQueue workQueue = apiClient.getAsyncUploadQueue();
         WorkItemNewSegmentUpload workItem = (WorkItemNewSegmentUpload)workQueue.obtainWorkItem(WorkItemNewSegmentUpload.TYPE);

         workItem.set(this, (++mSegmentId).ToString(), source, length, callback, handler, closure);
         return workQueue.enqueue(workItem);
      }

      public bool cancelUploadSegment(object closure) {
         if (DEBUG) {
            Log.d(TAG, "Cancelled video upload requested with closure: " + closure);
         }
         UserImpl user = getContainer() as UserImpl;
         APIClientImpl apiClient = user.getContainer() as APIClientImpl;
         AsyncWorkQueue workQueue = apiClient.getAsyncUploadQueue();
         WorkItemDelete workItem = (WorkItemDelete)workQueue.obtainWorkItem(WorkItemDelete.TYPE);

         UploadSegmentCanceller canceller = new UploadSegmentCanceller(closure);
         workQueue.iterateWorkItems(canceller, null);
         bool ret = canceller.wasFound();
         Log.d(TAG, "Cancelled live segment result: " + ret + " closure: " + closure);
         return ret;
      }

      internal class WorkItemQuery : WorkItemForUser {

         private class WorkItemTypeQuery : AsyncWorkItemType {

            public AsyncWorkItem newInstance(APIClientImpl apiClient) {
               return new WorkItemQuery(apiClient);
            }
         }

         public static readonly AsyncWorkItemType TYPE = new WorkItemTypeQuery();


         public WorkItemQuery(APIClientImpl apiClient)
            : base(apiClient, TYPE) {
         }

         private string mUserLiveEventId;
         private UserLiveEventImpl mUserLiveEventImpl;


         public WorkItemQuery set(UserImpl user, string userLiveEventId, UserLiveEventImpl userLiveEventImpl,
                UserLiveEvent.Result.Query.If callback, SynchronizationContext handler, object closure) {
            base.set(user, callback, handler, closure);
            mUserLiveEventImpl = userLiveEventImpl;
            mUserLiveEventId = userLiveEventId;
            return this;
         }

         protected override void recycle() {
            base.recycle();
            mUserLiveEventId = null;
            mUserLiveEventImpl = null;
         }

         private static readonly string TAG = Util.getLogTag(typeof(WorkItemQuery));


         protected override void onRun() {
            HttpPlugin.GetRequest request = null;
            UserImpl user = mUser;
            string[,] headers = {
                    {UserImpl.HEADER_SESSION_TOKEN, user.getSessionToken()},
                    {APIClientImpl.HEADER_API_KEY, mAPIClient.getApiKey()}
            };
            try {
               String liveEventId = mUserLiveEventId;
               if (liveEventId == null) {
                  Log.d(TAG, "onRun : " + " liveEventId is null! this wont work!");
                  return;

               }
               String userId = user.getUserId();
               request = newGetRequest(string.Format("user/{0}/video/{1}", userId, liveEventId), headers);
               if (null == request) {
                  dispatchFailure(VR.Result.STATUS_HTTP_PLUGIN_NULL_CONNECTION);
                  return;
               }

               if (isCancelled()) {
                  dispatchCancelled();
                  return;
               }

               HttpStatusCode rsp = getResponseCode(request);
               String data = readHttpStream(request, "code: " + rsp);

               if (null == data) {
                  dispatchFailure(VR.Result.STATUS_HTTP_PLUGIN_STREAM_READ_FAILURE);
                  return;
               }

               Log.d(TAG, "onSuccess : " + data);
               JObject jsonObject = JObject.Parse(data);

               if (isHTTPSuccess(rsp)) {
                  JObject liveEvent = Util.jsonOpt<JObject>(jsonObject, "video", null);
                  UserLiveEventImpl result = mUser.containerOnQueryOfContainedFromServiceLocked<UserLiveEventImpl>(
                          UserLiveEventImpl.sType, mUserLiveEventImpl, liveEvent);
                  if (null != result) {
                     dispatchSuccessWithResult<UserLiveEvent.If>(result);
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

      /*
       * Finish
       */

      internal class WorkItemFinish : WorkItemForUser {


         private class WorkItemTypeQuery : AsyncWorkItemType {

            public AsyncWorkItem newInstance(APIClientImpl apiClient) {
               return new WorkItemFinish(apiClient);
            }
         }

         public static readonly AsyncWorkItemType TYPE = new WorkItemTypeQuery();

         WorkItemFinish(APIClientImpl apiClient) : base(apiClient, TYPE) {
         }

         private UserLiveEventImpl mUserLiveEvent;

         public WorkItemFinish set(UserLiveEventImpl userLiveEvent, UserLiveEvent.Result.Finish.If callback,
            SynchronizationContext handler, object closure) {

            base.set((UserImpl)userLiveEvent.getUser(), callback, handler, closure);
            mUserLiveEvent = userLiveEvent;
            return this;
         }

         protected override void recycle() {
            base.recycle();
            mUserLiveEvent = null;
         }

         private static readonly string TAG = Util.getLogTag(typeof(WorkItemFinish));

         protected override void onRun() {
            HttpPlugin.PutRequest request = null;
            User.If user = mUserLiveEvent.getUser();

            JObject jsonParam = new JObject();
            jsonParam.Add("state", UserLiveEvent.State.LIVE_FINISHED_ARCHIVED.ToString());

            string jsonStr = jsonParam.ToString(Newtonsoft.Json.Formatting.None);
            byte[] bdata = System.Text.Encoding.UTF8.GetBytes(jsonStr);

            string[,] headers = {
                    {UserImpl.HEADER_SESSION_TOKEN, user.getSessionToken()},
                    {APIClientImpl.HEADER_API_KEY, mAPIClient.getApiKey()},
                    {HEADER_CONTENT_TYPE, "application/json"},
                    {HEADER_CONTENT_LENGTH, bdata.Length.ToString()},
            };
            try {
               String liveEventId = mUserLiveEvent.getId();
               String userId = user.getUserId();
               request = newPutRequest(string.Format("user/{0}/video/{1}", userId, liveEventId), headers);

               if (null == request) {
                  dispatchFailure(VR.Result.STATUS_HTTP_PLUGIN_NULL_CONNECTION);
                  return;
               }

               writeBytes(request, bdata, jsonStr);
               if (isCancelled()) {
                  dispatchCancelled();
                  return;
               }

               HttpStatusCode rsp = getResponseCode(request);
               if (isHTTPSuccess(rsp)) {
                  dispatchSuccess();
                  return;
               }
               string data = readHttpStream(request, "code: " + rsp);
               if (null == data) {
                  dispatchFailure(VR.Result.STATUS_HTTP_PLUGIN_STREAM_READ_FAILURE);
                  return;
               }
               JObject jsonObject = JObject.Parse(data);
               int status = Util.jsonOpt(jsonObject, "status", VR.Result.STATUS_SERVER_RESPONSE_NO_STATUS_CODE);
               dispatchFailure(status);

            } finally {
               destroy(request);
            }
         }
      }

      /*
       * Delete
       */

      internal class WorkItemDelete : WorkItemForUser {

         private class WorkItemTypeDelete : AsyncWorkItemType {

            public AsyncWorkItem newInstance(APIClientImpl apiClient) {
               return new WorkItemDelete(apiClient);
            }
         }

         public static readonly AsyncWorkItemType TYPE = new WorkItemTypeDelete();

         WorkItemDelete(APIClientImpl apiClient) : base(apiClient, TYPE) {
         }

         private UserLiveEventImpl mUserLiveEvent;

         public WorkItemDelete set(UserLiveEventImpl userLiveEvent, UserLiveEvent.Result.Delete.If callback,
             SynchronizationContext handler, object closure) {
            base.set((UserImpl)userLiveEvent.getUser(), callback, handler, closure);
            mUserLiveEvent = userLiveEvent;
            return this;
         }

         protected override void recycle() {
            base.recycle();
            mUserLiveEvent = null;
         }

         private static readonly String TAG = Util.getLogTag(typeof(WorkItemDelete));

         protected override void onRun() {
            User.If user = mUserLiveEvent.getUser();
            HttpPlugin.DeleteRequest request = null;
            string[,] headers = {
                    {UserImpl.HEADER_SESSION_TOKEN, user.getSessionToken()},
                    {APIClientImpl.HEADER_API_KEY, mAPIClient.getApiKey()}
            };
            try {
               String liveEventId = mUserLiveEvent.getId();

               String userId = user.getUserId();

               request = newDeleteRequest(string.Format("user/{0}/video/{1}", userId, liveEventId), headers);
               if (null == request) {
                  dispatchFailure(VR.Result.STATUS_HTTP_PLUGIN_NULL_CONNECTION);
                  return;
               }

               if (isCancelled()) {
                  dispatchCancelled();
                  return;
               }

               HttpStatusCode rsp = getResponseCode(request);

               if (isHTTPSuccess(rsp)) {
                  if (null != mUserLiveEvent.getContainer().containerOnDeleteOfContainedFromServiceLocked(
                          UserLiveEventImpl.sType, mUserLiveEvent)) {
                     dispatchSuccess();
                  } else {
                     dispatchFailure(VR.Result.STATUS_SERVER_RESPONSE_INVALID);
                  }
                  return;
               }
               String data = readHttpStream(request, "code: " + rsp);
               if (null == data) {
                  dispatchFailure(VR.Result.STATUS_HTTP_PLUGIN_STREAM_READ_FAILURE);
                  return;
               }
               JObject jsonObject = new JObject(data);
               int status = Util.jsonOpt(jsonObject, "status", VR.Result.STATUS_SERVER_RESPONSE_NO_STATUS_CODE);
               dispatchFailure(status);

            } finally {
               destroy(request);
            }

         }
      }


      /*
       * Upload live
       */

      abstract internal class WorkItemSegmentUploadBase : WorkItemForUser {

         private ObjectHolder<bool> mCancelHolder;

         internal WorkItemSegmentUploadBase(APIClientImpl apiClient, AsyncWorkItemType type)
            : base(apiClient, type) {
         }

         protected void set(ObjectHolder<bool> cancelHolder, UserImpl user, UserLiveEvent.Result.UploadSegment.If callback, SynchronizationContext handler,
                            object closure) {
            base.set(user, callback, handler, closure);
            mCancelHolder = cancelHolder;
         }

         protected override void recycle() {
            base.recycle();
            mCancelHolder = null;
         }

         protected ObjectHolder<bool> getCancelHolder() {
            return mCancelHolder;
         }

         override public void cancel() {
            lock (this) {
               base.cancel();
               if (null != mCancelHolder) {
                  mCancelHolder.set(true);
               }
            }
         }


         override public bool isCancelled() {
            lock (this) {
               bool cancelA = (null != mCancelHolder && mCancelHolder.get());
               bool cancelB = base.isCancelled();
               if (DEBUG) {
                  Log.d(TAG, "Check for isCancelled, this: " + this + " a: " + cancelA + " b: " + cancelB);
               }
               return cancelA || cancelB;
            }
         }
      }

      internal class WorkItemNewSegmentUpload : WorkItemSegmentUploadBase {

         public class SegmentIdAvailableCallbackNotifier : Util.CallbackNotifier {

            private readonly UserLiveEventSegment.If mSegment;

            public SegmentIdAvailableCallbackNotifier(ResultCallbackHolder callbackHolder, UserLiveEventSegment.If segment) :
               base(callbackHolder) {
               mSegment = segment;
            }

            override protected void notify(object callback, object closure) {
               ((UserLiveEvent.Result.UploadSegment.If)callback).onSegmentIdAvailable(closure, mSegment);
            }
         }

         private class WorkItemTypeNewSegmentUpload : AsyncWorkItemType {

            public AsyncWorkItem newInstance(APIClientImpl apiClient) {
               return new WorkItemNewSegmentUpload(apiClient);
            }
         }

         public static readonly AsyncWorkItemType TYPE = new WorkItemTypeNewSegmentUpload();

         WorkItemNewSegmentUpload(APIClientImpl apiClient)
            : base(apiClient, TYPE) {
         }

         private Stream mSource;
         private long mLength;
         private UserLiveEventImpl mUserLiveEvent;
         private string mSegmentId;

         public UserLiveEventImpl.WorkItemNewSegmentUpload set(UserLiveEventImpl userLiveEvent, String segmentId,
             Stream source, long length, UserLiveEvent.Result.UploadSegment.If callback, SynchronizationContext handler,
             object closure) {

            set(new ObjectHolder<bool>(false), (UserImpl)userLiveEvent.getUser(), callback, handler, closure);
            mSegmentId = segmentId;
            mUserLiveEvent = userLiveEvent;
            mSource = source;
            mLength = length;
            return this;
         }


         override protected void recycle() {
            base.recycle();
            mSource = null;
         }

         private static readonly String TAG = Util.getLogTag(typeof(UserImpl.WorkItemNewVideoUpload));

         override protected void onRun() {

            UserImpl user = mUserLiveEvent.getContainer() as UserImpl;

            string[,] headers0 = {
                    {UserImpl.HEADER_SESSION_TOKEN, user.getSessionToken()},
                    {APIClientImpl.HEADER_API_KEY, mAPIClient.getApiKey()},
            };

            string[,] headers1 = {
                    {HEADER_CONTENT_LENGTH, "0"},
                    {HEADER_CONTENT_TYPE, "application/json" + ClientWorkItem.CONTENT_TYPE_CHARSET_SUFFIX_UTF8},
                    {headers0[0, 0], headers0[0, 1]},
                    {headers0[1, 0], headers0[1, 1]}
            };

            JObject jsonParam = new JObject();

            jsonParam.Add("status", "init");

            string jsonStr = jsonParam.ToString(Newtonsoft.Json.Formatting.None);

            HttpPlugin.PutRequest setupRequest = null;
            String signedUrl = null;

            try {

               MD5 digest = user.getMD5Digest();
               if (null == digest) {
                  dispatchFailure(UserLiveEvent.Result.UploadSegment.STATUS_SEGMENT_NO_MD5_IMPL);
               }

               byte[] data = System.Text.Encoding.UTF8.GetBytes(jsonStr);
               string uploadUrl = string.Format("user/{0}/video/{1}/live_segment/{2}",
                       user.getUserId(), mUserLiveEvent.getId(), mSegmentId);
               headers1[0, 1] = data.Length.ToString();
               setupRequest = newPutRequest(uploadUrl, headers1);
               if (null == setupRequest) {
                  dispatchFailure(VR.Result.STATUS_HTTP_PLUGIN_NULL_CONNECTION);
                  return;
               }

               writeBytes(setupRequest, data, jsonStr);

               if (isCancelled()) {
                  dispatchCancelled();
                  return;
               }

               HttpStatusCode rsp = getResponseCode(setupRequest);
               string data4 = readHttpStream(setupRequest, "code: " + rsp);
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

               signedUrl = Util.jsonGet<string>(jsonObject, "signed_url");

               UserLiveEventSegmentImpl segment = new UserLiveEventSegmentImpl(mUserLiveEvent, mSegmentId);
               Util.CallbackNotifier notifier = new WorkItemNewSegmentUpload.SegmentIdAvailableCallbackNotifier(mCallbackHolder, segment);
               notifier.setNoLock(mCallbackHolder);

               if (!segment.uploadContent(getCancelHolder(), uploadUrl, digest, mSource, mLength, signedUrl, mCallbackHolder)) {
                  dispatchUncounted(notifier);
                  dispatchFailure(User.Result.UploadVideo.STATUS_CONTENT_UPLOAD_SCHEDULING_FAILED);
               } else {
                  dispatchCounted(notifier);
               }

            } finally {
               destroy(setupRequest);
            }

         }
      }
   }

}
