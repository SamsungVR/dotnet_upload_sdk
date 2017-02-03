using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Text;
using System.Threading;

namespace SDKLib {

   public sealed class UserLiveEvent {

      private UserLiveEvent() {
      }

      public enum State {
         UNKNOWN,
         LIVE_CREATED,
         LIVE_CONNECTED,
         LIVE_DISCONNECTED,
         LIVE_FINISHED_ARCHIVED,
         LIVE_ACTIVE,
         LIVE_ARCHIVING
      }

      public enum FinishAction {
         ARCHIVE,
         DELETE
      }


      public enum Source {
         [Description("rtmp")]
         RTMP,
         [Description("segmented_ts")]
         SEGMENTED_TS,
         [Description("segmented_mp4")]
         SEGMENTED_MP4
      }

      public interface If {

         string getId();
         string getTitle();
         string getDescription();
         string getProducerUrl();
         string getViewUrl();

         /**
          * Queries the the details if the specific live event
          *
          * @param callback This may be NULL.
          * @param handler A handler on which callback should be called. If null, main handler is used.
          * @param closure An object that the application can use to uniquely identify this request.
          *                See callback documentation.
          * @return true if the query succeeded, false otherwise
          */

         //bool query(Result.Query.If callback, SynchronizationContext handler, object closure);

      }

      public sealed class Result {

         private Result() {
         }

         /**
          * This callback is used to provide status update for querying the details of a live event.
          */

         public sealed class Query {

            private Query() {
            }

            /**
             * The live event id provided in the request is invalid
             */

            public static readonly int INVALID_LIVE_EVENT_ID = 1;

            public interface If : VR.Result.BaseCallback.If, VR.Result.SuccessWithResultCallback.If<UserLiveEvent.If> {
            }
         }

         /**
          * This callback is used to provide status update for deleting a live event.
          */

         public sealed class Delete {

            private Delete() {
            }

            /**
             * The live event id provided in the request is invalid
             */

            public static readonly int INVALID_LIVE_EVENT_ID = 1;

            public interface If : VR.Result.BaseCallback.If, VR.Result.SuccessCallback.If {
            }
         }


         /**
         * This callback is used to provide status update for updating a live event.
         */

         public sealed class Finish {

            private Finish() {
            }

            /**
            * The live event id provided in the request is invalid
            */

            public static readonly int INVALID_LIVE_EVENT_ID = 1;

            public interface If : VR.Result.BaseCallback.If, VR.Result.SuccessCallback.If {
            }
         }

         /**
         * This callback is used to provide status update for updating a live event.
         */

         public sealed class SetPermission {

            private SetPermission() {
            }

            /**
            * The live event id provided in the request is invalid
            */

            public static readonly int INVALID_LIVE_EVENT_ID = 1;

            public interface If : VR.Result.BaseCallback.If, VR.Result.SuccessCallback.If {
            }
         }

         /**
          * This callback is used to provide status update for uploading a thumbnail for a live event.
          */

         public sealed class UploadThumbnail {

            private UploadThumbnail() {
            }

            /**
             * The live event id provided in the request is invalid
             */

            public static readonly int INVALID_LIVE_EVENT_ID = 1;

            public interface If : VR.Result.BaseCallback.If, VR.Result.SuccessCallback.If,
                 VR.Result.ProgressCallback.If {

            }
         }

      }
   }

}
