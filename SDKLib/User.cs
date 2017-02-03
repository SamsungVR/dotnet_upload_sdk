using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;

namespace SDKLib {

   public sealed class User {

      private User() {
      }

      public interface Observer {
      }

      public interface If : Observable.If<User.Observer> {
         string getName();
         string getEmail();
         string getProfilePicUrl();
         string getSessionToken();
         string getUserId();

         /**
          * Upload a video
          *
          * @param callback This may be NULL. SDK does not close the source parcel file descriptor.
          *                 SDK transfers back ownership of the FD only on the callback.  Consider
          *                 providing a Non Null callback so that the application can close the FD.
          * @param handler A handler on which callback should be called. If null, main handler is used.
          * @param closure An object that the application can use to uniquely identify this request.
          *                See callback documentation.
          * @return true if the upload was schedule, false otherwise
          */

         bool uploadVideo(Stream source, long length, string title, string description,
                     UserVideo.Permission permission, Result.UploadVideo.If callback,
                     SynchronizationContext handler, object closure);
         bool cancelUploadVideo(object closure);

         /**
          * Creates a new live event
          *
          * @param title Short description of the live event. This field is shown by all the different players.
          * @param description Detailed description of the live event.
          * @param permission See UserVideo.Permission enum. This is how the privacy settings of the new
          *                   live event can be set
          * @param protocol See UserLiveEvent.Protocol enum. Use this parameter to control how the inbond
          *                 stream should be ingested
          * @param videoStereoscopyType See UserLiveEvent.VideoStereoscopyType enum.
          *                             Describes the video projection used in the inbound stream.
          * @param callback This may be NULL. SDK does not close the source parcel file descriptor.
          *                 SDK transfers back ownership of the FD only on the callback.  Consider
          *                 providing a Non Null callback so that the application can close the FD.
          * @param handler A handler on which callback should be called. If null, main handler is used.
          * @param closure An object that the application can use to uniquely identify this request.
          *                See callback documentation.
          * @return true if the live event got created, false otherwise
          */

         bool createLiveEvent(string title, string description, UserVideo.Permission permission,
                                 UserLiveEvent.Source protocol, UserVideo.VideoStereoscopyType videoStereoscopyType,
                                 User.Result.CreateLiveEvent.If callback, SynchronizationContext handler, object closure);
      }

      public sealed class Result {

         private Result() {
         }

         /**
          * Callback delivering results of uploadVideo. Most status codes
          * are self explanatory. The non-obvious ones are documented.
          */

         public sealed class UploadVideo {

            private UploadVideo() {
            }

            public static readonly int STATUS_OUT_OF_UPLOAD_QUOTA = 1;
            public static readonly int STATUS_BAD_FILE_LENGTH = 3;
            public static readonly int STATUS_FILE_LENGTH_TOO_LONG = 4;
            public static readonly int STATUS_INVALID_STEREOSCOPIC_TYPE = 5;
            public static readonly int STATUS_INVALID_AUDIO_TYPE = 6;


            public interface If : VR.Result.BaseCallback.If,
                   VR.Result.ProgressCallback.If, VR.Result.SuccessCallback.If {

               /**
                * The server issued a video id for this upload.  The contents
                * of the video may not have been uploaded yet.
                */
               void onVideoIdAvailable(object closure, UserVideo.If video);

            }


            public static readonly int STATUS_CHUNK_UPLOAD_FAILED = 101;

            /**
             * An attempt to query the url for the next chunk upload failed.
             */
            public static readonly int STATUS_SIGNED_URL_QUERY_FAILED = 102;

            /**
             * An attempt to schedule the content upload onto a background
             * thread failed. This may indicate that the system is low on resources.
             * The user could attempt to kill unwanted services/processess and retry
             * the upload operation
             */

            public static readonly int STATUS_CONTENT_UPLOAD_SCHEDULING_FAILED = 103;

            /**
             * The file has been modified while the upload was in progress. This could
             * be a checksum mismatch or file length mismatch.
             */

            public static readonly int STATUS_FILE_MODIFIED_AFTER_UPLOAD_REQUEST = 104;


         }

         /**
          * Callback delivering results of createLiveEvent. Failure status codes are self
          * explanatory.
          */

         public sealed class CreateLiveEvent {

            private CreateLiveEvent() {
            }

            public interface If : VR.Result.BaseCallback.If, VR.Result.SuccessWithResultCallback.If<UserLiveEvent.If> {
            }

            public static readonly int STATUS_MISSING_STREAMING_PROTOCOL = 1;
            public static readonly int STATUS_INVALID_STREAMING_PROTOCOL = 2;
            public static readonly int STATUS_MISSING_DURATION = 3;
            public static readonly int STATUS_INVALID_DURATION = 4;
            public static readonly int STATUS_INVALID_STEREOSCOPIC_TYPE = 5;
            public static readonly int STATUS_INVALID_AUDIO_TYPE = 6;
            public static readonly int STATUS_MISSING_START_TIME = 7;
            public static readonly int STATUS_INVALID_START_TIME_FORMAT = 8;
            public static readonly int STATUS_START_TIME_IN_PAST = 9;
            public static readonly int STATUS_START_TIME_TOO_FAR_IN_FUTURE = 10;
            public static readonly int STATUS_MISSING_INGEST_BITRATE = 11;
            public static readonly int STATUS_INGEST_BITRATE_TOO_LOW = 12;
            public static readonly int STATUS_INGEST_BITRATE_TOO_HIGH = 13;

         }

        public sealed class QueryLiveEvents {
           
           private QueryLiveEvents() {
           }

           public interface If : VR.Result.BaseCallback.If, VR.Result.SuccessWithResultCallback.If<List<UserLiveEvent>> {
           }
        }

      }
   }

}
