using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json.Linq;
using System.Threading;
using System.IO;
using System.Net;
using System.Security.Cryptography;

namespace SDKLib {


   internal class UserLiveEventSegmentImpl : UserLiveEventSegment.If {

      private static readonly string TAG = Util.getLogTag(typeof(UserLiveEventSegment));
      private static readonly bool DEBUG = Util.DEBUG;

      private readonly UserLiveEventImpl mUserLiveEvent;
      private readonly String mSegmentId;

      public UserLiveEventSegmentImpl(UserLiveEventImpl userLiveEvent, string segmentId) {
         mUserLiveEvent = userLiveEvent;
         mSegmentId = segmentId;
      }

      private string mInitialSignedUrl, mUploadUrl;
      private bool mUploading = false;
      private MD5 mMD5Digest;

      void setIsUploading(bool isUploading) {
         lock (this) {
            mUploading = isUploading;
         }
      }

      public bool uploadContent(ObjectHolder<bool> cancelHolder, string uploadUrl, MD5 md5Digest, Stream source, long length,
         string initialSignedUrl, ResultCallbackHolder callbackHolder) {
         lock (this) {
            if (mUploading) {
               return false;
            }
            mInitialSignedUrl = initialSignedUrl;
            mUploadUrl = uploadUrl;
            mMD5Digest = md5Digest;
            return retryUploadNoLock(cancelHolder, source, length,
                    (UserLiveEvent.Result.UploadSegment.If)callbackHolder.getCallbackNoLock(),
                    callbackHolder.getHandlerNoLock(), callbackHolder.getClosureNoLock());
         }
      }

      void onUploadComplete() {
         lock (this) {
            mInitialSignedUrl = null;
            mUploadUrl = null;
            mMD5Digest = null;
            mUploading = false;
         }
      }

      public bool cancelUpload(object closure) {
         if (DEBUG) {
            Log.d(TAG, "Cancelled segment upload requested this: " + this);
         }
         lock (this) {
            if (!mUploading) {
               return false;
            }
            return mUserLiveEvent.cancelUploadSegment(closure);
         }
      }

      private bool retryUploadNoLock(ObjectHolder<bool> cancelHolder, Stream source, long length,
          UserLiveEvent.Result.UploadSegment.If callback, SynchronizationContext handler, object closure) {
         if (null == mSegmentId || null == mInitialSignedUrl) {
            return false;
         }
         UserImpl user = mUserLiveEvent.getContainer() as UserImpl;
         APIClientImpl apiClient = user.getContainer() as APIClientImpl;

         AsyncWorkQueue workQueue = apiClient.getAsyncUploadQueue();

         WorkItemSegmentContentUpload workItem = (WorkItemSegmentContentUpload)workQueue.obtainWorkItem(WorkItemSegmentContentUpload.TYPE);
         workItem.set(cancelHolder, mUploadUrl, mMD5Digest, this, mUserLiveEvent, source, length,
                 mInitialSignedUrl, mSegmentId, callback, handler, closure);
         mUploading = workQueue.enqueue(workItem);
         return mUploading;

      }

      internal class WorkItemSegmentContentUpload : UserLiveEventImpl.WorkItemSegmentUploadBase {


         private UserLiveEventSegmentImpl mSegment;
         private Stream mSource;
         private long mLength;
         private string mSegmentId, mInitialSignedUrl;
         private UserLiveEventImpl mUserLiveEvent;
         private MD5 mMD5Digest;
         private string mUploadUrl;

         protected override void dispatchCounted(Util.CallbackNotifier notifier) {
            base.dispatchCounted(notifier);
            mSegment.setIsUploading(false);
         }

         protected override void dispatchCancelled() {
            base.dispatchCancelled();
            mSegment.onUploadComplete();
         }

         protected override void dispatchSuccess() {
            base.dispatchSuccess();
            mSegment.onUploadComplete();
         }

         private class WorkItemTypeSegmentContentUpload : AsyncWorkItemType {

            public AsyncWorkItem newInstance(APIClientImpl apiClient) {
               return new WorkItemSegmentContentUpload(apiClient);
            }
         }

         public static readonly AsyncWorkItemType TYPE = new WorkItemTypeSegmentContentUpload();

         WorkItemSegmentContentUpload(APIClientImpl apiClient)
            : base(apiClient, TYPE) {
         }


         public WorkItemSegmentContentUpload set(ObjectHolder<bool> cancelHolder,
             string uploadUrl, MD5 md5Digest,
             UserLiveEventSegmentImpl segment, UserLiveEventImpl userLiveEvent,
             Stream source, long length, string initialSignedUrl, string segmentId,
             UserLiveEvent.Result.UploadSegment.If callback, SynchronizationContext handler, object closure) {

            base.set(cancelHolder, callback, handler, closure);
            mSegment = segment;
            mUploadUrl = uploadUrl;
            mMD5Digest = md5Digest;
            mSegmentId = segmentId;
            mUserLiveEvent = userLiveEvent;
            mSource = source;
            mLength = length;
            mInitialSignedUrl = initialSignedUrl;
            return this;
         }


         override protected void recycle() {
            base.recycle();
            mSegment = null;
            mSource = null;
            mLength = -1;
            mUploadUrl = null;
            mInitialSignedUrl = null;
            mMD5Digest = null;
         }

         private static readonly string TAG = Util.getLogTag(typeof(WorkItemSegmentContentUpload));

         internal class DigestStream : HttpUploadStream {

            private readonly MD5 mDigest = MD5.Create();
            private readonly long mTotalBytes;
            private readonly ClientWorkItem mWorkItem;

            public DigestStream(ClientWorkItem workItem, Stream inner, MD5 digest, long total)
               : base(inner, workItem.getIOBuf(), total <= 0) {
               mDigest = digest;
               mTotalBytes = total;
               mWorkItem = workItem;
            }


            override protected bool canContinue() {
               return !mWorkItem.isCancelled();
            }


            override protected void onBytesProvided(byte[] data, int offset, int len) {
               mDigest.TransformBlock(data, offset, len, null, 0);
            }


            override protected void onProgress(long providedSoFar, bool isEOF) {
               ResultCallbackHolder holder = mWorkItem.getCallbackHolder();

               mWorkItem.dispatchUncounted(new ProgressCallbackNotifier(holder, providedSoFar, mTotalBytes));
            }

            public byte[] digest() {
               byte[] temp = new byte[0];
               mDigest.TransformFinalBlock(temp, 0, 0);
               return mDigest.Hash;
            }

         }



         override protected void onRun() {

            User.If user = mUserLiveEvent.getUser();
            Stream source = mSource;

            long length = mLength;

            HttpPlugin.PutRequest uploadRequest = null;
            HttpPlugin.PutRequest finishRequest = null;

            try {
               bool isChunked = length <= 0;

               try {
                  source.Seek(0, SeekOrigin.Begin);
               } catch (IOException ex) {
               }
               string content_type = "video/MP2T";
               if (this.mUserLiveEvent.getSource() == UserLiveEvent.Source.SEGMENTED_MP4) {
                  content_type = "video/mp4";
               }
               string[,] headers0 = {
                    {null, null},
                    {HEADER_CONTENT_TYPE, content_type},
                    {HEADER_CONTENT_TRANSFER_ENCODING, "binary"},
                };

               if (isChunked) {
                  headers0[0, 0] = HEADER_TRANSFER_ENCODING;
                  headers0[0, 1] = TRANSFER_ENCODING_CHUNKED;
               } else {
                  headers0[0, 0] = HEADER_CONTENT_LENGTH;
                  headers0[0, 1] = length.ToString();
               }

               uploadRequest = newRequest<HttpPlugin.PutRequest>(mInitialSignedUrl, HttpMethod.PUT, headers0);
               if (null == uploadRequest) {
                  dispatchFailure(VR.Result.STATUS_HTTP_PLUGIN_NULL_CONNECTION);
                  return;
               }

               DigestStream digestStream = new DigestStream(this, source, mMD5Digest, length);
               try {
                  writeHttpStream(uploadRequest, digestStream);
               } catch (Exception ex) {
                  if (isCancelled()) {
                     dispatchCancelled();
                     return;
                  }
                  throw ex;
               }

               HttpStatusCode rsp2 = getResponseCode(uploadRequest);

               if (!isHTTPSuccess(rsp2)) {
                  dispatchFailure(UserLiveEvent.Result.UploadSegment.STATUS_SEGMENT_UPLOAD_FAILED);
                  return;
               }

               destroy(uploadRequest);
               uploadRequest = null;

               byte[] digest = digestStream.digest();
               string hexDigest = Util.bytesToHex(digest, false);

               JObject jsonParam = new JObject();

               jsonParam.Add("status", "uploaded");
               jsonParam.Add("md5", hexDigest);

               string jsonStr = jsonParam.ToString(Newtonsoft.Json.Formatting.None);
               byte[] data = System.Text.Encoding.UTF8.GetBytes(jsonStr);

               string[,] headers1 = {
                    {HEADER_CONTENT_LENGTH, data.Length.ToString()},
                    {HEADER_CONTENT_TYPE, "application/json" + ClientWorkItem.CONTENT_TYPE_CHARSET_SUFFIX_UTF8},
                    {UserImpl.HEADER_SESSION_TOKEN, user.getSessionToken()},
                    {APIClientImpl.HEADER_API_KEY, mAPIClient.getApiKey()},
                };

               finishRequest = newPutRequest(mUploadUrl, headers1);
               if (null == finishRequest) {
                  dispatchFailure(VR.Result.STATUS_HTTP_PLUGIN_NULL_CONNECTION);
                  return;
               }

               writeBytes(finishRequest, data, jsonStr);

               if (isCancelled()) {
                  dispatchCancelled();
                  return;
               }

               HttpStatusCode rsp = getResponseCode(finishRequest);

               if (!isHTTPSuccess(rsp)) {
                  dispatchFailure(UserLiveEvent.Result.UploadSegment.STATUS_SEGMENT_END_NOTIFY_FAILED);
                  return;
               }

               dispatchSuccess();

            } finally {
               destroy(uploadRequest);
               destroy(finishRequest);
            }

         }
      }
   }

}
