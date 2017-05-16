using System;
using System.Windows.Forms;
using SDKLib;

namespace SampleApp {

   public partial class FormLoggedIn : FormSub {

      class CallbackUILib : UILib.UILib.Callback {

         private readonly FormLoggedIn mFormLoggedIn;

         public CallbackUILib(FormLoggedIn formLoggedIn) {
            mFormLoggedIn = formLoggedIn;
         }

         private static readonly string TAG = Util.getLogTag(typeof(CallbackUILib));

         public void onLibInitStatus(object closure, bool status) {
         }

         public void onLibDestroyStatus(object closure, bool status) {
         }

         public void onLogoutSuccess(User.If user, object closure) {
            App app = App.getInstance();
            app.getFormMain().setControl(new FormUILibLogin());

         }

         public void onLogoutFailure(User.If user, object closure) {
         }

         public void onLoginSuccess(User.If user, object closure) {
         }

         public void onLoginFailure(object closure) {
         }

         public void showLoginUI(UserControl loginUI, object closure) {
         }
      }

      private readonly CallbackUILib mCallbackUILib;

      public FormLoggedIn() {
         InitializeComponent();
         mCallbackUILib = new CallbackUILib(this);
      }

      private FormBase mControl;

      internal void setControl(FormBase control) {
         if (mControl == control) {
            return;
         }
         if (null != mControl) {
            mControl.onUnload();
            ctrlCurrentAction.Controls.Remove(mControl);
         }
         mControl = control;
         if (null != mControl) {
            ctrlCurrentAction.Controls.Add(mControl);
            mControl.onLoad();
            centerControl();
         }
      }

      private void centerControl() {
         if (null != mControl) {
            mControl.Left = (ctrlCurrentAction.Width - mControl.Width) / 2;
            mControl.Top = (ctrlCurrentAction.Height - mControl.Height) / 2;
         }
      }

      private void ctrlUploadVideo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) {
         if (mControl is FormUploadVideo) {
            return;
         }
         setControl(new FormUploadVideo());
      }

      private void ctrlCreateLiveEvent_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) {
         if (mControl is FormCreateLiveEvent) {
            return;
         }
         setControl(new FormCreateLiveEvent());
      }

      private void ctrlListLiveEvents_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) {
         if (mControl is FormListLiveEvents) {
            return;
         }
         setControl(new FormListLiveEvents());

      }

      private void ctrlManageLogs_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) {
         if (mControl is FormManageLogs) {
            return;
         }
         setControl(new FormManageLogs());
      }

      private void ctrlLogout_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) {
         
         if (App.USE_UILIB) {
            if (UILib.UILib.logout()) {
               return;
            }
         } else {
            User.If user = App.getInstance().getUser();
            if (null != user) {
               user.logout(null, App.getInstance().getHandler(), null);
            }
         }
         
         App.getInstance().showLoginForm();
      }

      public override void onAdded() {
         base.onAdded();
         App.getInstance().mUILibCallback.mSubCallbacks.Add(mCallbackUILib);
         User.If user = App.getInstance().getUser();
         if (null != user) {
            ctrlUsername.Text = user.getName();
            ctrlEmail.Text = user.getEmail();
            ctrlProfilePic.ImageLocation = user.getProfilePicUrl();
         }
      }


      public override void onRemoved() {
         base.onRemoved();
         App.getInstance().mUILibCallback.mSubCallbacks.Remove(mCallbackUILib);
      }

      private void panel2_Paint(object sender, PaintEventArgs e) {

      }
   }
}
