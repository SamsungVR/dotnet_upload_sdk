using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Net;
using System.Threading;

namespace SDKLib {


   internal class UserVideoImpl : UserVideo.If {

      private static readonly string TAG = Util.getLogTag(typeof(UserVideo));

      private readonly UserImpl mUser;
      private readonly string mTitle, mDesc;
      private UserVideo.Permission mPermission;
      private readonly int mChunkSize, mNumChunks;
      private readonly string mVideoId, mUploadId, mInitialSignedUrl;
      private readonly JObject mJObject;


      private static object getAttrFromJObject(JObject jObject, string attr, object d) {
         JToken value = null;
         if (!jObject.TryGetValue(attr, out value)) {
            return d;
         }
         return ((JValue)value).Value;
      }

      public static UserVideoImpl fromJObject(UserImpl user, JObject jObject) {
         
         string title = (string)getAttrFromJObject(jObject, "title", null);
         string desc = (string)getAttrFromJObject(jObject, "desc", null);
         string permission = (string)getAttrFromJObject(jObject, "permission", null);
         string videoId = (string)getAttrFromJObject(jObject, "videoId", null);
         string uploadId = (string)getAttrFromJObject(jObject, "uploadId", null);
         string initialSignedUrl = (string)getAttrFromJObject(jObject, "initialSignedUrl", null);
         if (null == title || null == desc || null == permission || null == videoId || null == uploadId
            || null == initialSignedUrl) {
            return null;
         }
         object temp = getAttrFromJObject(jObject, "chunkSize", -1);
         long chunkSize = (long)temp;
         temp = getAttrFromJObject(jObject, "numChunks", -1);
         long numChunks = (long)temp;
         if (-1 == chunkSize || -1 == numChunks) {
            return null;
         }
         long lastSuccessfulChunk = (long)getAttrFromJObject(jObject, "lastSuccessfulChunk", -1);

         return new UserVideoImpl(user, title, desc, UserVideo.fromString(permission), videoId,
            uploadId, initialSignedUrl, (int)chunkSize, (int)numChunks, (int)lastSuccessfulChunk);
      }

      public UserVideoImpl(UserImpl user, string title, string desc, UserVideo.Permission permission,
         string videoId, string uploadId, string initialSignedUrl, int chunkSize, int numChunks) :
         this(user, title, desc, permission, videoId, uploadId, initialSignedUrl, chunkSize, numChunks, -1) {
      }

      public UserVideoImpl(UserImpl user, string title, string desc, UserVideo.Permission permission,
         string videoId, string uploadId, string initialSignedUrl, int chunkSize, int numChunks, 
         int lastSuccessfulChunk) {
         mUser = user;
         mTitle = title;
         mDesc = desc;
         mPermission = permission;
         mVideoId = videoId;
         mUploadId = uploadId;
         mInitialSignedUrl = initialSignedUrl;
         mChunkSize = chunkSize;
         mNumChunks = numChunks;
         mLastSuccessfulChunk = lastSuccessfulChunk;
      }

      private int mLastSuccessfulChunk = -1;
      private bool mUploading = false;

      public void setIsUploading(bool isUploading) {
         lock (this) {
            Log.d(TAG, "setIsUploading value: " + isUploading + " this: " + this);
            mUploading = isUploading;
         }
      }

      public void setLastSuccessfulChunk(int chunk) {
         lock (this) {
            if (chunk >= 0 && chunk < mNumChunks) {
               mLastSuccessfulChunk = chunk;
            }
         }
      }

      public bool uploadContent(ObjectHolder<bool> cancelHolder, Stream source, long length, ResultCallbackHolder callbackHolder) {
         lock (this) {
            if (mUploading) {
               return false;
            }
            return retryUploadNoLock(cancelHolder, source, length,
               (User.Result.UploadVideo.If)callbackHolder.getCallbackNoLock(),
               callbackHolder.getHandlerNoLock(), callbackHolder.getClosureNoLock());
         }
      }

      public void onUploadComplete() {
         lock (this) {
            mUploading = false;
         }
      }

      public bool cancelUpload(object closure) {
         Log.d(TAG, "Cancelled video upload requested this: " + this);
         lock (this) {
            if (!mUploading) {
               return false;
            }
            return mUser.cancelUploadVideo(closure);
         }
      }

      public bool retryUpload(Stream source, long length, User.Result.UploadVideo.If callback,
                                 SynchronizationContext handler, object closure) {
         Log.d(TAG, "Retry video upload requested this: " + this);
         lock (this) {
            if (mUploading) {
               return false;
            }
            return retryUploadNoLock(null, source, length, callback, handler, closure);
         }
      }

      private bool retryUploadNoLock(ObjectHolder<bool> cancelHolder, Stream source, long length,
          User.Result.UploadVideo.If callback, SynchronizationContext handler, object closure) {
         APIClientImpl apiClient = mUser.getContainer() as APIClientImpl;
         AsyncWorkQueue workQueue = apiClient.getAsyncUploadQueue();
         WorkItemVideoContentUpload workItem = (WorkItemVideoContentUpload)workQueue.obtainWorkItem(WorkItemVideoContentUpload.TYPE);
         workItem.set(cancelHolder, this, mUser, source, length, mInitialSignedUrl, mVideoId, mUploadId,
                 mChunkSize, mNumChunks, mLastSuccessfulChunk,
                 callback, handler, closure);
         mUploading = workQueue.enqueue(workItem);
         return mUploading;
      }

      public JObject toJObject(JObject jObject) {
         if (null == jObject) {
            jObject = new JObject();
         } else {
            jObject.RemoveAll();
         }
         jObject.Add("userId", new JValue(mUser.getUserId()));
         jObject.Add("title", new JValue(mTitle));
         jObject.Add("desc", new JValue(mDesc));
         jObject.Add("permission", new JValue(UserVideo.toString(mPermission)));
         jObject.Add("videoId", new JValue(mVideoId));
         jObject.Add("uploadId", new JValue(mUploadId));
         jObject.Add("initialSignedUrl", new JValue(mInitialSignedUrl));
         jObject.Add("chunkSize", mChunkSize);
         jObject.Add("numChunks", mNumChunks);
         lock (this) {
            jObject.Add("lastSuccessfulChunk", mLastSuccessfulChunk);
         }
         return jObject;
      }

      public class WorkItemVideoContentUpload : UserImpl.WorkItemVideoUploadBase {

         private class WorkItemTypeVideoContentUpload : AsyncWorkItemType {

            public AsyncWorkItem newInstance(APIClientImpl apiClient) {
               return new WorkItemVideoContentUpload(apiClient);
            }
         }

         private UserVideoImpl mVideo;
         private Stream mSource;
         private int mChunkSize, mNumChunks, mLastSuccessfulChunk;
         private long mLength;
         private string mVideoId, mUploadId, mInitialSignedUrl;
         private UserImpl mUser;

         protected override void dispatchCounted(Util.CallbackNotifier notifier) {
            base.dispatchCounted(notifier);
            mVideo.setIsUploading(false);
         }

         protected override void dispatchCancelled() {
            base.dispatchCancelled();
            mVideo.onUploadComplete();
         }

         protected override void dispatchSuccess() {
            base.dispatchSuccess();
            mVideo.onUploadComplete();
         }

         public static readonly AsyncWorkItemType TYPE = new WorkItemTypeVideoContentUpload();

         WorkItemVideoContentUpload(APIClientImpl apiClient)
            : base(apiClient, TYPE) {
         }


         public WorkItemVideoContentUpload set(ObjectHolder<bool> cancelHolder,
             UserVideoImpl video, UserImpl user, Stream source, long length,
             string initialSignedUrl, string videoId, string uploadId, int chunkSize, int numChunks,
             int lastSuccessfulChunk, User.Result.UploadVideo.If callback, SynchronizationContext handler, object closure) {

            base.set(cancelHolder, callback, handler, closure);
            mVideo = video;
            mUser = user;
            mLastSuccessfulChunk = lastSuccessfulChunk;
            mSource = source;
            mLength = length;
            mInitialSignedUrl = initialSignedUrl;
            mVideoId = videoId;
            mUploadId = uploadId;
            mChunkSize = chunkSize;
            mNumChunks = numChunks;
            return this;
         }

         protected override void recycle() {
            base.recycle();
            mUser = null;
            mSource = null;
            mLength = 0;
            mInitialSignedUrl = null;
            mVideoId = null;
            mUploadId = null;
            mVideo = null;
         }

         private static readonly string TAG = Util.getLogTag(typeof(WorkItemVideoContentUpload));

         private string nextChunkUploadUrl(string userId, string videoId, string uploadId,
                                         string[,] headers, int chunkId, bool readData) {

            HttpPlugin.GetRequest nextRequest = null;

            try {
               string url = string.Format("user/{0}/video/{1}/upload/{2}/{3}/next",
                       userId, videoId, uploadId, chunkId);
               Log.d(TAG, "Requesting next chunk endpoint from: " + url);
               nextRequest = newGetRequest(url, headers);
               if (null == nextRequest) {
                  dispatchFailure(VR.Result.STATUS_HTTP_PLUGIN_NULL_CONNECTION);
                  return null;
               }
               HttpStatusCode rsp3 = getResponseCode(nextRequest);

               if (!isHTTPSuccess(rsp3)) {
                  dispatchFailure(User.Result.UploadVideo.STATUS_SIGNED_URL_QUERY_FAILED);
                  return null;
               }
               if (!readData) {
                  return null;
               }

               string data3 = readHttpStream(nextRequest, "code: " + rsp3);
               if (null == data3) {
                  dispatchFailure(VR.Result.STATUS_HTTP_PLUGIN_STREAM_READ_FAILURE);
                  return null;
               }
               JObject jsonObject2 = JObject.Parse(data3);
               string signedUrl = Util.jsonOpt<string>(jsonObject2, "signed_url", null);
               if (null == signedUrl) {
                  dispatchFailure(User.Result.UploadVideo.STATUS_SIGNED_URL_QUERY_FAILED);
                  return null;
               }
               return signedUrl;
            } finally {
               destroy(nextRequest);
            }

         }

         private class MySplitStream : SplitStream {

            private readonly WorkItemVideoContentUpload mVideoContentUpload;

            public MySplitStream(WorkItemVideoContentUpload videoContentUpload,
               Stream aBase, long totalLen, long chunkLen)
               : base(aBase, totalLen, chunkLen) {
               mVideoContentUpload = videoContentUpload;
            }

            protected override bool canContinue() {
               return !mVideoContentUpload.isCancelled();
            }

         }

         protected override void onRun() {

            UserImpl user = mUser;
            string videoId = mVideoId;
            string uploadId = mUploadId;
            int chunkSize = mChunkSize, numChunks = mNumChunks,
                    lastSuccessfulChunk = mLastSuccessfulChunk;
            Stream source = mSource;
            UserVideoImpl video = mVideo;

            int currentChunk = lastSuccessfulChunk + 1;
            long filePos = currentChunk * chunkSize;

            string[,] headers0 = {
                {UserImpl.HEADER_SESSION_TOKEN, user.getSessionToken()},
                {APIClientImpl.HEADER_API_KEY, mAPIClient.getApiKey()},
            };

            long length = mLength;

            if ((numChunks * chunkSize) < length) {
               dispatchFailure(User.Result.UploadVideo.STATUS_FILE_MODIFIED_AFTER_UPLOAD_REQUEST);
               return;
            }

            long remaining = length - filePos;
            if (remaining < 1) {
               dispatchFailure(User.Result.UploadVideo.STATUS_FILE_MODIFIED_AFTER_UPLOAD_REQUEST);
               return;
            }

            Log.d(TAG, "Uploading content for videoId: " + videoId + " uploadId: " + uploadId +
                  " chunkSize: " + chunkSize + " numChunks: " + numChunks +
                  " lastSuccessfulChunk: " + lastSuccessfulChunk + " length: " + length +
                  " currentPos: " + filePos + " remaining: " + remaining);

            try {
               source.Seek(filePos, SeekOrigin.Begin);

               MySplitStream split = new MySplitStream(this, source, remaining, chunkSize);

               string[,] headers2 = {
                        /*
                         * Content length must be the first header. Index 0 is used to set the
                         * real length later
                         */
                        {HEADER_CONTENT_LENGTH, "0"},
                        {HEADER_CONTENT_TYPE, "application/octet-stream"},
                        {HEADER_CONTENT_TRANSFER_ENCODING, "binary"}

                };

               for (int i = currentChunk; i < numChunks; i++) {

                  if (isCancelled()) {
                     dispatchCancelled();
                     return;
                  }

                  float progress = 100f * ((float)(i) / (float)numChunks);

                  dispatchUncounted(new ProgressCallbackNotifier(mCallbackHolder, i, numChunks));

                  string signedUrl;

                  if (i == 0) {

                     signedUrl = mInitialSignedUrl;

                  } else {
                     signedUrl = nextChunkUploadUrl(user.getUserId(), videoId, uploadId,
                             headers0, i - 1, true);
                     if (null == signedUrl) {
                        return;
                     }
                  }


                  Log.d(TAG, "Uploading chunk: " + i + " url: " + signedUrl);
                  if (isCancelled()) {
                     dispatchCancelled();
                     return;
                  }

                  HttpPlugin.PutRequest uploadRequest = null;

                  try {
                     split.renew();
                     headers2[0, 1] = split.availableAsLong().ToString();

                     uploadRequest = newRequest<HttpPlugin.PutRequest>(signedUrl, HttpMethod.PUT, headers2);
                     if (null == uploadRequest) {
                        dispatchFailure(VR.Result.STATUS_HTTP_PLUGIN_NULL_CONNECTION);
                        return;
                     }
                     try {
                        writeHttpStream(uploadRequest, split);
                     } catch (Exception ex) {
                        Log.d(TAG, "Exception writing chunk ex: " + ex);
                        throw ex;
                     }

                     if (isCancelled()) {
                        dispatchCancelled();
                        return;
                     }

                     HttpStatusCode rsp2 = getResponseCode(uploadRequest);

                     if (!isHTTPSuccess(rsp2)) {
                        dispatchFailure(User.Result.UploadVideo.STATUS_CHUNK_UPLOAD_FAILED);
                        return;
                     }

                     video.setLastSuccessfulChunk(i);

                  } finally {
                     destroy(uploadRequest);
                  }
               }
               dispatchUncounted(new ProgressCallbackNotifier(mCallbackHolder));

               Log.d(TAG, "After successful upload, bytes remaining: " + split.availableAsLong());
               /*
                * next of the last chunk is what triggers the server to declare
                * that file upload is complete
                */
               nextChunkUploadUrl(user.getUserId(), videoId, uploadId, headers0, numChunks - 1, false);
               dispatchSuccess();

            } finally {
            }
         }

      }
   }

}
