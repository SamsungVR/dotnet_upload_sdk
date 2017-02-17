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
         UserVideo.VideoStereoscopyType getVideoStereoscopyType();
         UserLiveEvent.State getState();
         long getViewerCount();
         UserLiveEvent.Source getSource();
         UserVideo.Permission getPermission();
         long getStartedTime();
         long getFinishedTime();
         string getThumbnailUrl();

         User.If getUser();

         /**
          * Queries the the details if the specific live event
          *
          * @param callback This may be NULL.
          * @param handler A handler on which callback should be called. If null, main handler is used.
          * @param closure An object that the application can use to uniquely identify this request.
          *                See callback documentation.
          * @return true if the query succeeded, false otherwise
          */

         bool query(Result.Query.If callback, SynchronizationContext handler, object closure);

         /**
          * Sets the state of the live event FINISHED.
          *
          * @param action  The action the server should take after marking the live event finished.
          *                Currently FINISHED_ARCHIVED is the only accepted value, which instructs
          *                the server to make the entire finished live event playble as streaming VOD
          * @param callback This may be NULL. SDK does not close the source parcel file descriptor.
          *                 SDK transfers back ownership of the FD only on the callback.  Consider
          *                 providing a Non Null callback so that the application can close the FD.
          * @param handler A handler on which callback should be called. If null, main handler is used.
          * @param closure An object that the application can use to uniquely identify this request.
          *                See callback documentation.
          * @return true if the live event got deleted, false otherwise
          */

         bool finish(Result.Finish.If callback, SynchronizationContext handler, object closure);

         /**
          * Deletes a new live event
          *
          * @param callback This may be NULL..
          * @param handler A handler on which callback should be called. If null, main handler is used.
          * @param closure An object that the application can use to uniquely identify this request.
          *                See callback documentation.
          * @return true if the live event got deleted, false otherwise
          */

         bool delete(Result.Delete.If callback, SynchronizationContext handler, object closure);

         /**
           * Upload a video
           *
           * @param source Ownership of this FD passes onto the SDK from this point onwards till the
           *               results are delivered via callback. The SDK may use a FileChannel to change the
           *               file pointer position.  The SDK will not close the FD. It is the application's
           *               responsibility to close the FD on success, failure, cancel or exception.
           * @param callback This may be NULL. SDK does not close the source parcel file descriptor.
           *                 SDK transfers back ownership of the FD only on the callback.  Consider
           *                 providing a Non Null callback so that the application can close the FD.
           * @param handler A handler on which callback should be called. If null, main handler is used.
           * @param closure An object that the application can use to uniquely identify this request.
           *                See callback documentation.
           * @return true if the upload was started, false otherwise
           */

         bool uploadSegmentFromFD(Stream source, long length,
             UserLiveEvent.Result.UploadSegment.If callback, SynchronizationContext handler, object closure);

         /**
          * Cancels an already started segment upload
          *
          * @param closure An object that the application can use to uniquely identify this request.
          *                See callback documentation.
          * @return true if the upload was successful, false otherwise
          */

         bool cancelUploadSegment(object closure);

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

         /**
          * This callback is used to provide status update for uploading a live event segment.
          */

         public sealed class UploadSegment {

            private UploadSegment() {
            }

            public static readonly int INVALID_LIVE_EVENT_ID = 1;

            public static readonly int STATUS_SEGMENT_NO_MD5_IMPL = 101;
            public static readonly int STATUS_SEGMENT_UPLOAD_FAILED = 102;
            public static readonly int STATUS_SEGMENT_END_NOTIFY_FAILED = 103;

            public interface If : VR.Result.BaseCallback.If, VR.Result.SuccessCallback.If,
                 VR.Result.ProgressCallback.If {

               /**
                * The server issued a video id for this upload.  The contents
                * of the video may not have been uploaded yet.
                */
               void onSegmentIdAvailable(object closure, UserLiveEventSegment.If segment);
            }
         }
      }
   }
}
