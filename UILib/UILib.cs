using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace UILib {

   internal class Runnable {

      public virtual void run() {
      }

      internal void postSelf(SynchronizationContext handler) {
         handler.Post(UILib.execRunnable, this);
      }
   }

   public static class UILib {

      internal static bool DEBUG = true;
      internal static readonly string TAG = SDKLib.Log.getLogTag(typeof(UILib));

      public interface Callback {
         void onLibInitSuccess(object closure);
         void onLibInitFailed(object closure);
         void onLoggedIn(SDKLib.User user, object closure);
         void onFailure(object closure);
         void showLoginUI(UserControl loginUI, object closure);
      }

      internal static readonly object sLock = new object();
      internal static UILibImpl sUILib = null;

      internal static void execRunnable(object args) {
         ((Runnable)args).run();
      }

      internal static SynchronizationContext sMainHandler = null;

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
         sMainHandler.Post(execRunnable, new InitRunnable(uiHandler, serverEndPoint, serverApiKey, ssoAppId, ssoAppSecret,
            httpPlugin, callback, handler, closure));
         return true;
      }

      public static bool login() {
         lock (sLock) {
            if (null == sUILib || null == sMainHandler) {
               return false;
            }
            new ShowLoginRunnable(false).postSelf(sMainHandler);
            return true;
         }
      }

      public static bool showLoginUIInWindow() {
         lock (sLock) {
            if (null == sUILib || null == sMainHandler) {
               return false;
            }
            new ShowLoginRunnable(true).postSelf(sMainHandler);
            return true;
         }

      }
   }

   internal class VRInitCallback : SDKLib.VR.Result.Init.If {

      public void onSuccess(object closure) {
         UILib.sUILib.onVRLibInitSuccess();
      }

      public void onFailure(object closure, int status) {
         UILib.sUILib.onVRLibInitFailure();
      }
   }


   internal class UILibImpl {

      internal readonly object mLock = new object();

      internal int mId;
      internal object mClosure;
      internal UILib.Callback mCallback;
      internal SynchronizationContext mHandler, mUIHandler;
      internal String mServerApiKey, mServerEndPoint;
      internal SDKLib.HttpPlugin.RequestFactory mHttpPlugin;
      internal FormLogin mFormLogin;

      public void initInternal(SynchronizationContext uiHandler, string serverEndPoint, string serverApiKey, string ssoAppId,
         string ssoAppSecret, SDKLib.HttpPlugin.RequestFactory httpPlugin, UILib.Callback callback, SynchronizationContext handler,
         object closure) {

         mUIHandler = uiHandler;
         lock (mLock) {
            mId += 1;
            mCallback = callback;
            mHandler = null == handler ? uiHandler : handler;
            mHttpPlugin = null == httpPlugin ? new SDKLib.HttpPlugin.RequestFactoryImpl() : httpPlugin;
         }

         String currentSSOAppSecret = null, currentSSOAppId = null;
         if (null != mFormLogin) {
            currentSSOAppId = mFormLogin.mSSOAppId;
            currentSSOAppSecret = mFormLogin.mSSOAppSecret;
         }

         bool matches = (currentSSOAppSecret == ssoAppSecret || null != currentSSOAppSecret && currentSSOAppSecret.Equals(ssoAppSecret)) &&
                           ((currentSSOAppId == ssoAppId) || null != currentSSOAppId && currentSSOAppId.Equals(ssoAppId)) &&
                           ((mServerEndPoint == serverEndPoint) || null != mServerEndPoint && mServerEndPoint.Equals(serverEndPoint)) &&
                           ((mServerApiKey == serverApiKey) || null != mServerApiKey && mServerApiKey.Equals(serverApiKey));

         if (matches) {
            new InitStatusNotifier(this, mId, true).postSelf(mHandler);
            return;
         }
         mServerApiKey = serverApiKey;
         mServerEndPoint = serverEndPoint;
         mFormLogin = new FormLogin(ssoAppId, ssoAppSecret);
         if (!SDKLib.VR.initAsync(serverEndPoint, serverApiKey, mHttpPlugin, new VRInitCallback(), mUIHandler, mClosure)) {
            new InitStatusNotifier(this, mId, false).postSelf(mHandler);
         }
      }

      public bool mVRLIbInitialized = false;

      public void onVRLibInitSuccess() {
         mVRLIbInitialized = true;
         new InitStatusNotifier(this, mId, true).postSelf(mHandler);
      }

      public void onVRLibInitFailure() {
         new InitStatusNotifier(this, mId, false).postSelf(mHandler);
      }

      public void loginInternal() {
         new ShowLoginUINotifier(this, mId, mFormLogin).postSelf(mHandler);
      }

      public FormMain mFormMain = null;

      public void showLoginUIAsFormInternal() {
         if (null == mFormLogin) {
            return;
         }
         if (null == mFormMain) {
            mFormMain = new FormMain();
            mFormMain.setControl(mFormLogin);
            mFormMain.Show();
         }
      }

      public void dispatchLoginAsUserControlInternal() {

      }

   }

   internal class InitRunnable : Runnable, SDKLib.VR.Result.Destroy.If {

      private readonly string mServerEndPoint, mServerApiKey, mSSOAppId, mSSOAppSecret;
      private readonly UILib.Callback mCallback;
      private readonly SynchronizationContext mHandler, mUIHandler;
      private readonly object mClosure;
      private readonly SDKLib.HttpPlugin.RequestFactory mHttpPlugin;

      public InitRunnable(SynchronizationContext uiHandler, string serverEndPoint, string serverApiKey, string ssoAppId, string ssoAppSecret,
         SDKLib.HttpPlugin.RequestFactory httpPlugin, UILib.Callback callback, SynchronizationContext handler,
         object closure) {

         mClosure = closure;
         mHandler = handler;
         mUIHandler = uiHandler;
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
         if (UILib.sUILib.mVRLIbInitialized) {
            SDKLib.VR.destroyAsync(this, UILib.sMainHandler, null);
            return;
         }
         onSuccess(null);
      }

      public void onSuccess(object closure) {
         UILib.sUILib.initInternal(mUIHandler, mServerEndPoint, mServerApiKey, mSSOAppId,
            mSSOAppSecret, mHttpPlugin, mCallback, mHandler, mClosure);
      }

      public void onFailure(object closure, int status) {
         new InitStatusNotifier(UILib.sUILib, UILib.sUILib.mId, false).postSelf(mHandler);
      }

   }

   internal class LoginRunnable : Runnable {

      public override void run() {
         UILib.sUILib.loginInternal();
      }
   }

   internal class ShowLoginRunnable : Runnable {

      private bool mShowAsWindow;

      public ShowLoginRunnable(bool showAsWindow) {
         mShowAsWindow = showAsWindow;
      }

      public override void run() {
         if (mShowAsWindow) {
            UILib.sUILib.showLoginUIAsFormInternal();
         } else {
            UILib.sUILib.loginInternal();
         }
         
      }
   }

   internal abstract class CallbackNotifier : Runnable {

      protected readonly int mMyId;
      protected readonly UILibImpl mUILibImpl;

      protected CallbackNotifier(int id, UILibImpl uiLibImpl) {
         mMyId = id;
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

   internal class InitStatusNotifier : CallbackNotifier {

      private bool mMySuccess;

      public InitStatusNotifier(UILibImpl uiLibImpl, int id, bool success)
         : base(id, uiLibImpl) {
         mMySuccess = success;
      }

      protected override void onRun(UILib.Callback callback, object closure) {
         if (mMySuccess) {
            callback.onLibInitSuccess(closure);
         } else {
            callback.onLibInitFailed(closure);
         }
      }
   }

   internal class ShowLoginUINotifier : CallbackNotifier {

      private readonly UserControl mUserControl;

      public ShowLoginUINotifier(UILibImpl uiLibImpl, int id, UserControl userControl)
         : base(id, uiLibImpl) {
         mUserControl = userControl;
      }

      protected override void onRun(UILib.Callback callback, object closure) {
         callback.showLoginUI(mUserControl, closure);
      }
   }
}


#if adsf
      internal class Impl {

         private FormMain mFormMain;

         private int mId = -1;
         
         private Callback.If mCallback;
         private object mClosure;
         private SynchronizationContext mHandler, mUIHandler;

         public Impl() {
         }



         public bool loginInternal() {
            if (null != mFormMain) {
               return false;
            }
            return true;
         }

         public void onFormClosed(FormClosedEventArgs e) {
            mFormMain = null;
         }

      }

      public static bool login() {
         if (null == sInstance) {
            sInstance = new Impl();
         }
         return sInstance.loginInternal();
      }
   }
#endif

