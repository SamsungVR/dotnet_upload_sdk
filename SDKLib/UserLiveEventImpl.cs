using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json.Linq;
using System.Threading;
using System.IO;
using System.Net;

namespace SDKLib {


   internal class UserLiveEventImpl : Contained.BaseImpl<UserImpl>, UserLiveEvent.If {

      private static readonly string TAG = Util.getLogTag(typeof(UserImpl));


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

      internal class WorkItemQuery : ClientWorkItem {

         private class WorkItemTypeQuery : AsyncWorkItemType {

            public AsyncWorkItem newInstance(APIClientImpl apiClient) {
               return new WorkItemQuery(apiClient);
            }
         }

         public static readonly AsyncWorkItemType TYPE = new WorkItemTypeQuery();


         public WorkItemQuery(APIClientImpl apiClient)
            : base(apiClient, TYPE) {
         }

         private UserImpl mUser;
         private string mUserLiveEventId;
         private UserLiveEventImpl mUserLiveEventImpl;


         public WorkItemQuery set(UserImpl user, string userLiveEventId, UserLiveEventImpl userLiveEventImpl,
                UserLiveEvent.Result.Query.If callback, SynchronizationContext handler, object closure) {
            base.set(callback, handler, closure);
            mUserLiveEventImpl = userLiveEventImpl;
            mUserLiveEventId = userLiveEventId;
            mUser = user;
            return this;
         }

         protected override void recycle() {
            base.recycle();
            mUser = null;
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

   }

}
