using System;
using System.Collections.Generic;
using System.Windows.Forms;
using SDKLib;
using System.IO;
using Newtonsoft.Json.Linq;

namespace SampleApp {

   public partial class FormUploadVideo : FormBase, UploadVideoManager.Callback {

      private readonly UploadVideoManager mUploadVideoManager;

      public FormUploadVideo() {
         InitializeComponent();
         mUploadVideoManager = App.getInstance().getUploadVideoManager();
      }

      private void ctrlSelectFile_Click(object sender, EventArgs e) {
         DialogResult result = ctrlVideoChooser.ShowDialog();
         if (DialogResult.OK == result) {
            ctrlSelectedFile.Text = ctrlVideoChooser.FileName;
         } else {
            ctrlSelectedFile.Text = string.Empty;
         }
      }

      public void onBeginUpload() {
         onBeginUploadInternal();
         ctrlUploadDashboard.SelectTab(1);
      }

      public void onUploadProgress(float progressPercent, long complete, long max) {
         if (-1 != progressPercent) {
            ctrlUploadProgressVisual.Value = (int)progressPercent;
         } else {
            ctrlUploadProgressVisual.Style = ProgressBarStyle.Marquee;
         }
         if (-1 != max) {
            ctrlUploadProgressRaw.Text = complete + " / " + max;
         } else {
            ctrlUploadProgressRaw.Text = complete + "";
         }
      }

      public void onPendingItemsChanged() {
         onPendingItemsChangedInternal();
         ctrlUploadDashboard.SelectTab(0);
      }

      public void onFailedItemsChanged() {
         onFailedItemsChangedInternal();
         ctrlUploadDashboard.SelectTab(3);
      }

      private void onPendingItemsChangedInternal() {
         UploadVideoManager.PendingUploadItemsModel model = mUploadVideoManager.getPendingUploadsModel();
         populateList(ctrlPendingList, model.mItems);
      }

      private void populateList<T>(ListBox list, List<T> items) where T : UploadVideoManager.UploadItem {
         int selectedIndex = list.SelectedIndex;
         list.Items.Clear();
         foreach (T item in items) {
            list.Items.Add(item.getAsString());
         }
         int count = list.Items.Count;
         if (count > 0) {
            if (selectedIndex < 0) {
               selectedIndex = 0;
            } else if (selectedIndex >= count) {
               selectedIndex = count - 1;
            }
            if (list.SelectionMode == SelectionMode.One) {
               list.SelectedIndex = selectedIndex;
            }
         }
      }

      private void onFailedItemsChangedInternal() {
         UploadVideoManager.FailedUploadItemsModel model = mUploadVideoManager.getFailedUploadsModel();
         populateList(ctrlFailedList, model.mItems);
      }

      private void onBeginUploadInternal() {
         UploadVideoManager.ActiveUploadItem item = mUploadVideoManager.getActiveUpload();
         ctrlInProgressDetails.Items.Clear();
         if (null == item) {
            ctrlInProgressDetails.Items.Add(ResourceStrings.noActiveUpload);
            return;
         }
         ctrlInProgressDetails.Items.Add(item.getTitle());
         ctrlInProgressDetails.Items.Add(item.getDescription());
         ctrlInProgressDetails.Items.Add(item.getFilename());
         ctrlInProgressDetails.Items.Add(item.getPermission());
      }

      public override void onLoad() {
         
         mUploadVideoManager.addCallback(this);
         onBeginUploadInternal();
         onPendingItemsChangedInternal();
         onFailedItemsChangedInternal();
      }

      public override void onUnload() {
         mUploadVideoManager.removeCallback(this);
      }

      private void ctrlEnqueue_Click(object sender, EventArgs e) {
         String permission = ctrlPermission.Text;
         String title = ctrlTitle.Text;
         if (null == title || 0 == title.Trim().Length) {
            ctrlEnqueueStatus.Text = ResourceStrings.uploadTitleInvalid;
            return;
         }
         String description = ctrlDescription.Text;
         if (null == description || 0 == description.Trim().Length) {
            ctrlEnqueueStatus.Text = ResourceStrings.uploadDescriptionInvalid;
            return;
         }
         String fileName = ctrlSelectedFile.Text;
         mUploadVideoManager.getPendingUploadsModel().
            addItem(new UploadVideoManager.PendingUploadItem(fileName, permission, title, description));
      }

      private void ctrlRemovePending_Click(object sender, EventArgs e) {
         mUploadVideoManager.getPendingUploadsModel().removeItemAt(ctrlPendingList.SelectedIndex);
      }

      private void ctrlPendingMoveUp_Click(object sender, EventArgs e) {
         if (mUploadVideoManager.getPendingUploadsModel().moveItemUp(ctrlPendingList.SelectedIndex)) {
            ctrlPendingList.SelectedIndex -= 1;
         }
      }

      private void ctrlPendingMoveDown_Click(object sender, EventArgs e) {
         if (mUploadVideoManager.getPendingUploadsModel().moveItemDown(ctrlPendingList.SelectedIndex)) {
            ctrlPendingList.SelectedIndex += 1;
         }
      }

      private void ctrlClearFailedList_Click(object sender, EventArgs e) {
         mUploadVideoManager.getFailedUploadsModel().removeAllItems();
      }

      private void ctrlCancelActiveUpload_Click(object sender, EventArgs e) {

      }

      private void tabPage2_Click(object sender, EventArgs e) {

      }
   }
}
