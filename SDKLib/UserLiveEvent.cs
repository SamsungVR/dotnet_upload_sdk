using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Text;
using System.Threading;

namespace SDKLib {

    /// <summary>  
    ///  The UserLiveEvent class provides methods, enums and callbacks to manage live events for SamsungVR.   
    /// </summary>  
   public sealed class UserLiveEvent {

      private UserLiveEvent() {
      }

      /// <summary>  
      ///  The State enum has all the possible states a live event can be in.   
      /// </summary>  
      public enum State {


         ///<summary>The server sent an unexpected state string. Most likely the SamsungVR server and the UploadSDK are out of sync. </summary>
         UNKNOWN,


         ///<summary>The live event has been created but there was no further activity. </summary>
         LIVE_CREATED,


         ///<summary>Successful RTMP connect / publish was made </summary>
         LIVE_CONNECTED,


         ///<summary>After a successful RTMP connect the RTMP client disconnected from the server. </summary>
         LIVE_DISCONNECTED,


         ///<summary>The live event is archived and cannot be changed anymore. The content is playable as VOD </summary>
         LIVE_FINISHED_ARCHIVED,


         ///<summary>The live even is currently active. It's source is producing input and the players should be able to play the stream. </summary>
         LIVE_ACTIVE,


         ///<summary>The server received a LIVE_FINISHED_ARCHIVE state change request which is currently 
         ///in the queue to be executed or the archiving process is ongoing.  </summary>
         LIVE_ARCHIVING
      }


      /// <summary>  
      ///  The Source enum has all the possible sources the content from a live event can come from.   
      /// </summary>  
      public enum Source {

         ///<summary>The live is an RTMP stream, it provides h264 live video through the server provided RTMP URL</summary>
         [Description("rtmp")]
         RTMP,

         ///<summary>The live is delivered through segmented trasport stream (.TS) chunks. </summary>
         [Description("segmented_ts")]
         SEGMENTED_TS,

         ///<summary>The live is delivered through mp4 chunks. </summary>
         [Description("segmented_mp4")]
         SEGMENTED_MP4
      }

      public interface If {

          /// <summary>Returns the ID of a live event</summary>
          /// <returns>ID of the live event</returns>
         string getId();


         /// <summary>Returns the title of a live event</summary>
         /// <returns>Title of the live event</returns>
         string getTitle();


         /// <summary>Returns the description of a live event</summary>
         /// <returns>Description of the live event</returns>
         string getDescription();


         /// <summary>Returns the producer URL  of an RTMP live event. 
         /// This is the RTMP URL the streaming application should connect and send the RTMP live feed. </summary>
         /// <returns>Produces URL of the live event</returns>
         string getProducerUrl();


         /// <summary>Returns the view UR of a live event. 
         /// This is a sharable URL. When used, the SamsungVR GearVR, 
         /// SamsungVR Android or SamsugVR Web player will be started 
         /// based on platform and availabity to play the live event</summary>
         /// <returns>View UR of the live event</returns>
         string getViewUrl();


         /// <summary>Returns the view geometry (stereoscopy type) of a live event
         /// This important imformation determines how the server and the player will interpret 
         /// the incoming live stream. </summary>
         /// <returns>View geometry of the live event</returns>

         UserVideo.VideoStereoscopyType getVideoStereoscopyType();


         /// <summary>Returns the current state of a live event</summary>
         /// <returns>Current state of the live event</returns>
         UserLiveEvent.State getState();


         /// <summary>Returns the current number of viewers a live event</summary>
         /// <returns>Number of viewers of the live event</returns>
         long getViewerCount();


         /// <summary>Returns the source of the live event. 
         /// This helps determine how the live video feed should be delivered to the server</summary>
         /// <returns>Source of the live event</returns>
         UserLiveEvent.Source getSource();


         /// <summary>Returns the permission settings of a live event.
         /// This flag indicates who is allowed to see the live event</summary>
         /// <returns>Permission settings of the live event</returns>
         UserVideo.Permission getPermission();


         /// <summary>Returns the time the live event started</summary>
         /// <returns>Start time in milliseconds since epoch</returns>
         long getStartedTime();


         /// <summary>Returns the time the live event finished</summary>
         /// <returns>Finish time in milliseconds since epoch</returns>
         long getFinishedTime();


         /// <summary>Returns the URL of the thumbnail displayed with this live event.</summary>
         /// <returns>Thumbnail URL</returns>
         string getThumbnailUrl();


         /// <summary>Returns the user who is the creator / owner of the live event</summary>
         /// <returns>Owner of the live event</returns>
         User.If getUser();


         /// <summary>Queries the the details of a specific live event</summary>
         /// <param name="callback">The callback to be called when the query operation is finished. This may be NULL</param>
         /// <param name="handler">A handler on which callback should be called. If null, main handler is used.</param>
         /// <param name="closure">An object that the application can use to uniquely identify this request.</param>
         /// <returns>true if the query succeeded, false otherwise</returns>
         bool query(Result.Query.If callback, SynchronizationContext handler, object closure);


         /// <summary>Sets the state of the live event to FINISHED.</summary>
         /// <param name="action">The action the server should take after marking the live event finished.
         ///                      Currently FINISHED_ARCHIVED is the only accepted value, which instructs
         ///                      the server to make the entire finished live event playble as streaming VOD</param>
         /// <param name="callback">The callback to be called when the operation is finished. This may be NULL</param>
         /// <param name="handler">A handler on which callback should be called. If null, main handler is used.</param>
         /// <param name="closure">An object that the application can use to uniquely identify this request.</param>
         /// <returns>true if the state change succeeded</returns>
         bool finish(Result.Finish.If callback, SynchronizationContext handler, object closure);

         /// <summary>Deletes a live event</summary>
         /// <param name="callback">The callback to be called when the operation is finished. This may be NULL</param>
         /// <param name="handler">A handler on which callback should be called. If null, main handler is used.</param>
         /// <param name="closure">An object that the application can use to uniquely identify this request.</param>
         /// <returns>true if the delete operation succeeded</returns>
         bool delete(Result.Delete.If callback, SynchronizationContext handler, object closure);


         /// <summary>Uploads a live segment. This method is only for segmented (non RTMP) live types. </summary>
         /// <param name="source">Ownership of this FD passes onto the SDK from this point onwards till the
         ///                      results are delivered via callback. The SDK may use a FileChannel to change the
         ///                      file pointer position.  The SDK will not close the FD. It is the application's
         ///                     responsibility to close the FD on success, failure, cancel or exception.</param>
         /// <param name="callback">The callback to be called when the operation is finished. 
         /// This may be NULL. SDK does not close the source parcel file descriptor.
         /// SDK transfers back ownership of the FD only on the callback.  Consider
         /// providing a Non Null callback so that the application can close the FD.</param>
         /// <param name="handler">A handler on which callback should be called. If null, main handler is used.</param>
         /// <param name="closure">An object that the application can use to uniquely identify this request.</param>
         /// <returns>true if the upload was started</returns>
         bool uploadSegmentFromFD(Stream source, long length,
             UserLiveEvent.Result.UploadSegment.If callback, SynchronizationContext handler, object closure);


         /// <summary>Cancels and already started live segment upload. 
         /// <param name="closure">An object that the application can use to uniquely identify this request.</param>
         /// <returns>true if the upload was succesfully canceled</returns>
         bool cancelUploadSegment(object closure);

      }


      public sealed class Result {

         private Result() {
         }


         /// <summary>This callback is used to provide status update for querying the details of a live event.</summary>
         public sealed class Query {

            private Query() {
            }


            /// <summary>The live event id provided in the request is invalid</summary>
            public static readonly int INVALID_LIVE_EVENT_ID = 1;

            public interface If : VR.Result.BaseCallback.If, VR.Result.SuccessWithResultCallback.If<UserLiveEvent.If> {
            }
         }


         /// <summary>This callback is used to provide status update for deleting a live event.</summary>>
         public sealed class Delete {

            private Delete() {
            }

            /// <summary>The live event id provided in the request is invalid</summary>            
            public static readonly int INVALID_LIVE_EVENT_ID = 1;

            public interface If : VR.Result.BaseCallback.If, VR.Result.SuccessCallback.If {
            }
         }


         /// <summary>This callback is used to provide status update for updating a live event.</summary>>
         public sealed class Finish {

            private Finish() {
            }

            /// <summary>The live event id provided in the request is invalid</summary>            
            public static readonly int INVALID_LIVE_EVENT_ID = 1;

            public interface If : VR.Result.BaseCallback.If, VR.Result.SuccessCallback.If {
            }
         }

         /// <summary>This callback is used to provide status update for updating a live event.</summary>>
         public sealed class SetPermission {

            private SetPermission() {
            }

            /// <summary>The live event id provided in the request is invalid</summary>            
            public static readonly int INVALID_LIVE_EVENT_ID = 1;

            public interface If : VR.Result.BaseCallback.If, VR.Result.SuccessCallback.If {
            }
         }

         /// <summary>This callback is used to provide status update for updating a thumbnail of a live event.</summary>>
         public sealed class UploadThumbnail {

            private UploadThumbnail() {
            }

            /// <summary>The live event id provided in the request is invalid</summary>            
            public static readonly int INVALID_LIVE_EVENT_ID = 1;

            public interface If : VR.Result.BaseCallback.If, VR.Result.SuccessCallback.If,
                 VR.Result.ProgressCallback.If {

            }
         }

         /// <summary>This callback is used to provide status update for uploading a segment of a live event.</summary>>
         public sealed class UploadSegment {

            private UploadSegment() {
            }


            /// <summary>The live event id provided in the request is invalid</summary>            
            public static readonly int INVALID_LIVE_EVENT_ID = 1;

            public static readonly int STATUS_SEGMENT_NO_MD5_IMPL = 101;
            public static readonly int STATUS_SEGMENT_UPLOAD_FAILED = 102;
            public static readonly int STATUS_SEGMENT_END_NOTIFY_FAILED = 103;

            public interface If : VR.Result.BaseCallback.If, VR.Result.SuccessCallback.If,
                 VR.Result.ProgressCallback.If {

               /// <summary>The server issued a video id for this upload.  The contents of the video may not have been uploaded yet.</summary>>
               void onSegmentIdAvailable(object closure, UserLiveEventSegment.If segment);
            }
         }
      }
   }
}
