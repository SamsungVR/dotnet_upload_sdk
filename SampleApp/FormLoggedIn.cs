using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using SDKLib;

namespace SampleApp {
   public partial class FormLoggedIn : UserControl {
      public FormLoggedIn() {
         InitializeComponent();
      }

      private void FormLoggedIn_Load(object sender, EventArgs e) {
         User.If user = App.getInstance().getUser();
         if (null != user) {
            ctrlUsername.Text = user.getName();
            ctrlEmail.Text = user.getEmail();
            ctrlProfilePic.ImageLocation = user.getProfilePicUrl();
         }
      }


      private Control mControl;

      public void setControl(Control control) {
         if (mControl == control) {
            return;
         }
         if (null != mControl) {
            ctrlCurrentAction.Controls.Remove(mControl);
         }
         mControl = control;
         if (null != mControl) {
            ctrlCurrentAction.Controls.Add(mControl);
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
   }
}
