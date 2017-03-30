using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;

namespace SDKLib {

   internal class APIClientImpl : Container.BaseImpl<APIClient.Observer>, APIClient.If {

      internal static readonly string HEADER_API_KEY = "X-API-KEY";

      private readonly string mEndPoint, mApiKey;

      private static string TAG = Util.getLogTag(typeof(APIClient));

      private ClientWorkItem asyncWorkItemFactory(AsyncWorkItemType type) {
         return (ClientWorkItem)type.newInstance(this);
      }

      private const int QUEUE_COUNT = 2;

      private void onAsyncQueueStateChanged(AsyncWorkQueue queue, System.Enum oldState, System.Enum newState) {


         switch ((AsyncWorkQueue.State)newState) {

            case AsyncWorkQueue.State.INITIALIZED:
               mInitializedQueueCount += 1;
               if (mInitializedQueueCount >= QUEUE_COUNT) {
                  mStateManager.setState(State.INITIALIZED);
                  if (null != mInitCallbackHolder) {
                     mInitCallbackHolder.post();
                  }
               }
               break;

            case AsyncWorkQueue.State.DESTROYED:
               queue.getStateManager().removeAllObservers(onAsyncQueueStateChanged);
               mInitializedQueueCount -= 1;
               if (mInitializedQueueCount <= 0) {
                  mStateManager.setState(State.DESTROYED);
                  if (null != mDestroyCallbackHolder) {
                     new Util.SuccessCallbackNotifier(mDestroyCallbackHolder).post();
                  }
               }
               break;
         }

         Log.d(TAG, "onAsyncQueueStateChanged queue: " + queue + " oldState: " + oldState + " newState: " + newState + " mInitializedQueueCount " + mInitializedQueueCount);
      }

      private int mInitializedQueueCount = 0;

      private readonly AsyncWorkQueue mAsyncWorkQueue, mAsyncUploadQueue;

      public enum State {
         INITIALIZING,
         INITIALIZED,
         DESTROYING,
         DESTROYED
      }

      private readonly StateManager<APIClientImpl> mStateManager;

      private readonly HttpPlugin.RequestFactory mHttpRequestFactory;
      private Util.CallbackNotifier mInitCallbackHolder;
      private ResultCallbackHolder mDestroyCallbackHolder;

      private static readonly int MUL = 1024;


      public APIClientImpl(string endPoint, string apiKey, HttpPlugin.RequestFactory httpRequestFactory,
         APIClient.Result.Init.If callback, SynchronizationContext handler, object closure)
         : base(true) {

         mStateManager = new StateManager<APIClientImpl>(this, State.INITIALIZING);
         mInitCallbackHolder = new Util.SuccessWithResultCallbackNotifier<APIClient.If>(callback, handler, closure, this);

         mEndPoint = endPoint;
         mApiKey = apiKey;
         mHttpRequestFactory = httpRequestFactory;
         mAsyncWorkQueue = new AsyncWorkQueue(asyncWorkItemFactory, 8 * MUL, Timeout.Infinite, onAsyncQueueStateChanged, handler);
         mAsyncUploadQueue = new AsyncWorkQueue(asyncWorkItemFactory, 1024 * MUL, Timeout.Infinite, onAsyncQueueStateChanged, handler);
      }

      internal HttpPlugin.RequestFactory getRequestFactory() {
         return mHttpRequestFactory;
      }

      public string getEndPoint() {
         return mEndPoint;
      }

      public string getApiKey() {
         return mApiKey;
      }

      public StateManager<APIClientImpl> getStateManager() {
         return mStateManager;
      }

      public bool destroy(APIClient.Result.Destroy.If callback, SynchronizationContext handler, object closure) {
         if (!mStateManager.isInState(State.INITIALIZED)) {
            return false;
         }
         mStateManager.setState(State.DESTROYING);
         mDestroyCallbackHolder = new ResultCallbackHolder(callback, handler, closure);
         mAsyncUploadQueue.destroy();
         mAsyncWorkQueue.destroy();
         return true;
      }

      public AsyncWorkQueue getAsyncWorkQueue() {
         return mAsyncWorkQueue;
      }

      public AsyncWorkQueue getAsyncUploadQueue() {
         return mAsyncUploadQueue;
      }

      public bool login(string email, string password, VR.Result.Login.If callback, SynchronizationContext handler, object closure) {
         if (!mStateManager.isInState(State.INITIALIZED)) {
            return false;
         }
         WorkItemPerformLogin workItem = (WorkItemPerformLogin)mAsyncWorkQueue.obtainWorkItem(WorkItemPerformLogin.TYPE);
         workItem.set(email, password, callback, handler, closure);
         return mAsyncWorkQueue.enqueue(workItem);
      }

      public bool loginSamsungAccount(string samsung_sso_token, string auth_server, VR.Result.LoginSSO.If callback,
         SynchronizationContext handler, object closure) {
         if (!mStateManager.isInState(State.INITIALIZED)) {
            return false;
         }
         WorkItemPerformLoginSamsungAccount workItem = (WorkItemPerformLoginSamsungAccount)mAsyncWorkQueue.obtainWorkItem(WorkItemPerformLoginSamsungAccount.TYPE);
         workItem.set(samsung_sso_token, auth_server, callback, handler, closure);
         return mAsyncWorkQueue.enqueue(workItem);
      }

      public User getUserById(string userId) {
         if (!mStateManager.isInState(State.INITIALIZED)) {
            return null;
         }
         return null;
      }

      public bool getUserBySessionId(string sessionId, VR.Result.GetUserBySessionId.If callback, SynchronizationContext handler, object closure) {
         if (!mStateManager.isInState(State.INITIALIZED)) {
            return false;
         }
         return true;
      }

      public bool getUserBySessionToken(string sessionToken, VR.Result.GetUserBySessionToken.If callback, SynchronizationContext handler, object closure) {
         if (!mStateManager.isInState(State.INITIALIZED)) {
            return false;
         }
         return true;
      }

      public override List<U> containerOnQueryListOfContainedFromServiceLocked<U>(Contained.CType type, JObject jsonObject) {
         return null;
      }

      public override U containerOnQueryOfContainedFromServiceLocked<U>(Contained.CType type, U contained, JObject jsonObject) {
         return default(U);
      }

      public override U containerOnCreateOfContainedInServiceLocked<U>(Contained.CType type, JObject jsonObject) {
         U result = processCreateOfContainedInServiceLocked<U>(type, jsonObject, true);
         Log.d(TAG, "Add contained: " + result);
         return result;
      }

      public override U containerOnUpdateOfContainedToServiceLocked<U>(Contained.CType type, U contained) {
         return default(U);
      }

      public override U containerOnDeleteOfContainedFromServiceLocked<U>(Contained.CType type, U contained) {
         return default(U);
      }

   }

   internal class WorkItemPerformLogin : ClientWorkItem {

      private class WorkItemTypePerformLogin : AsyncWorkItemType {

         public AsyncWorkItem newInstance(APIClientImpl apiClient) {
            return new WorkItemPerformLogin(apiClient);
         }
      }

      public static readonly AsyncWorkItemType TYPE = new WorkItemTypePerformLogin();

      public WorkItemPerformLogin(APIClientImpl apiClient)
         : base(apiClient, TYPE) {
      }

      public void set(String email, String password, VR.Result.Login.If callback, SynchronizationContext handler, object closure) {
         base.set(callback, handler, closure);
         mEmail = email;
         mPassword = password;
      }

      protected override void recycle() {
         base.recycle();
         mEmail = null;
         mPassword = null;
      }

      private string mEmail, mPassword;

      protected override void onRun() {

         HttpPlugin.PostRequest request = null;

         try {
            JObject o = new JObject();
            o.Add("email", mEmail);
            o.Add("password", mPassword);

            string jsonStr = o.ToString(Newtonsoft.Json.Formatting.None);
            byte[] data = System.Text.Encoding.UTF8.GetBytes(jsonStr);

            string[,] headers = new string[,] {
                        {HEADER_CONTENT_LENGTH, data.Length.ToString()},
                        {HEADER_CONTENT_TYPE, "application/json" + ClientWorkItem.CONTENT_TYPE_CHARSET_SUFFIX_UTF8},
                        {APIClientImpl.HEADER_API_KEY, mAPIClient.getApiKey()}
                };

            request = newPostRequest("user/authenticate", headers);
            if (null == request) {
               dispatchFailure(VR.Result.STATUS_HTTP_PLUGIN_NULL_CONNECTION);
               return;
            }

            writeBytes(request, data, jsonStr);

            if (isCancelled()) {
               dispatchCancelled();
               return;
            }

            HttpStatusCode rsp = getResponseCode(request);
            String data2 = readHttpStream(request, "code: " + rsp);
            if (null == data2) {
               dispatchFailure(VR.Result.STATUS_HTTP_PLUGIN_STREAM_READ_FAILURE);
               return;
            }
            JObject jsonObject = JObject.Parse(data2);

            if (isHTTPSuccess(rsp)) {
               UserImpl user = mAPIClient.containerOnCreateOfContainedInServiceLocked<UserImpl>(UserImpl.sType, jsonObject);

               if (null != user) {
                  dispatchSuccessWithResult<User.If>(user);
               } else {
                  dispatchFailure(VR.Result.STATUS_SERVER_RESPONSE_INVALID);
               }
               return;
            }
            int status = Util.jsonOpt(jsonObject, "status", VR.Result.STATUS_SERVER_RESPONSE_NO_STATUS_CODE);
            dispatchFailure(status);
         } finally {
            destroy(request);
         }


      }
   }

   internal class WorkItemPerformLoginSamsungAccount : ClientWorkItem {

      private class WorkItemTypePerformLoginSamsungAccount : AsyncWorkItemType {

         public AsyncWorkItem newInstance(APIClientImpl apiClient) {
            return new WorkItemPerformLoginSamsungAccount(apiClient);
         }
      }

      public static readonly AsyncWorkItemType TYPE = new WorkItemTypePerformLoginSamsungAccount();

      public WorkItemPerformLoginSamsungAccount(APIClientImpl apiClient)
         : base(apiClient, TYPE) {
      }

      public void set(string samsung_sso_token, string auth_server, VR.Result.LoginSSO.If callback, SynchronizationContext handler, object closure) {
         base.set(callback, handler, closure);
         mSamsungSSOToken = samsung_sso_token;
         mAuthServer = auth_server;
      }

      protected override void recycle() {
         base.recycle();
         mAuthServer = null;
         mSamsungSSOToken = null;
      }

      private string mSamsungSSOToken, mAuthServer;

      private bool tryLogin(ObjectHolder<int> statusOut) {

         HttpPlugin.PostRequest request = null;
         try {

            JObject jsonParam = new JObject();
            jsonParam.Add("auth_type", "SamsungSSO");
            jsonParam.Add("samsung_sso_token", mSamsungSSOToken);
            if (mAuthServer != null) {
               jsonParam.Add("auth_server", mAuthServer);
            }

            string jsonStr = jsonParam.ToString(Newtonsoft.Json.Formatting.None);
            byte[] data = System.Text.Encoding.UTF8.GetBytes(jsonStr);
            string[,] headers = new string[,] {
                        {HEADER_CONTENT_LENGTH, data.Length.ToString()},
                        {HEADER_CONTENT_TYPE, "application/json" + ClientWorkItem.CONTENT_TYPE_CHARSET_SUFFIX_UTF8},
                        {APIClientImpl.HEADER_API_KEY, mAPIClient.getApiKey()}
                };
            request = newPostRequest("user/authenticate", headers);
            if (null == request) {
               dispatchFailure(VR.Result.STATUS_HTTP_PLUGIN_NULL_CONNECTION);
               return true;
            }

            writeBytes(request, data, jsonStr);

            if (isCancelled()) {
               dispatchCancelled();
               return true;
            }

            HttpStatusCode rsp = getResponseCode(request);
            String data2 = readHttpStream(request, "code: " + rsp);
            if (null == data2) {
               dispatchFailure(VR.Result.STATUS_HTTP_PLUGIN_STREAM_READ_FAILURE);
               return true;
            }
            JObject jsonObject = JObject.Parse(data2);

            if (isHTTPSuccess(rsp)) {
               UserImpl user = mAPIClient.containerOnCreateOfContainedInServiceLocked<UserImpl>(UserImpl.sType, jsonObject);

               if (null != user) {
                  dispatchSuccessWithResult<User.If>(user);
               } else {
                  dispatchFailure(VR.Result.STATUS_SERVER_RESPONSE_INVALID);
               }
               return true;
            }
            int status = Util.jsonOpt(jsonObject, "status", VR.Result.STATUS_SERVER_RESPONSE_NO_STATUS_CODE);
            if (status == VR.Result.STATUS_SERVER_RESPONSE_NO_STATUS_CODE) {
               dispatchFailure(status);
               return true;
            }
            statusOut.set(status);
            return false;

         } finally {
            destroy(request);
         }
      }

      private bool tryRegister() {

         HttpPlugin.PostRequest request = null;
         try {
            JObject jsonParam = new JObject();
            jsonParam.Add("auth_type", "SamsungSSO");
            jsonParam.Add("samsung_sso_token", mSamsungSSOToken);
            if (mAuthServer != null) {
               jsonParam.Add("auth_server", mAuthServer);
            }

            string jsonStr = jsonParam.ToString(Newtonsoft.Json.Formatting.None);
            byte[] data = System.Text.Encoding.UTF8.GetBytes(jsonStr);
            string[,] headers = new string[,] {
                        {HEADER_CONTENT_LENGTH, data.Length.ToString()},
                        {HEADER_CONTENT_TYPE, "application/json" + ClientWorkItem.CONTENT_TYPE_CHARSET_SUFFIX_UTF8},
                        {APIClientImpl.HEADER_API_KEY, mAPIClient.getApiKey()}
                };

            request = newPostRequest("user", headers);
            if (null == request) {
               dispatchFailure(VR.Result.STATUS_HTTP_PLUGIN_NULL_CONNECTION);
               return true;
            }

            writeBytes(request, data, jsonStr);

            if (isCancelled()) {
               dispatchCancelled();
               return true;
            }

            HttpStatusCode rsp = getResponseCode(request);
            String data2 = readHttpStream(request, "code: " + rsp);
            if (null == data2) {
               dispatchFailure(VR.Result.STATUS_HTTP_PLUGIN_STREAM_READ_FAILURE);
               return true;
            }
            if (isHTTPSuccess(rsp)) {
               return false;
            }
            JObject jsonObject = JObject.Parse(data2);
            int status = Util.jsonOpt(jsonObject, "status", VR.Result.STATUS_SERVER_RESPONSE_NO_STATUS_CODE);
            dispatchFailure(VR.Result.LoginSSO.CLASS_REG | status);
            return true;
         } finally {
            destroy(request);
         }
      }

      private static readonly String TAG = Util.getLogTag(typeof(WorkItemPerformLoginSamsungAccount));

      protected override void onRun() {

         ObjectHolder<int> status = new ObjectHolder<int>(0);

         if (tryLogin(status)) {
            return;
         }
         int statusInt = status.get();
         Log.d(TAG, "Login failed with status " + statusInt);
         if (11 == statusInt) {
            if (tryRegister()) {
               return;
            }
            if (tryLogin(status)) {
               return;
            }
            statusInt = status.get();
         }
         dispatchFailure(VR.Result.LoginSSO.CLASS_AUTH | statusInt);
      }
   }
}
