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
         PROP_METADATA = "metadata";

      public class CType : Contained.CType {

         public CType()
            : base(new string[] { PROP_ID, PROP_TITLE, PROP_PERMISSION, PROP_SOURCE, PROP_STEREOSCOPIC_TYPE,
                  PROP_DESCRIPTION, PROP_INGEST_URL, PROP_VIEW_URL, PROP_STATE, PROP_THUMBNAIL_URL,
                  PROP_VIEWER_COUNT, PROP_LIVE_STARTED, PROP_LIVE_STOPPED, PROP_METADATA}) {
         }

         public override SDKLib.Contained.If newInstance(Container.If container, JObject jsonObject) {
            return new UserLiveEventImpl((UserImpl)container, jsonObject);
         }

         public override object getContainedId(JObject jsonObject) {
            return jsonObject.GetValue(PROP_ID);
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
            if (PROP_ID.Equals(key) || PROP_TITLE.Equals(key) || PROP_DESCRIPTION.Equals(key) ||
               PROP_INGEST_URL.Equals(key) || PROP_VIEW_URL.Equals(key) || PROP_THUMBNAIL_URL.Equals(key)) {
               return (string)rawValue;
            }
            if (PROP_VIEWER_COUNT.Equals(key) || PROP_LIVE_STARTED.Equals(key) || PROP_LIVE_STOPPED.Equals(key)) {
               return long.Parse((string)rawValue);
            }

            if (PROP_METADATA.Equals(key)) {
               string st_type = (string)token["stereoscopic_type"];
               if (null == st_type) {
                  Log.d("VRSDK", "NULL, returning MONOSCOPIC");
                  return UserVideo.VideoStereoscopyType.MONOSCOPIC;
               }
               Log.d("VRSDK", "other " + st_type);
               if ("top-bottom".Equals(st_type)) {
                  Log.d("VRSDK", "returning TOP_BOTTOM_STEREOSCOPIC");
                  return UserVideo.VideoStereoscopyType.TOP_BOTTOM_STEREOSCOPIC;
               }
               if ("left-right".Equals(st_type)) {
                  Log.d("VRSDK", "returning LEFT_RIGHT_STEREOSCOPIC");
                  return UserVideo.VideoStereoscopyType.LEFT_RIGHT_STEREOSCOPIC;
               }
               if ("dual-fisheye".Equals(st_type)) {
                  Log.d("VRSDK", "returning DUAL_FISHEYE");
                  return UserVideo.VideoStereoscopyType.DUAL_FISHEYE;
               }
               Log.d("VRSDK", "default returning LEFT_RIGHT_STEREOSCOPIC");
               return UserVideo.VideoStereoscopyType.MONOSCOPIC;
            }
            if (PROP_STEREOSCOPIC_TYPE.Equals(key)) {
               string value = (string)rawValue;
               if ("top-bottom".Equals(value))
                  return UserVideo.VideoStereoscopyType.TOP_BOTTOM_STEREOSCOPIC;
               if ("left-right".Equals(value))
                  return UserVideo.VideoStereoscopyType.LEFT_RIGHT_STEREOSCOPIC;
               if ("dual-fisheye".Equals(value))
                  return UserVideo.VideoStereoscopyType.DUAL_FISHEYE;
               return UserVideo.VideoStereoscopyType.MONOSCOPIC;
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
         return (string)getLocked(PROP_ID);
      }

      public string getTitle() {
         return (string)getLocked(PROP_TITLE);
      }


      public string getDescription() {
         return (string)getLocked(PROP_DESCRIPTION);
      }



      public string getProducerUrl() {
         return (string)getLocked(PROP_INGEST_URL);
      }


      public string getViewUrl() {
         return (string)getLocked(PROP_VIEW_URL);
      }

      public UserVideo.VideoStereoscopyType getVideoStereoscopyType() {
         UserVideo.VideoStereoscopyType val = (UserVideo.VideoStereoscopyType)getLocked(PROP_METADATA);
         return val;
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

   }

}
