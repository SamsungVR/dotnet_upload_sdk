using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using SDKLib;
using System.IO;

namespace SampleApp {

   public partial class FormUploadVideo : UserControl, User.Result.UploadVideo.If {

      public FormUploadVideo() {
         InitializeComponent();
      }

      private void ctrlSelectFile_Click(object sender, EventArgs e) {
         DialogResult result = ctrlVideoChooser.ShowDialog();
         if (DialogResult.OK == result) {
            ctrlSelectedFile.Text = ctrlVideoChooser.FileName;
         } else {
            ctrlSelectedFile.Text = string.Empty;
         }
      }

      private Stream mSource;
      private long mLength;


      private void ctrlUpload_Click(object sender, EventArgs e) {
         if (null != mSource) {
            return;
         }
         string strPermission = ctrlPermission.SelectedText;
         UserVideo.Permission permission = UserVideo.fromString(strPermission);
         string title = ctrlTitle.Text;
         if (null == title || 0 == title.Trim().Length) {
            ctrlUploadStatus.Text = ResourceStrings.uploadTitleInvalid;
            return;
         }
         string description = ctrlDescription.Text;
         if (null == description || 0 == description.Trim().Length) {
            ctrlUploadStatus.Text = ResourceStrings.uploadDescriptionInvalid;
            return;
         }

         string fileName = ctrlSelectedFile.Text;
         FileInfo fileInfo = new FileInfo(fileName);
         if (!fileInfo.Exists) {
            ctrlUploadStatus.Text = ResourceStrings.uploadFileDoesNotExist;
            return;
         }
         FileStream stream = null;
         try {
            stream = fileInfo.OpenRead();
         } catch (Exception) {
            ctrlUploadStatus.Text = ResourceStrings.uploadFileOpenFailed;
            return;
         }
         long length = fileInfo.Length;
         
         User.If user = App.getInstance().getUser();
         if (user.uploadVideo(stream, length, title, description, permission, this, App.getInstance().getHandler(), this)) {
            mSource = stream;
            mLength = length;
            onBeginUpload();
         }
      }

      private UserVideo.If mVideo;

      public void onVideoIdAvailable(object closure, UserVideo.If video) {
         mVideo = video;
      }

      public void onCancelled(object closure) {
         onUploadComplete();
         ctrlUploadStatus.Text = ResourceStrings.uploadCancelled;
      }

      private void onBeginUpload() {
         ctrlUpload.Enabled = false;
         ctrlCancel.Enabled = true;
         ctrlRetry.Enabled = false;
         ctrlUploadStatus.Text = ResourceStrings.uploadInProgress;
      }

      private void onUploadComplete() {
         mVideo = null;
         if (null != mSource) {
            mSource.Close();
            mSource = null;
         }
         ctrlUpload.Enabled = true;
         ctrlCancel.Enabled = false;
         ctrlRetry.Enabled = false;
         ctrlProgressBar.Value = 0;
         ctrlRawProgress.Text = string.Empty;
      }

      private void setupForRetry() {
         if (null == mVideo) {
            onUploadComplete();
            return;
         }
         ctrlProgressBar.Value = 0;
         ctrlRawProgress.Text = string.Empty;
         ctrlRetry.Enabled = true;
      }

      public void onException(object closure, Exception ex) {
         setupForRetry();
         ctrlUploadStatus.Text = ex.Message;
         Console.WriteLine(ex.Message);
         Console.WriteLine(ex.StackTrace);
      }

      public void onFailure(object closure, int status) {
         setupForRetry();
         ctrlUploadStatus.Text = string.Format(ResourceStrings.uploadFailedWithStatus, status);
      }

      public void onProgress(object closure, float progressPercent, long complete, long max) {
         ctrlRawProgress.Text = progressPercent.ToString();
         ctrlProgressBar.Value = (int)progressPercent;
      }

      public void onProgress(object closure, long complete) {
         ctrlRawProgress.Text = complete.ToString();
      }

      public void onSuccess(object closure) {
         onUploadComplete();
         ctrlUploadStatus.Text = ResourceStrings.uploadSuccess;
      }

      private void FormUploadVideo_Load(object sender, EventArgs e) {
         onUploadComplete();
      }

      private void ctrlCancel_Click(object sender, EventArgs e) {
         bool result = false;
         if (null != mVideo) {
            result = mVideo.cancelUpload(this);
         } else {
            User.If user = App.getInstance().getUser();
            result = user.cancelUploadVideo(this);
         }
         if (!result) {
            onUploadComplete();
            ctrlUploadStatus.Text = ResourceStrings.uploadCancelled;
         }
      }

      private void ctrlUploadStatus_Click(object sender, EventArgs e) {
         ctrlUploadStatus.Text = string.Empty;
      }

      private void ctrlRetry_Click(object sender, EventArgs e) {
         if (null == mVideo || null == mSource) {
            return;
         }
         if (!mVideo.retryUpload(mSource, mLength, this, App.getInstance().getHandler(), this)) {
            ctrlUploadStatus.Text = ResourceStrings.uploadRetryFailed;
         } else {
            onBeginUpload();
         }
      }

   }
}
