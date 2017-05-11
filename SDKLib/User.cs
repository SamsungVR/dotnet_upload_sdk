using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;

namespace SDKLib {

   /// <summary>  
   ///  The User class provides methods, enums and callbacks to manage users for SamsungVR.   
   /// </summary>  
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


         /// <summary>Upload a video</summary>
         /// <param name="callback">This may be NULL. SDK does not close the source parcel file descriptor.
         ///                        SDK transfers back ownership of the FD only on the callback.  Consider
         ///                        providing a Non Null callback so that the application can close the FD.L</param>
         /// <param name="handler">A handler on which callback should be called. If null, main handler is used.</param>
         /// <param name="closure">An object that the application can use to uniquely identify this request.</param>
         /// <returns>true if the upload was succesfully scheduled</returns>
         bool uploadVideo(Stream source, long length, string title, string description,
                     UserVideo.Permission permission, Result.UploadVideo.If callback,
                     SynchronizationContext handler, object closure);

         /// <summary>Upload a video using a JObject acquired from UserVideo.asJObject()</summary>
         /// <param name="callback">This may be NULL. SDK does not close the source parcel file descriptor.
         ///                        SDK transfers back ownership of the FD only on the callback.  Consider
         ///                        providing a Non Null callback so that the application can close the FD.L</param>
         /// <param name="handler">A handler on which callback should be called. If null, main handler is used.</param>
         /// <param name="closure">An object that the application can use to uniquely identify this request.</param>
         /// <returns>true if the upload was succesfully scheduled</returns>

         bool uploadVideo(Stream source, long length, JObject serializedUserVideo, Result.UploadVideo.If callback,
            SynchronizationContext handler, object closure);

         /// <summary>Cancel an already schedule video upload</summary>
         /// <param name="closure">An object that the application can use to uniquely identify this request.</param>
         /// <returns>true if the upload was succesfully canceled</returns>
         bool cancelUploadVideo(object closure);


         /// <summary>Creates a new live event</summary>
         /// <param name="title"> Short description of the live event. This field is shown by all the different players.</param>
         /// <param name="description"> Detailed description of the live event.</param>
         /// <param name="permission"> See UserVideo.Permission enum. This is how the privacy settings of the new live event can be set</param>
         /// <param name="protocol"> See UserLiveEvent.Protocol enum. Use this parameter to control how the inbond stream should be ingested</param>
         /// <param name="videoStereoscopyType">Describes the video projection used in the inbound stream.</param>
         /// <param name="callback">This may be NULL.</param>
         /// <param name="handler">A handler on which callback should be called. If null, main handler is used.</param>
         /// <param name="closure">An object that the application can use to uniquely identify this request.</param>
         /// <returns>true if the live even create request is succesfuly sent to the server</returns>
         bool createLiveEvent(string title, string description, UserVideo.Permission permission,
                                 UserLiveEvent.Source protocol, UserVideo.VideoStereoscopyType videoStereoscopyType,
                                 User.Result.CreateLiveEvent.If callback, SynchronizationContext handler, object closure);


         /// <summary>Queries the live events of the user</summary>
         /// <param name="callback">This may be NULL.</param>
         /// <param name="handler">A handler on which callback should be called. If null, main handler is used.</param>
         /// <param name="closure">An object that the application can use to uniquely identify this request.</param>
         /// <returns>true if the live event query request is succesfuly sent to the server</returns>
         bool queryLiveEvents(Result.QueryLiveEvents.If callback, SynchronizationContext handler, object closure);


         /// <summary>Given an live event id, return the corresponding live event</summary>
         /// <param name="liveEventId">The id of the live event to look up</param>
         /// <param name="callback">This may be NULL.</param>
         /// <param name="handler">A handler on which callback should be called. If null, main handler is used.</param>
         /// <param name="closure">An object that the application can use to uniquely identify this request.</param>
         /// <returns>true if the live event query request is succesfuly sent to the server</returns>
         bool queryLiveEvent(string liveEventId, UserLiveEvent.Result.Query.If callback, SynchronizationContext handler, object closure);

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

            /**
             * The chunk that was requested to be uploaded is more than the number of 
             * chunks the file has.  This could mean an attempt to upload a completely
             * uploaded file or the file was truncated after an initial upload
             */

            public static readonly int STATUS_CURRENT_CHUNK_GREATER_THAN_NUM_CHUNKS = 105;


         }

         ///<summary>Callback delivering results of createLiveEvent. Failure status codes are self explanatory.</summary>
         public sealed class CreateLiveEvent {

            private CreateLiveEvent() {
            }

            public interface If : VR.Result.BaseCallback.If, VR.Result.SuccessWithResultCallback.If<UserLiveEvent.If> {
            }

            ///<summary>Invalid stereoscopy type</summary>
            public static readonly int STATUS_INVALID_STEREOSCOPIC_TYPE = 5;


            ///<summary>Invalid audio type</summary>
            public static readonly int STATUS_INVALID_AUDIO_TYPE = 6;

         }


         ///<summary>Callback delivering results of queryLiveEvent request.</summary>
          public sealed class QueryLiveEvents {
           
           private QueryLiveEvents() {
           }

           public interface If : VR.Result.BaseCallback.If, VR.Result.SuccessWithResultCallback.If<List<UserLiveEvent.If>> {
           }
        }

      }
   }

}
