using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Net;
using System.Threading;

namespace SDKLib {

   internal abstract class ClientWorkItem : AsyncWorkItem {


      protected class ProgressCallbackNotifier : Util.CallbackNotifier {

         private readonly long mComplete, mMax;
         private readonly float mProgress;

         public ProgressCallbackNotifier(ResultCallbackHolder callbackHolder)
            : this(callbackHolder, 1, 1) {
         }

         public ProgressCallbackNotifier(ResultCallbackHolder callbackHolder, long complete, long max)
            : base(callbackHolder) {
            mMax = max;
            mComplete = complete;
            if (mMax > 0) {
               mProgress = (float)(100.0 * ((double)mComplete / (double)mMax));
            } else {
               mProgress = -1.0f;
            }
         }

         protected override void notify(object callback, object closure) {
            VR.Result.ProgressCallback.If tCallback = callback as VR.Result.ProgressCallback.If;

            if (mProgress < 0.0f) {
               tCallback.onProgress(closure, mComplete);
            } else {
               tCallback.onProgress(closure, mProgress, mComplete, mMax);
            }
         }
      }

      protected class CancelledCallbackNotifier : Util.CallbackNotifier {

         public CancelledCallbackNotifier(ResultCallbackHolder callbackHolder)
            : base(callbackHolder) {
         }

         protected override void notify(object callback, object closure) {
            VR.Result.BaseCallback.If tCallback = callback as VR.Result.BaseCallback.If;
            tCallback.onCancelled(closure);
         }

      }

      private class ExceptionCallbackNotifier : Util.CallbackNotifier {

         private readonly Exception mException;

         public ExceptionCallbackNotifier(ResultCallbackHolder callbackHolder, Exception exception)
            : base(callbackHolder) {
            mException = exception;
         }

         protected override void notify(object callback, object closure) {
            VR.Result.BaseCallback.If tCallback = callback as VR.Result.BaseCallback.If;
            tCallback.onException(closure, mException);
         }
      }

      protected readonly APIClientImpl mAPIClient;
      protected readonly object mLock = new object();

      public ClientWorkItem(APIClientImpl apiClient, AsyncWorkItemType type)
         : base(type) {
         mAPIClient = apiClient;
      }

      protected abstract void onRun();

      private static string TAG = Util.getLogTag(typeof(ClientWorkItem));

      private int mDispatchedCount;
      protected readonly ResultCallbackHolder mCallbackHolder = new ResultCallbackHolder();

      protected virtual void dispatchCounted(Util.CallbackNotifier notifier) {
         dispatchUncounted(notifier);
         mDispatchedCount += 1;
         onDispatchCounted(mDispatchedCount);
      }

      protected virtual void onDispatchCounted(int count) {
      }

      protected virtual void dispatchUncounted(Util.CallbackNotifier notifier) {
         notifier.post();
      }

      protected virtual void dispatchCancelled() {
         dispatchCounted(new CancelledCallbackNotifier(mCallbackHolder));
      }

      protected virtual void dispatchFailure(int status) {
         dispatchCounted(new Util.FailureCallbackNotifier(mCallbackHolder, status));
      }

      protected virtual void dispatchSuccess() {
         dispatchCounted(new Util.SuccessCallbackNotifier(mCallbackHolder));
      }

      protected virtual void dispatchSuccessWithResult<T>(T rf) {
         dispatchCounted(new Util.SuccessWithResultCallbackNotifier<T>(mCallbackHolder, rf));
      }

      protected virtual void dispatchException(Exception ex) {
         dispatchCounted(new ExceptionCallbackNotifier(mCallbackHolder, ex));
      }

      public override void run() {
         mDispatchedCount = 0;

         Log.d(TAG, "Running work item: " + this + " type: " + getType());
         if (isCancelled()) {
            dispatchCancelled();
         } else {
            try {
               onRun();
               if ((mDispatchedCount < 1) && isCancelled()) {
                  dispatchCancelled();
               }
            } catch (Exception ex) {
               Log.d(TAG, "Exception occured on work item: " + this
                       + " type: " + getType() + " ex: " + ex + " stack: " + ex.StackTrace);
               dispatchException(ex);
            }
         }
         if (1 != mDispatchedCount) {
            throw new Exception("Invalid number of dispatches made, count: " + mDispatchedCount);
         }
      }



      protected virtual void set(VR.Result.BaseCallback.If callback, SynchronizationContext handler, object closure) {
         mCallbackHolder.setNoLock(callback, handler, closure);
      }

      protected virtual string toRESTUrl(string suffix) {
         string result = null;
         result = string.Format("{0}/{1}", mAPIClient.getEndPoint(), suffix);
         return result;
      }

      protected enum HttpMethod {
         GET,
         POST,
         DELETE,
         PUT
      }

      protected virtual HttpStatusCode getResponseCode(HttpPlugin.ReadableRequest request) {
         HttpStatusCode responseCode = request.responseCode();
         Log.d(TAG, "Returning response code " + responseCode + " from request " + request);
         return responseCode;
      }

      protected virtual bool isHTTPSuccess(HttpStatusCode responseCode) {
         return HttpStatusCode.OK.CompareTo(responseCode) <= 0 && HttpStatusCode.BadRequest.CompareTo(responseCode) > 0;
      }

      protected virtual T newRequest<T>(string url, HttpMethod method,
                      string[,] headers) where T : HttpPlugin.BaseRequest {
         HttpPlugin.RequestFactory reqFactory = mAPIClient.getRequestFactory();
         T result = default(T);
         if (null == reqFactory) {
            return result;
         }
         switch (method) {
            case HttpMethod.GET:
               result = (T)reqFactory.newGetRequest(url, headers);
               break;
            case HttpMethod.POST:
               result = (T)reqFactory.newPostRequest(url, headers);
               break;
            case HttpMethod.DELETE:
               result = (T)reqFactory.newDeleteRequest(url, headers);
               break;
            case HttpMethod.PUT:
               result = (T)reqFactory.newPutRequest(url, headers);
               break;
         }
         return result;
      }

      internal static readonly string HEADER_CONTENT_TYPE = "Content-Type";
      internal static readonly string HEADER_CONTENT_LENGTH = "Content-Length";
      internal static readonly string HEADER_CONTENT_DISPOSITION = "Content-Disposition";
      internal static readonly string HEADER_CONTENT_TRANSFER_ENCODING = "Content-Transfer-Encoding";
      internal static readonly string HEADER_COOKIE = "Cookie";
      internal static readonly string CONTENT_TYPE_CHARSET_SUFFIX_UTF8 = "; charset=utf-8";

      private T newEndPointRequest<T>(string urlSuffix, HttpMethod method, string[,] headers) where T : HttpPlugin.BaseRequest {
         T result = default(T);
         string restUrl = toRESTUrl(urlSuffix);
         if (null == restUrl) {
            return result;
         }
         return newRequest<T>(restUrl, method, headers);
      }

      protected virtual HttpPlugin.GetRequest newGetRequest(string suffix, string[,] headers) {
         return newEndPointRequest<HttpPlugin.GetRequest>(suffix, HttpMethod.GET, headers);
      }

      protected virtual HttpPlugin.PostRequest newPostRequest(string suffix, string[,] headers) {
         return newEndPointRequest<HttpPlugin.PostRequest>(suffix, HttpMethod.POST, headers);
      }

      protected virtual HttpPlugin.DeleteRequest newDeleteRequest(string suffix, string[,] headers) {
         return newEndPointRequest<HttpPlugin.DeleteRequest>(suffix, HttpMethod.DELETE, headers);
      }

      protected virtual HttpPlugin.PutRequest newPutRequest(string suffix, string[,] headers) {
         return newEndPointRequest<HttpPlugin.PutRequest>(suffix, HttpMethod.PUT, headers);
      }

      private WebRequest initConnection(WebRequest con) {
         return con;
      }

      protected virtual string toCookieString(string[,] cookies) {
         string cookieStr = "";
         if (null != cookies) {
            for (int i = 0; i < cookies.Length; i += 1) {
               if (i > 0) {
                  cookieStr += "; ";
               }
               cookieStr += cookies[i, 0] + "=" + cookies[i, 1];
            }
         }
         return cookieStr;
      }

      protected virtual void destroy(HttpPlugin.BaseRequest request) {
         Log.d(TAG, "Disconnecting " + request);
         if (null != request) {
            request.destroy();
         }
      }

      protected virtual void writeBytes(HttpPlugin.WritableRequest request, byte[] data, string debugMsg) {
         int len = data.Length;
         if (null != debugMsg) {
            Log.d(TAG, "Writing len: " + len + " msg: " + debugMsg);
         }
         MemoryStream bis = new MemoryStream(data);
         request.output(bis, mIOBuf);
         bis.Close();

      }

      private string readHttpStream(Stream stream, string debugMsg) {
         StreamReader bos = new StreamReader(stream);
         string result = bos.ReadToEnd();
         bos.Close();
         if (null != debugMsg) {
            Log.d(TAG, "readHttpStream str: " + result + " msg: " + debugMsg);
         }
         return result;
      }

      private bool closeInputStream(Stream stream) {
         if (null == stream) {
            return false;
         }
         stream.Close();
         return true;
      }

      protected virtual string readHttpStream(HttpPlugin.ReadableRequest request, string debugMsg) {
         Stream input = null;
         try {
            input = request.input();
            if (null == input) {
               return null;
            }
            return readHttpStream(input, debugMsg);
         } finally {
            closeInputStream(input);
         }
      }

      protected virtual void writeHttpStream(HttpPlugin.WritableRequest request, Stream input) {
         Log.d(TAG, "Writing input stream to output stream " + request);
         request.output(input, mIOBuf);
         Log.d(TAG, "Done writing to stream " + request);
      }

      protected override void recycle() {
         base.recycle();
         mCallbackHolder.clearNoLock();
      }

      protected virtual void set(object callback, SynchronizationContext handler, object closure) {
         mCallbackHolder.setNoLock(callback, handler, closure);
      }

      protected virtual void set(ResultCallbackHolder callbackHolder) {
         mCallbackHolder.copyFromNoLock(callbackHolder);
      }

      public object getClosure() {
         return mCallbackHolder.getClosureNoLock();
      }

      private SynchronizationContext getHandler() {
         return mCallbackHolder.getHandlerNoLock();
      }


      internal static readonly string HYPHENS = "--";
      internal static readonly string QUOTE = "\"";
      internal static readonly string ENDL = "\r\n";

      protected static string headersToString(string[,] headers) {
         if (null == headers) {
            return null;
         }
         int len = headers.Length;
         if (len < 1) {
            return null;
         }
         string result = string.Empty;
         for (int i = 0; i < headers.Length; i += 1) {
            string attr = headers[i, 0];
            string value = headers[i, 1];
            result += attr + ": " + value + ENDL;
         }
         return result;
      }

      private static string makeBoundary() {
         return Guid.NewGuid().ToString();
      }

      protected virtual bool setupPost(WebRequest connection, string contentType, bool includeApiKey, string[][] args) {
         connection.Method = "POST";
         if (null != contentType) {
            connection.ContentType = contentType;
         }
         if (includeApiKey) {
            connection.Headers.Add("X-API-KEY", mAPIClient.getApiKey());
         }
         if (null != args) {
            foreach (string[] arg in args) {
               connection.Headers.Add(arg[0], arg[1]);
            }
         }
         return true;
      }

      protected virtual WebRequest newConnection(string suffix) {
         string restUrl = toRESTUrl(suffix);
         if (null == restUrl) {
            return null;
         }
         WebRequest con = null;

         try {
            con = WebRequest.Create(restUrl);
         } catch (Exception) {
            return null;
         }
         return initConnection(con);
      }

      protected virtual bool writeJson(WebRequest connection, JObject jsonObject) {
         Stream os = null;
         StreamWriter sw = null;
         bool success = true;
         try {
            os = connection.GetRequestStream();
            sw = new StreamWriter(os);
            string jsonStr = jsonObject.ToString();
            sw.Write(jsonStr);
            sw.Flush();
         } catch (Exception) {
            success = false;
         } finally {
            if (null != sw) {
               sw.Dispose();
            }
            if (null != os) {
               os.Dispose();
            }
         }
         return success;
      }


      protected class SplitStream : Stream {

         private class LengthHolder {

            private long mTotal, mAvailable;

            public void setTotal(long total) {
               mTotal = total;
            }

            public long getAvailable() {
               return mAvailable;
            }

            public void onRead(int len) {
               if (-1 == len) {
                  mAvailable = 0;
               } else {
                  mAvailable = Math.Max(0, mAvailable - len);
               }
            }

            public void renew() {
               mAvailable = mTotal;
            }

         }

         private readonly LengthHolder mChunkInfo = new LengthHolder(), mBaseInfo = new LengthHolder();
         private readonly Stream mBase;

         public SplitStream(Stream aBase, long totalLen, long chunkLen) {
            mBase = aBase;

            mChunkInfo.setTotal(chunkLen);
            mBaseInfo.setTotal(totalLen);
            mBaseInfo.renew();
         }

         public virtual long availableAsLong() {
            long a = mChunkInfo.getAvailable();
            long b = mBaseInfo.getAvailable();
            return Math.Min(a, b);
         }


         public virtual void renew() {
            mChunkInfo.renew();
         }


         private void onRead(int len) {
            mChunkInfo.onRead(len);
            mBaseInfo.onRead(len);
         }


         protected virtual bool canContinue() {
            return true;
         }

         public override bool CanRead {
            get {
               return true;
            }
         }

         public override bool CanSeek {
            get {
               return false;
            }
         }

         public override bool CanWrite {
            get {
               return false;
            }
         }

         public override void Flush() {
            throw new NotImplementedException();
         }

         public override long Length {
            get {
               return availableAsLong();
            }
         }

         public override long Position {
            get {
               throw new NotImplementedException();
            }
            set {
               throw new NotImplementedException();
            }
         }

         public override int Read(byte[] buffer, int byteOffset, int byteCount) {
            long available = availableAsLong();
            if (!canContinue()) {

               Log.d(TAG, "Cannot continue, available: " + available);
               return -1;
            }
            byteOffset = Math.Max(byteOffset, 0);
            int bufAvailable = Math.Min(buffer.Length - byteOffset, byteCount);
            int canRead = (int)Math.Min(bufAvailable, available);
            Log.d(TAG, "Split pre read buf remaining: " + available + " canRead: " +
                    canRead + " byteCount: " + byteCount + " byteOffset: " + byteOffset);
            if (canRead < 1) {
               return -1;
            }
            int wasRead = mBase.Read(buffer, byteOffset, canRead);
            onRead(wasRead);
            Log.d(TAG, "Split post read buf remaining: " + availableAsLong() + " canRead: " +
                    canRead + " wasRead: " + wasRead + " byteCount: " + byteCount +
                    " byteOffset: " + byteOffset);
            return wasRead;

         }

         public override long Seek(long offset, SeekOrigin origin) {
            throw new NotImplementedException();
         }

         public override void SetLength(long value) {
            throw new NotImplementedException();
         }

         public override void Write(byte[] buffer, int offset, int count) {
            throw new NotImplementedException();
         }

      }

      protected class HttpUploadStream : Stream {

         private readonly ByteArrayHolder[] mBufs = new ByteArrayHolder[3] { new ByteArrayHolder(true), new ByteArrayHolder(false), new ByteArrayHolder(true) };

         private class ByteArrayHolder {

            internal readonly bool mIsPseudo;

            internal ByteArrayHolder(bool isPseudo) {
               mIsPseudo = isPseudo;
               clear();
            }

            private byte[] mArray;
            int mMark, mLen;

            internal int set(byte[] array, int offset, int len) {
               mLen = len;
               mArray = array;
               mMark = 0;
               return mLen;
            }

            internal int available() {
               return mLen - mMark;
            }

            internal int set(byte[] array) {
               if (null == array) {
                  return set(null, 0, 0);
               } else {
                  return set(array, 0, array.Length);
               }
            }

            internal void clear() {
               set(null, 0, 0);
            }

            internal int read(byte[] dst, int dstOffset, int dstCount) {
               if (null != mArray) {
                  int remain = available();
                  if (remain > 0) {
                     int toCopy = Math.Min(remain, dstCount);
                     Array.Copy(mArray, mMark, dst, dstOffset, toCopy);
                     mMark += toCopy;
                     return toCopy;
                  }
               }
               return 0;
            }
         }

         public override long Position {
            get {
               throw new NotImplementedException();
            }
            set {
               throw new NotImplementedException();
            }
         }

         public override long Length {
            get {
               return 0;
            }
         }

         public override bool CanRead {
            get {
               return true;
            }
         }

         public override bool CanSeek {
            get {
               return false;
            }
         }

         public override bool CanWrite {
            get {
               return false;
            }
         }

         public override void Flush() {
            throw new NotImplementedException();
         }

         public override long Seek(long offset, SeekOrigin origin) {
            throw new NotImplementedException();
         }

         public override void SetLength(long value) {
            throw new NotImplementedException();
         }

         public override void Write(byte[] buffer, int offset, int count) {
            throw new NotImplementedException();
         }

         public override void Close() {
         }

         public override int Read(byte[] buffer, int byteOffset, int byteCount) {
            if (buffer.Length < (byteOffset + byteCount)) {
               throw new IOException();
            }
            int canRead = byteCount;
            int totalRead = 0;

            while (canContinue() && canRead > 0 && ensureAvailable()) {
               int read = 0;
               for (int i = 0; i < mBufs.Length; i += 1) {
                  ByteArrayHolder holder = mBufs[i];
                  if (holder.available() > 0) {
                     int offset = byteOffset + totalRead + read;
                     int thisRead = holder.read(buffer, offset, canRead);
                     if (thisRead > 0) {
                        canRead -= thisRead;
                        onProvided(holder, buffer, offset, thisRead);
                        read += thisRead;
                     }
                  }
               }
               totalRead += read;
            }
            if (totalRead < 1) {
               totalRead = -1;
            }
            onProgress(mProvidedSoFar, totalRead > 0);
            return totalRead;
         }

         private long mProvidedSoFar = 0;

         private bool ensureAvailable() {
            long available = mBufs[0].available() + mBufs[1].available() + mBufs[2].available();
            if (available > 0) {
               return true;
            }
            mBufs[0].clear();
            mBufs[1].clear();
            mBufs[2].clear();

            int read = mInner.Read(mIOBuf, 0, mIOBuf.Length);
            if (read < 1) {
               return false;
            }
            available = 0;
            if (mIsChunked) {
               byte[] header = System.Text.Encoding.UTF8.GetBytes(read.ToString() + ClientWorkItem.ENDL);
               available += mBufs[0].set(header);
               available += mBufs[2].set(System.Text.Encoding.UTF8.GetBytes(ClientWorkItem.ENDL));
            }
            available += mBufs[1].set(mIOBuf, 0, read);
            return available > 0;
         }

         protected bool isChunked() {
            return mIsChunked;
         }

         private void onProvided(ByteArrayHolder holder, byte[] data, int offset, int len) {
            if (!holder.mIsPseudo) {
               onBytesProvided(data, offset, len);
               mProvidedSoFar += len;
            }
         }

         protected void onBytesProvided(byte[] data, int offset, int len) {

         }

         protected void onProgress(long providedSoFar, bool isEOF) {

         }

         protected bool canContinue() {
            return true;
         }

         private readonly Stream mInner;
         private readonly bool mIsChunked;
         private readonly byte[] mIOBuf;

         protected HttpUploadStream(Stream inner, byte[] ioBuf, bool isChunked) {
            mInner = inner;
            mIsChunked = isChunked;
            mIOBuf = null == ioBuf ? new byte[8192] : ioBuf;
         }

      }
   }

}
