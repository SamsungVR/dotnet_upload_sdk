using System;
using System.Threading;

namespace SDKLib {

   public static class VR {

      /**
       * The result class is used as a grouping (akin to namespace) for callbacks that provide
       * results for requests asynchronously. Results are dispatched to Handlers provided by the
       * application and callbacks are called on the Threads backing the Handlers.
       */

      public sealed class Result {

         private Result() {
         }

        /**
         * These status codes are common across all requests.  Some of them, for example
         * STATUS_MISSING_API_KEY, STATUS_INVALID_API_KEY are not meant for the end user.
         */

         /**
          * User id in HTTP header is invalid
          */

         public static readonly int STATUS_INVALID_USER_ID = 1002;

         /**
          * Session token in HTTP header is missing or invalid
          */

         public static readonly int STATUS_MISSING_X_SESSION_TOKEN_HEADER = 1003;
         public static readonly int STATUS_INVALID_SESSION = 1004;

         /**
          * API key in HTTP header is missing or invalid
          */

         public static readonly int STATUS_MISSING_API_KEY = 1005;
         public static readonly int STATUS_INVALID_API_KEY = 1006;

         private static readonly int STATUS_HTTP_BASE = 1 << 16;

         /**
          * Http Plugin returned a NULL connection object
          */

         public static readonly int STATUS_HTTP_PLUGIN_NULL_CONNECTION = STATUS_HTTP_BASE | 1;

         /**
          * Http Plugin provided input stream for reading server responses could not be read
          */

         public static readonly int STATUS_HTTP_PLUGIN_STREAM_READ_FAILURE = STATUS_HTTP_BASE | 2;


         private static readonly int STATUS_SERVER_RESPONSE_BASE = 2 << 16;

         /**
          * We were unable to process the server response json. Possible causes are response
          * did not contain JSON, or JSON is missing important data
          */
         public static readonly int STATUS_SERVER_RESPONSE_INVALID = STATUS_SERVER_RESPONSE_BASE | 1;

         /**
          * The server returned a HTTP Error code, but no JSON based status was found.
          */

         public static readonly int STATUS_SERVER_RESPONSE_NO_STATUS_CODE = STATUS_SERVER_RESPONSE_BASE | 2;

         private static readonly int STATUS_SDK_BASE = 3 << 16;

         /**
          * This feature is not supported
          */

         public static readonly int STATUS_FEATURE_NOT_SUPPORTED = STATUS_SDK_BASE | 1;

         /**
          * The initializer group of callbacks. Used by VR.init()
          */


         public static readonly int STATUS_UNKNOWN_FAILURE = -1;

         public sealed class Init {

            private Init() {
            }

            public static readonly int STATUS_ALREADY_INITIALIZED = 1;

            public interface If {
               void onSuccess(object closure);
               void onFailure(object closure, int status);
            }

         }

         public sealed class FailureCallback {

            private FailureCallback() {
            }

            public interface If {

               /**
                * The request failed
                *
                *
                * @param closure Application provided object used to identify this request.
                * @param status The reason for failure. These are specific to the request. The callback
                *               interface for the request will have these defined.  Also, status'es
                *               common for any request, from VR.Result could also be provided here.
                */

               void onFailure(object closure, int status);
            }
         }

         public sealed class BaseCallback {

            private BaseCallback() {
            }

            public interface If : FailureCallback.If {

               /**
                * This request was cancelled.  The request is identified by the closure param.
                *
                * @param closure Application provided object used to identify this request.
                */

               void onCancelled(object closure);

               /**
                * This request resulted in an exception.  Some exceptions can be analyzed to take
                * corrective actions.  For example:  if HTTP Socket Write timeout is a user configurable
                * param, the application should examine the exception to see if it is a SocketWriteTimeout
                * exception.  If so, the user could be prompted to increase the timeout value.
                *
                * In other cases, the application should log these exceptions for review by
                * engineering.
                *
                * @param closure Application provided object used to identify this request.
                */

               void onException(object closure, Exception ex);

            }
         }


         /**
          * A callback used to notify progress of a long running request
          */

         public sealed class ProgressCallback {

            private ProgressCallback() {
            }

            public interface If {
               /**
                * The latest progress update.
                *
                * @param closure Application provided object used to identify this request.
                * @param progressPercent Progress percentage between 0.0 to 100.0
                */

               void onProgress(object closure, float progressPercent, long complete, long max);
               void onProgress(object closure, long complete);
            }
         }


         /**
          * Callbacks used to notify success of a request. Two types are defined here, one with
          * a result object, and the other with only the closure
          */
         public sealed class SuccessCallback {

            private SuccessCallback() {
            }

            public interface If {
               /**
                * The request was successful
                *
                * @param closure Application provided object used to identify this request.
                */

               void onSuccess(object closure);
            }

         }

         public sealed class SuccessWithResultCallback {

            private SuccessWithResultCallback() {
            }

            public interface If<T> {

               /**
                * The request was successful and a result object was located or created
                *
                * @param result The object created or located for the request made
                * @param closure Application provided object used to identify this request.
                */

               void onSuccess(object closure, T result);
            }
         }


        /**
         * Callback for the getUserBySessionId request. The success callback has result
         * of type User
         */

        public sealed class GetUserBySessionId {
           
           private GetUserBySessionId() {
           }

           public interface If : BaseCallback.If, SuccessWithResultCallback.If<User.If> {
           }

        }


        /**
         * Intentionally undocumented
         */

        public sealed class GetUserBySessionToken {

           private GetUserBySessionToken() {
           }

           public interface If : BaseCallback.If, SuccessWithResultCallback.If<User.If> {
           }
        }

        /**
         * Callback for the login request. Success callback has a result of type User.
         * Status codes are not documented and are self explanatory.
         */

        public sealed class Login {

           private Login() {
           }

           public static readonly int STATUS_MISSING_EMAIL_OR_PASSWORD = 1;
           public static readonly int STATUS_ACCOUNT_LOCKED_EXCESSIVE_FAILED_ATTEMPTS = 2;
           public static readonly int STATUS_ACCOUNT_WILL_BE_LOCKED_OUT = 3;
           public static readonly int STATUS_ACCOUNT_NOT_YET_ACTIVATED = 4;
           public static readonly int STATUS_UNKNOWN_USER = 5;
           public static readonly int STATUS_LOGIN_FAILED = 6;

           public interface If : BaseCallback.If, SuccessWithResultCallback.If<User.If> {
           }

        }

         public sealed class Destroy {

            private Destroy() {
            }

            public static readonly int STATUS_ALREADY_DESTROYED = 1;
            public static readonly int DESTROY_IN_PROGRESS = 2;

            public interface If {
               void onSuccess(object closure);
               void onFailure(object closure, int status);
            }
         }
      }

      private static readonly string TAG = Util.getLogTag(typeof(SDKLib.VR));
      private static APIClient.If sAPIClient;

      private static readonly object sLock = new object();

      private class InitCallback : APIClient.Result.Init.If {

         private Result.Init.If mOriginalCallback;

         public InitCallback(Result.Init.If originalCallback) {
            mOriginalCallback = originalCallback;
         }

         public void onSuccess(object closure, APIClient.If apiClient) {
            lock (VR.sLock) {
               VR.sAPIClient = apiClient;
               VR.sInitCallback = null;
               if (null != mOriginalCallback) {
                  mOriginalCallback.onSuccess(closure);
               }
            }
         }

         public void onFailure(object closure, int status) {
            lock (VR.sLock) {
               VR.sInitCallback = null;
               if (null != mOriginalCallback) {
                  mOriginalCallback.onFailure(closure, status);
               }
            }
         }
      }

      private static InitCallback sInitCallback;

      public static bool initAsync(string endPoint, string apiKey, HttpPlugin.RequestFactory factory,
            Result.Init.If callback, SynchronizationContext handler, object closure) {
         lock (sLock) {
            if (null == sAPIClient && null == sInitCallback) {
               APIClient.Result.Init.If temp = new InitCallback(callback);
               sInitCallback = new InitCallback(callback);
               new APIClientImpl(endPoint, apiKey, factory, sInitCallback, handler, closure);
               return true;
            }
            return false;
         }
      }

      private class DestroyCallback : APIClient.Result.Destroy.If {

         private VR.Result.Destroy.If mOriginalCallback;

         public DestroyCallback(Result.Destroy.If originalCallback) {
            mOriginalCallback = originalCallback;
         }

         public void onSuccess(object closure) {
            lock (VR.sLock) {
               VR.sAPIClient = null;
               VR.sDestroyCallback = null;
               if (null != mOriginalCallback) {
                  mOriginalCallback.onSuccess(closure);
               }
            }
         }

         public void onFailure(object closure, int status) {
            lock (VR.sLock) {
               VR.sDestroyCallback = null;
               if (null != mOriginalCallback) {
                  mOriginalCallback.onFailure(closure, status);
               }
            }
         }
      }

      private static DestroyCallback sDestroyCallback;

      public static bool destroyAsync(VR.Result.Destroy.If callback, SynchronizationContext handler, object closure) {
         lock (sLock) {
            if (null != sInitCallback || null != sDestroyCallback) {
               /* Pending either init or destroy */
               return false;
            }
            DestroyCallback temp = new DestroyCallback(callback);
            if (null == sAPIClient) {
               /* Was never init */
               sDestroyCallback = temp;
               new Util.SuccessCallbackNotifier(sDestroyCallback, handler, closure).post();
               return true;
            }
            if (sAPIClient.destroy(temp, handler, closure)) {
               sDestroyCallback = temp;
               return true;
            }
            return false;
         }
      }

      public static bool login(string email, string password, VR.Result.Login.If callback, SynchronizationContext handler, object closure) {
         lock (sLock) {
            if (null == sAPIClient) {
               return false;
            }
            return sAPIClient.login(email, password, callback, handler, closure);
         }
      }
   }
}