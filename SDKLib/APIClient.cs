using System.Threading;

namespace SDKLib {

   internal sealed class APIClient {

      private APIClient() {
      }

      internal sealed class Result {

         private Result() {
         }

         public sealed class Init {

            private Init() {
            }

            public interface If : VR.Result.SuccessWithResultCallback.If<APIClient.If> {
            }
         }

         public sealed class Destroy {

            private Destroy() {
            }

            public interface If : VR.Result.SuccessCallback.If, VR.Result.FailureCallback.If {
            }
         }
      }

      public interface If : Observable.If<APIClient.Observer> {

         string getEndPoint();
         string getApiKey();

         AsyncWorkQueue getAsyncWorkQueue();
         AsyncWorkQueue getAsyncUploadQueue();

         bool login(string email, string password, VR.Result.Login.If callback, SynchronizationContext handler, object closure);
         bool loginSamsungAccount(string samsung_sso_token, string auth_server, VR.Result.LoginSSO.If callback, SynchronizationContext handler, object closure);

         User getUserById(string userId);
         bool getUserBySessionId(string sessionId, VR.Result.GetUserBySessionId.If callback,
                                    SynchronizationContext handler, object closure);
         bool getUserBySessionToken(string sessionToken, VR.Result.GetUserBySessionToken.If callback,
                                    SynchronizationContext handler, object closure);
         bool destroy(APIClient.Result.Destroy.If callback, SynchronizationContext handler, object closure);

      }

      public interface Observer {
      }
   }
}
