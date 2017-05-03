using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace UILib {

   public static class UILib {

      internal class Runnable {

         public virtual void run() {
         }

         internal void postSelf(SynchronizationContext handler) {
            handler.Post(UILib.execRunnable, this);
         }
      }


      internal class LoginSuccessNotifier : CallbackNotifier {

         private SDKLib.User.If mUser;

         public LoginSuccessNotifier(UILibImpl uiLibImpl, SDKLib.User.If user)
            : base(uiLibImpl) {
            mUser = user;
         }

         protected override void onRun(UILib.Callback callback, object closure) {
            callback.onLoginSuccess(mUser, closure);
         }
      }

      internal class UILibImpl {

         internal readonly object mLock = new object();

         internal int mId;
         internal object mClosure;
         internal UILib.Callback mCallback;
         internal SynchronizationContext mHandler;
         internal String mServerApiKey, mServerEndPoint;
         internal SDKLib.HttpPlugin.RequestFactory mHttpPlugin;
         internal FormLogin mFormLogin;
         internal bool mVRLIbInitialized = false;
         internal FormMain mFormMain = null;

         internal void onLoginSuccessInternal(SDKLib.User.If user, bool save) {
            if (save) {
               UILibSettings.Default.userId = user.getUserId();
               UILibSettings.Default.userSessionToken = user.getSessionToken();
               UILibSettings.Default.Save();
            }
            new LoginSuccessNotifier(this, user).postSelf(mHandler);
         }
      }

      internal abstract class CallbackNotifier : Runnable {

         protected readonly int mMyId;
         protected readonly UILibImpl mUILibImpl;

         protected CallbackNotifier(UILibImpl uiLibImpl) {
            mMyId = uiLibImpl.mId;
            mUILibImpl = uiLibImpl;
         }

         public override void run() {

            int activeId;
            object closure;
            UILib.Callback callback;

            lock (mUILibImpl.mLock) {
               activeId = mUILibImpl.mId;
               closure = mUILibImpl.mClosure;
               callback = mUILibImpl.mCallback;
            }
            if (mMyId != activeId || null == callback) {
               return;
            }
            onRun(callback, closure);
         }

         protected abstract void onRun(UILib.Callback callback, object closure);
      }


      internal static bool DEBUG = true;
      internal static readonly string TAG = SDKLib.Log.getLogTag(typeof(UILib));

      public interface Callback {
         void onLibInitStatus(object closure, bool status);
         void onLibDestroyStatus(object closure, bool status);

         void onLoginSuccess(SDKLib.User.If user, object closure);
         void onLoginFailure(object closure);

         void showLoginUI(UserControl loginUI, object closure);
      }

      internal static readonly object sLock = new object();
      internal static UILibImpl sUILib = null;

      internal static void execRunnable(object args) {
         ((Runnable)args).run();
      }

      internal static SynchronizationContext sMainHandler = null;

      internal class InitStatusNotifier : CallbackNotifier {

         private bool mMySuccess;

         public InitStatusNotifier(UILibImpl uiLibImpl, bool success)
            : base(uiLibImpl) {
            mMySuccess = success;
         }

         protected override void onRun(UILib.Callback callback, object closure) {
            callback.onLibInitStatus(closure, mMySuccess);
         }
      }

      internal class VRInitCallback : SDKLib.VR.Result.Init.If {

         public void onSuccess(object closure) {
            UILibImpl impl = UILib.sUILib;
            impl.mVRLIbInitialized = true;
            new InitStatusNotifier(impl, true).postSelf(impl.mHandler);
         }

         public void onFailure(object closure, int status) {
            UILibImpl impl = UILib.sUILib;
            new InitStatusNotifier(impl, false).postSelf(impl.mHandler);
         }
      }

      internal class InitRunnable : Runnable, SDKLib.VR.Result.Destroy.If {

         private readonly string mServerEndPoint, mServerApiKey, mSSOAppId, mSSOAppSecret;
         private readonly UILib.Callback mCallback;
         private readonly SynchronizationContext mHandler, mUIHandler;
         private readonly object mClosure;
         private readonly SDKLib.HttpPlugin.RequestFactory mHttpPlugin;

         public InitRunnable(string serverEndPoint, string serverApiKey, string ssoAppId, string ssoAppSecret,
            SDKLib.HttpPlugin.RequestFactory httpPlugin, UILib.Callback callback, SynchronizationContext handler,
            object closure) {

            mClosure = closure;
            mHandler = handler;
            mCallback = callback;
            mServerEndPoint = serverEndPoint;
            mServerApiKey = serverApiKey;
            mSSOAppId = ssoAppId;
            mSSOAppSecret = ssoAppSecret;
            mHttpPlugin = httpPlugin;
         }

         public override void run() {
            lock (UILib.sLock) {
               if (null == UILib.sUILib) {
                  UILib.sUILib = new UILibImpl();
               }
            }

            UILibImpl impl = UILib.sUILib;

            lock (impl.mLock) {
               impl.mId += 1;
               impl.mCallback = mCallback;
               impl.mHandler = null == mHandler ? UILib.sMainHandler : mHandler;
               impl.mClosure = mClosure;
               impl.mHttpPlugin = null == mHttpPlugin ? new SDKLib.HttpPlugin.RequestFactoryImpl() : mHttpPlugin;
            }

            String currentSSOAppSecret = null, currentSSOAppId = null, currentServerEndPoint = null, currentServerApiKey = null;
            
            if (null != impl.mFormLogin) {
               currentSSOAppId = impl.mFormLogin.mSSOAppId;
               currentSSOAppSecret = impl.mFormLogin.mSSOAppSecret;
               currentServerApiKey = impl.mServerApiKey;
               currentServerEndPoint = impl.mServerEndPoint;
            }

            bool matches = (currentSSOAppSecret == mSSOAppSecret || null != currentSSOAppSecret && currentSSOAppSecret.Equals(mSSOAppSecret)) &&
                              ((currentSSOAppId == mSSOAppId) || null != currentSSOAppId && currentSSOAppId.Equals(mSSOAppId)) &&
                              ((currentServerEndPoint == mServerEndPoint) || null != currentServerEndPoint && currentServerEndPoint.Equals(mServerEndPoint)) &&
                              ((currentServerApiKey == mServerApiKey) || null != currentServerApiKey && currentServerApiKey.Equals(mServerApiKey));

            if (impl.mVRLIbInitialized) {
               if (matches) {
                  new InitStatusNotifier(impl, true).postSelf(impl.mHandler);
                  return;
               }
               SDKLib.VR.destroyAsync(this, UILib.sMainHandler, null);
               return;
            }

            onSuccess(null);
         }

         public void onFailure(object closure, int status) {
            new InitStatusNotifier(UILib.sUILib, false).postSelf(mHandler);
         }

         public void onSuccess(object closure) {
            UILibImpl impl = UILib.sUILib;

            impl.mServerApiKey = mServerApiKey;
            impl.mServerEndPoint = mServerEndPoint;
            impl.mFormLogin = new FormLogin(impl, mSSOAppId, mSSOAppSecret);
            if (!SDKLib.VR.initAsync(impl.mServerEndPoint, impl.mServerApiKey, impl.mHttpPlugin, new VRInitCallback(), sMainHandler, mClosure)) {
               new InitStatusNotifier(impl, false).postSelf(impl.mHandler);
            }
         }
      }

      public static bool init(SynchronizationContext uiHandler, string serverEndPoint, string serverApiKey, string ssoAppId,
            string ssoAppSecret, SDKLib.HttpPlugin.RequestFactory httpPlugin, Callback callback, SynchronizationContext handler,
            object closure) {

         lock (sLock) {
            sMainHandler = uiHandler;
            if (null == sMainHandler) {
               return false;
            }
            if (null == sUILib) {
               sUILib = new UILibImpl();
            }
         }
         new InitRunnable(serverEndPoint, serverApiKey, ssoAppId, ssoAppSecret, httpPlugin, 
            callback, handler, closure).postSelf(sMainHandler);
         return true;
      }


      internal class DestroyStatusNotifier : CallbackNotifier {

         private readonly bool mMySuccess;

         public DestroyStatusNotifier(UILibImpl uiLibImpl, bool success) : base(uiLibImpl) {
            mMySuccess = success;
         }

         protected override void onRun(UILib.Callback callback, object closure) {
               callback.onLibDestroyStatus(closure, mMySuccess);
         }
      }

      internal class DestroyRunnable : Runnable, SDKLib.VR.Result.Destroy.If {

         public override void run() {
            if (!SDKLib.VR.destroyAsync(this, SynchronizationContext.Current, null)) {
               new DestroyStatusNotifier(UILib.sUILib, false).postSelf(UILib.sUILib.mHandler);
            }
         }

         public void onSuccess(object closure) {
            new DestroyStatusNotifier(UILib.sUILib, true).postSelf(UILib.sUILib.mHandler);
            lock (sLock) {
               sMainHandler = null;
               sUILib = null;
            }
         }

         public void onFailure(object closure, int status) {
            new DestroyStatusNotifier(UILib.sUILib, false).postSelf(UILib.sUILib.mHandler);
         }
      }


      public static bool destroy() {
         lock (sLock) {
            if (null == sUILib || null == sMainHandler) {
               return false;
            }
            new DestroyRunnable().postSelf(sMainHandler);
            return true;
         }
      }

      internal class ShowLoginUINotifier : CallbackNotifier {

         public ShowLoginUINotifier(UILibImpl uiLibImpl) : base(uiLibImpl) {
         }

         protected override void onRun(UILib.Callback callback, object closure) {
            callback.showLoginUI(mUILibImpl.mFormLogin, closure);
         }
      }

      internal class ShowLoginRunnable : Runnable {

         private bool mShowAsWindow;

         public ShowLoginRunnable(bool showAsWindow) {
            mShowAsWindow = showAsWindow;
         }

         public override void run() {
            UILibImpl impl = sUILib;
            if (mShowAsWindow) {
               if (null == impl.mFormLogin) {
                  return;
               }
               if (null == impl.mFormMain) {
                  impl.mFormMain = new FormMain();
                  impl.mFormMain.setControl(impl.mFormLogin);
                  impl.mFormMain.Show();
               }
            } else {
               sUILib.mFormLogin.toLoginPage();
               new ShowLoginUINotifier(impl).postSelf(impl.mHandler);
            }

         }
      }

      internal class LoginRunnable : Runnable, SDKLib.VR.Result.GetUserBySessionToken.If {

         public override void run() {
            UILibImpl impl = sUILib;
            string userId = UILibSettings.Default.userId;
            string userSessionToken = UILibSettings.Default.userSessionToken;
            if (!String.IsNullOrEmpty(userId) && !String.IsNullOrEmpty(userSessionToken) && 
                  SDKLib.VR.getUserBySessionToken(userId, userSessionToken, this, sMainHandler, null)) {
               return;
            }
            showLoginUI();
         }

         private void showLoginUI() {
            UILibImpl impl = sUILib;
            impl.mFormLogin.toLoginPage();
            new ShowLoginUINotifier(impl).postSelf(impl.mHandler);
         }

         public void onFailure(object closure, int status) {
            showLoginUI();
         }

         public void onSuccess(object closure, SDKLib.User.If user) {
            sUILib.onLoginSuccessInternal(user, false);
         }

         public void onCancelled(object closure) {
         }

         public void onException(object closure, Exception ex) {
            showLoginUI();
         }
      }

      public static bool login() {
         lock (sLock) {
            if (null == sUILib || null == sMainHandler) {
               return false;
            }
            new LoginRunnable().postSelf(sMainHandler);
            return true;
         }
      }

      internal class LogoutRunnable : Runnable {

         public override void run() {
            UILibSettings.Default.userId = null;
            UILibSettings.Default.userSessionToken = null;
            UILibSettings.Default.Save();
         }
      }


      public static bool logout() {
         lock (sLock) {
            if (null == sUILib || null == sMainHandler) {
               return false;
            }
            new LogoutRunnable().postSelf(sMainHandler);
            return true;
         }

      }

      internal static bool showLoginUIInWindow() {
         lock (sLock) {
            if (null == sUILib || null == sMainHandler) {
               return false;
            }
            new ShowLoginRunnable(true).postSelf(sMainHandler);
            return true;
         }
      }
   }

}



