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
         ctrlSelectedFiles.Items.Clear();
         if (DialogResult.OK == result) {
            string[] files = ctrlVideoChooser.FileNames;
            if (null != files) {
               foreach (string file in files) {
                  ctrlSelectedFiles.Items.Add(file);
               }
            }
         }
      }

      public void onUploadStateChanged(UploadVideoManager.UploadState state) {
         switch (state) {
            case UploadVideoManager.UploadState.BEGIN:
               ctrlInProgressStatus.Text = ResourceStrings.uploadBegin;
               onBeginUploadInternal();
               break;
            case UploadVideoManager.UploadState.END:
               ctrlInProgressStatus.Text = ResourceStrings.uploadEnd;
               onEndUploadInternal();
               break;
            case UploadVideoManager.UploadState.PAUSED:
               ctrlInProgressStatus.Text = ResourceStrings.uploadPaused;
               break;
            case UploadVideoManager.UploadState.RESUMED:
               ctrlInProgressStatus.Text = ResourceStrings.uploadResumed;
               break;
         }
         ctrlUploadDashboard.SelectTab(1);

      }

      public void onServiceReachable(bool isReachable) {
         if (isReachable) {
            ctrlConnectivityStatus.Text = ResourceStrings.vrServiceReachable;
            ctrlConnectivityStatus.ForeColor = System.Drawing.Color.Green;
         } else {
            ctrlConnectivityStatus.Text = ResourceStrings.vrServiceNotReachable;
            ctrlConnectivityStatus.ForeColor = System.Drawing.Color.Red;
         }
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

      public void onCompletedItemsChanged() {
         onCompletedItemsChangedInternal();
         ctrlUploadDashboard.SelectTab(2);
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

      private void onCompletedItemsChangedInternal() {
         UploadVideoManager.CompletedUploadItemsModel model = mUploadVideoManager.getCompletedUploadsModel();
         populateList(ctrlCompletedList, model.mItems);
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

      private void onEndUploadInternal() {
         ctrlInProgressDetails.Items.Clear();
         ctrlInProgressDetails.Items.Add(ResourceStrings.noActiveUpload);
         ctrlUploadProgressRaw.Text = "";
         ctrlUploadProgressVisual.Value = 0;
      }

      public override void onLoad() {
         mUploadVideoManager.addCallback(this);
         onPendingItemsChangedInternal();
         onFailedItemsChangedInternal();
         onCompletedItemsChangedInternal();
         onBeginUploadInternal();
         onServiceReachable(mUploadVideoManager.isServiceReachable());
         onUploadStateChanged(mUploadVideoManager.getUploadState());
      }

      public override void onUnload() {
         mUploadVideoManager.removeCallback(this);
      }

      private string getTitle(string fileName) {
         string result = fileName;   
         char[] fileChars = fileName.ToCharArray();
         int len = fileChars.Length;
         for (int i = len - 1; i >= 0; i -= 1) {
            if ('\\' == fileChars[i]) {
               result = fileName.Substring(i + 1);
               break;
            }
         }
         return result.Replace('.', '_');
      }

      private void ctrlEnqueue_Click(object sender, EventArgs e) {
         String permission = ctrlPermission.Text;
         int count = ctrlSelectedFiles.Items.Count;
         if (count > 0) {
            for (int i = 0; i < count; i += 1) {
               object fileObj = ctrlSelectedFiles.Items[i];
               string fileName = (string)fileObj;
               string title = getTitle(fileName);
               string description = title;
               mUploadVideoManager.getPendingUploadsModel().
                  addItem(new UploadVideoManager.PendingUploadItem(fileName, permission, title, description));
            }
         }
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
         mUploadVideoManager.cancelActiveUpload();
      }

      private void ctrlRequeueFailed_Click(object sender, EventArgs e) {
         int selectedIndex = ctrlFailedList.SelectedIndex;
         mUploadVideoManager.moveFailedToPending(selectedIndex);
      }

      private void ctrlClearCompletedList_Click(object sender, EventArgs e) {
         mUploadVideoManager.getCompletedUploadsModel().removeAllItems();
      }

      private void ctrlRequeueCompleted_Click(object sender, EventArgs e) {
         int selectedIndex = ctrlCompletedList.SelectedIndex;
         mUploadVideoManager.moveCompletedToPending(selectedIndex);
      }
   }
}
