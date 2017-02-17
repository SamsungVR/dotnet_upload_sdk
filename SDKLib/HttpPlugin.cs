using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace SDKLib {

   public sealed class HttpPlugin {

      private HttpPlugin() {
      }

      /**
       * Assume class X is your implementation of BaseRequest or any of its descendant interfaces.
       * Let m be an instance of X, that is X m = new X();
       * m will be touched from only one thread. This thread may not be the UI thread.
       * Let n be an instance of X, that is X n = new X();
       * m and n could be in parallel use in different threads.
       */
      public interface BaseRequest {

         /**
          * Destroy this request object. Cleanup any HTTP resources here.
          *
          * @throws Exception
          */

         void destroy();
      }

      /**
       * A request that allows reading from the server.
       */

      public interface ReadableRequest : BaseRequest {

         /**
          * Used by the SDK to read data from the server.
          *
          * @return an InputStream from which server data can be read. The SDK will close this stream.
          */

         System.IO.Stream input();

         /**
          * Response code for this request
          *
          * @return an HTTP resoponse code
          */

         HttpStatusCode responseCode();
      }

      /**
       * A request that allows writing to the server
       */

      public interface WritableRequest : BaseRequest {

         /**
          * This method is called by the SDK requesting the plugin to copy data from the
          * provided input stream to the HTTP socket - i.e. send data to the server.
          *
          * @param input The input stream from which data must be read and written to the HTTP
          *              socket
          * @param buf This is a helper byte buffer. The application can use this buffer for
          *            reading from the input stream and writing to the HTTP socket. This
          *            is usually a per-thread buffer maintained by the SDK, and hence is much
          *            cheaper to use than allocating a new buffer for every write request.  This
          *            is also appropriately sized for the task at hand. For example, to
          *            send a large file, this could be a 1MB buffer.  For sending JSON, could
          *            be 1KB buffer.
          */

         void output(System.IO.Stream input, byte[] buf);
      }

      /**
       * A request that allows bi-drectional ordered communication.  Writes happen before reads.
       */

      public interface ReadableWritableRequest : ReadableRequest, WritableRequest {
      }

      /**
       * A HTTP Get request
       */
      public interface GetRequest : ReadableRequest {
      }

      /**
       * A HTTP Post request
       */

      public interface PostRequest : ReadableWritableRequest {
      }

      /**
       * A HTTP Delete request
       */

      public interface DeleteRequest : ReadableRequest {
      }

      /**
       * A HTTP Put request
       */

      public interface PutRequest : ReadableWritableRequest {
      }

      /**
       * A factory interface that allows creation of HTTP Get, Post, Put and Delete requests.
       * These methods will be called from any thread; the implementation MUST be thread safe.
       * The implementation MUST establish a HTTP connection to url, and send the http headers on that
       * request. The http headers for the most part will contain the content-length.  If content-length
       * is missing, it means that chunked mode is desired.  The headers provided here are all
       * that are needed by the server to respond.  The implementation does not need to
       * add any additional headers.
       */

      public interface RequestFactory {
         GetRequest newGetRequest(string url, string[,] headers);
         PostRequest newPostRequest(string url, string[,] headers);
         DeleteRequest newDeleteRequest(string url, string[,] headers);
         PutRequest newPutRequest(string url, string[,] headers);
      }

      private class BaseRequestImpl : BaseRequest {

         private static readonly string TAG = Util.getLogTag(typeof(HttpPlugin));

         protected readonly HttpWebRequest mHttpRequest;
         private readonly System.Globalization.CultureInfo mCulture = System.Globalization.CultureInfo.CreateSpecificCulture("en-US");

         protected BaseRequestImpl(string method, string url, string[,] headers) {
            mHttpRequest = HttpWebRequest.Create(url) as HttpWebRequest;
            mHttpRequest.Timeout = -1;
            mHttpRequest.ReadWriteTimeout = -1;
            mHttpRequest.AllowWriteStreamBuffering = false;
            mHttpRequest.Method = method;
            mHttpRequest.KeepAlive = false;
                
            if (null != headers) {
               int count = headers.GetLength(0);
               Log.d(TAG, "Adding headers count: " + count);
               for (int i = 0; i < count; i += 1) {
                  string attr = headers[i, 0];
                  string attrLowerCase = attr.ToLower(mCulture);
                  string value = headers[i, 1];
                  Log.d(TAG, "Add header attr: " + attr + " value: " + value + " index: " + i);
                  if ("content-length".Equals(attrLowerCase)) {
                     mHttpRequest.ContentLength = long.Parse(value);
                     continue;
                  }
                  if ("content-type".Equals(attrLowerCase)) {
                     mHttpRequest.ContentType = value;
                     continue;
                  }
                  mHttpRequest.Headers.Add(attr, value);
               }
            }
         }

         public virtual void destroy() {
         }
      }

      private class ReadableRequestImpl : BaseRequestImpl, ReadableRequest {

         protected HttpWebResponse mResponse = null;

         protected ReadableRequestImpl(string method, string url, string[,] headers) :
            base(method, url, headers) {
         }

         public System.IO.Stream input() {
            HttpWebResponse response = getResponse();
            return response.GetResponseStream();
         }

         private HttpWebResponse getResponse() {
            if (null == mResponse) {
               try {
                  mResponse = mHttpRequest.GetResponse() as HttpWebResponse;
               } catch (WebException ex) {
                  if (ex.Response is HttpWebResponse) {
                     mResponse = ex.Response as HttpWebResponse;
                  }
               }
            }
            return mResponse;
         }

         public HttpStatusCode responseCode() {
            HttpWebResponse response = getResponse();
            HttpStatusCode statusCode = default(HttpStatusCode);
            if (null != response) {
               statusCode = response.StatusCode;
            }
            return statusCode;
         }

         public override void destroy() {
            base.destroy();
            if (null != mResponse) {
               mResponse.Close();
            }
         }
      }

      private class ReadableWriteableRequestImpl : ReadableRequestImpl, WritableRequest {

         public ReadableWriteableRequestImpl(string method, string url, string[,] headers) :
            base(method, url, headers) {
         }

         public void output(System.IO.Stream input, byte[] buf) {
            System.IO.Stream output = mHttpRequest.GetRequestStream();
            if (null != output) {
               int len;

               while ((len = input.Read(buf, 0, buf.Length)) > 0) {
                  output.Write(buf, 0, len);
               }
               output.Close();
            }
         }
      }

      private class GetRequestImpl : ReadableRequestImpl, GetRequest {
         public GetRequestImpl(string url, string[,] headers) : 
            base("GET", url, headers) {
         }
      }

      private class PostRequestImpl : ReadableWriteableRequestImpl, PostRequest {
         public PostRequestImpl(string url, string[,] headers) : 
            base("POST", url, headers) {
         }
      }

      private class PutRequestImpl : ReadableWriteableRequestImpl, PutRequest {
         public PutRequestImpl(string url, string[,] headers) : 
            base("PUT", url, headers) {
         }
      }

      private class DeleteRequestImpl : ReadableRequestImpl, DeleteRequest {
         public DeleteRequestImpl(string url, string[,] headers) : 
            base("DELETE", url, headers) {
         }
      }

      public class RequestFactoryImpl : RequestFactory {

         public GetRequest newGetRequest(string url, string[,] headers) {
            return new GetRequestImpl(url, headers);
         }

         public PostRequest newPostRequest(string url, string[,] headers) {
            return new PostRequestImpl(url, headers);
         }

         public DeleteRequest newDeleteRequest(string url, string[,] headers) {
            return new DeleteRequestImpl(url, headers);
         }

         public PutRequest newPutRequest(string url, string[,] headers) {
            return new PutRequestImpl(url, headers);
         }
      }
   }
}