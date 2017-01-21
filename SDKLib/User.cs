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
      }
   }

}
