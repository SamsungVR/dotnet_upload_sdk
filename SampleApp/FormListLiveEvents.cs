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


   public partial class FormListLiveEvents : UserControl {

      private static readonly string TAG = Util.getLogTag(typeof(FormListLiveEvents));

      private readonly CallbackQueryLiveEvents mCallbackQueryLiveEvents;
      private readonly CallbackQueryLiveEvent mCallbackQueryLiveEvent;
      private readonly CallbackFinishLiveEvent mCallbackFinishLiveEvent;
      private readonly CallbackDeleteLiveEvent mCallbackDeleteLiveEvent;
      private readonly CallbackUploadSegment mCallbackUploadSegment;

      public FormListLiveEvents() {
         InitializeComponent();

         mCallbackQueryLiveEvents = new CallbackQueryLiveEvents(this);
         mCallbackQueryLiveEvent = new CallbackQueryLiveEvent(this);
         mCallbackFinishLiveEvent = new CallbackFinishLiveEvent(this);
         mCallbackDeleteLiveEvent = new CallbackDeleteLiveEvent(this);
         mCallbackUploadSegment = new CallbackUploadSegment(this);

         onSelected(null);
      }

      List<UserLiveEvent.If> mLiveEvents;

      private void setLiveEvents(List<UserLiveEvent.If> liveEvents) {
         mLiveEvents = liveEvents;
         refreshLiveEvents(-1);
      }

      private void refreshLiveEvents(int selectIndex) {
         ctrlEventsList.Items.Clear();
         if (null != mLiveEvents) {
            for (int i = 0; i < mLiveEvents.Count; i += 1) {
               UserLiveEvent.If liveEvent = mLiveEvents[i];
               ctrlEventsList.Items.Add(liveEvent.getId());
            }
            if (-1 != selectIndex) {
               ctrlEventsList.SelectedIndex = selectIndex;
            }
         }
      }

      public class CallbackQueryLiveEvents : User.Result.QueryLiveEvents.If {

         private readonly FormListLiveEvents mFormListLiveEvents;

         public CallbackQueryLiveEvents(FormListLiveEvents formListLiveEvents) {
            mFormListLiveEvents = formListLiveEvents;
         }

         public void onSuccess(object closure, List<UserLiveEvent.If> liveEvents) {
            Log.d(TAG, "onSuccess event: " + liveEvents);
            mFormListLiveEvents.ctrlStatus.Text = ResourceStrings.requestSuccess;
            mFormListLiveEvents.setLiveEvents(liveEvents);
         }

         public void onFailure(object closure, int status) {
            Log.d(TAG, "onError status: " + status);
            mFormListLiveEvents.ctrlStatus.Text = string.Format(ResourceStrings.failedWithStatus, status);
         }

         public void onCancelled(object closure) {
            Log.d(TAG, "onCancelled");
            mFormListLiveEvents.ctrlStatus.Text = ResourceStrings.requestCancelled;
         }

         public void onException(object closure, Exception ex) {
            mFormListLiveEvents.ctrlStatus.Text = ex.Message;
            Console.WriteLine(ex.Message);
            Console.WriteLine(ex.StackTrace);
         }
      };

      private void ctrlQuery_Click(object sender, EventArgs e) {
         onSelected(null);
         User.If user = App.getInstance().getUser();
         if (null == user) {
            return;
         }
         ctrlStatus.Text = ResourceStrings.requestInProgress;
         user.queryLiveEvents(mCallbackQueryLiveEvents, App.getInstance().getHandler(), null);
      }

      private string nullFix(object obj) {
         return null == obj ? "null" : obj.ToString();
      }

      private void onSelected(UserLiveEvent.If liveEvent) {
         ctrlLiveEventDetail.Items.Clear();
         if (null != liveEvent) {
            ctrlLiveEventDetail.Items.Add(nullFix(liveEvent.getTitle()));
            ctrlLiveEventDetail.Items.Add(nullFix(liveEvent.getDescription()));
            ctrlLiveEventDetail.Items.Add(nullFix(liveEvent.getProducerUrl()));
            ctrlLiveEventDetail.Items.Add(nullFix(liveEvent.getViewUrl()));
            ctrlLiveEventDetail.Items.Add(nullFix(liveEvent.getSource()));
            ctrlLiveEventDetail.Items.Add(nullFix(liveEvent.getState()));
            ctrlLiveEventDetail.Items.Add(nullFix(liveEvent.getVideoStereoscopyType()));
            ctrlLiveEventDetail.Items.Add(nullFix(liveEvent.getThumbnailUrl()));
            ctrlLiveEventDetail.Items.Add(nullFix(liveEvent.getStartedTime()));
            ctrlLiveEventDetail.Items.Add(nullFix(liveEvent.getViewerCount()));
         }
      }

      private UserLiveEvent.If getSelectedLiveEvent() {
         int index = ctrlEventsList.SelectedIndex;
         if (null != mLiveEvents && index >= 0 && index < mLiveEvents.Count) {
            return mLiveEvents[index];
         }
         return null;
      }

      private void ctrlEventsList_SelectedIndexChanged(object sender, EventArgs e) {
         UserLiveEvent.If selectedItem = getSelectedLiveEvent();
         onSelected(selectedItem);
      }

      public class CallbackQueryLiveEvent : UserLiveEvent.Result.Query.If {

         private readonly FormListLiveEvents mFormListLiveEvents;

         public CallbackQueryLiveEvent(FormListLiveEvents formListLiveEvents) {
            mFormListLiveEvents = formListLiveEvents;
         }

         public void onSuccess(object closure, UserLiveEvent.If liveEvent) {
            Log.d(TAG, "onSuccess event: " + liveEvent);
            mFormListLiveEvents.ctrlStatus.Text = ResourceStrings.requestSuccess;

            if (null != mFormListLiveEvents.mLiveEvents) {
               bool found = false;
               for (int i = mFormListLiveEvents.mLiveEvents.Count - 1; i >= 0; i -= 1) {
                  UserLiveEvent.If iLiveEvent = mFormListLiveEvents.mLiveEvents[i];
                  if (liveEvent.getId().Equals(iLiveEvent.getId())) {
                     mFormListLiveEvents.mLiveEvents.RemoveAt(i);
                     found = true;
                  }
               }
               if (found) {
                  mFormListLiveEvents.mLiveEvents.Insert(0, liveEvent);
                  mFormListLiveEvents.refreshLiveEvents(0);
               }
            }

         }

         public void onFailure(object closure, int status) {
            Log.d(TAG, "onError status: " + status);
            mFormListLiveEvents.ctrlStatus.Text = string.Format(ResourceStrings.failedWithStatus, status);
         }

         public void onCancelled(object closure) {
            Log.d(TAG, "onCancelled");
            mFormListLiveEvents.ctrlStatus.Text = ResourceStrings.requestCancelled;
         }

         public void onException(object closure, Exception ex) {
            mFormListLiveEvents.ctrlStatus.Text = ex.Message;
            Console.WriteLine(ex.Message);
            Console.WriteLine(ex.StackTrace);
         }
      };

      public class CallbackFinishLiveEvent : UserLiveEvent.Result.Finish.If {

         private readonly FormListLiveEvents mFormListLiveEvents;

         public CallbackFinishLiveEvent(FormListLiveEvents formListLiveEvents) {
            mFormListLiveEvents = formListLiveEvents;
         }

         public void onSuccess(object closure) {
            Log.d(TAG, "onSuccess");
            mFormListLiveEvents.ctrlStatus.Text = ResourceStrings.requestSuccess;
         }

         public void onFailure(object closure, int status) {
            Log.d(TAG, "onError status: " + status);
            mFormListLiveEvents.ctrlStatus.Text = string.Format(ResourceStrings.failedWithStatus, status);
         }

         public void onCancelled(object closure) {
            Log.d(TAG, "onCancelled");
            mFormListLiveEvents.ctrlStatus.Text = ResourceStrings.requestCancelled;
         }

         public void onException(object closure, Exception ex) {
            mFormListLiveEvents.ctrlStatus.Text = ex.Message;
            Console.WriteLine(ex.Message);
            Console.WriteLine(ex.StackTrace);
         }
      };

      public class CallbackDeleteLiveEvent : UserLiveEvent.Result.Delete.If {

         private readonly FormListLiveEvents mFormListLiveEvents;

         public CallbackDeleteLiveEvent(FormListLiveEvents formListLiveEvents) {
            mFormListLiveEvents = formListLiveEvents;
         }

         public void onSuccess(object closure) {
            Log.d(TAG, "onSuccess");
            mFormListLiveEvents.ctrlStatus.Text = ResourceStrings.requestSuccess;
         }

         public void onFailure(object closure, int status) {
            Log.d(TAG, "onError status: " + status);
            mFormListLiveEvents.ctrlStatus.Text = string.Format(ResourceStrings.failedWithStatus, status);
         }

         public void onCancelled(object closure) {
            Log.d(TAG, "onCancelled");
            mFormListLiveEvents.ctrlStatus.Text = ResourceStrings.requestCancelled;
         }

         public void onException(object closure, Exception ex) {
            mFormListLiveEvents.ctrlStatus.Text = ex.Message;
            Console.WriteLine(ex.Message);
            Console.WriteLine(ex.StackTrace);
         }
      };

      private void ctrlRefresh_Click(object sender, EventArgs e) {
         UserLiveEvent.If selectedItem = getSelectedLiveEvent();
         if (null != selectedItem) {
            ctrlEventsList.Items.Clear();
            ctrlLiveEventDetail.Items.Clear();
            ctrlStatus.Text = ResourceStrings.requestInProgress;
            selectedItem.query(mCallbackQueryLiveEvent, App.getInstance().getHandler(), null);
         }
      }

      private void ctrlFinish_Click(object sender, EventArgs e) {
         UserLiveEvent.If selectedItem = getSelectedLiveEvent();

         if (null != selectedItem) {
            ctrlStatus.Text = ResourceStrings.requestInProgress;
            selectedItem.finish(mCallbackFinishLiveEvent, App.getInstance().getHandler(), null);
         }

      }

      private void ctrlDelete_Click(object sender, EventArgs e) {
         UserLiveEvent.If selectedItem = getSelectedLiveEvent();

         if (null != selectedItem) {
            ctrlStatus.Text = ResourceStrings.requestInProgress;
            selectedItem.delete(mCallbackDeleteLiveEvent, App.getInstance().getHandler(), null);
         }
      }

      private void ctrlSelectFile_Click(object sender, EventArgs e) {
         DialogResult result = ctrlVideoChooser.ShowDialog();
         if (DialogResult.OK == result) {
            ctrlSelectedFile.Text = ctrlVideoChooser.FileName;
         } else {
            ctrlSelectedFile.Text = string.Empty;
         }
      }

      private UserLiveEventSegment.If mSegment = null;

      public class CallbackUploadSegment : UserLiveEvent.Result.UploadSegment.If {

         private readonly FormListLiveEvents mFormListLiveEvents;

         public CallbackUploadSegment(FormListLiveEvents formListLiveEvents) {
            mFormListLiveEvents = formListLiveEvents;
         }

         public void onSuccess(object closure) {
            mFormListLiveEvents.onUploadComplete();
         }

         public void onProgress(object closure, float progressPercent, long complete, long max) {
            mFormListLiveEvents.ctrlRawProgress.Text = progressPercent.ToString();
            mFormListLiveEvents.ctrlProgressBar.Value = (int)progressPercent;
         }

         public void onProgress(object o, long completed) {
            mFormListLiveEvents.ctrlRawProgress.Text = completed.ToString();
         }

         public void onFailure(object closure, int status) {
            mFormListLiveEvents.onUploadComplete();            
         }

         public void onCancelled(object closure) {
            mFormListLiveEvents.onUploadComplete();
         }

         public void onException(object closure, Exception ex) {
            mFormListLiveEvents.onUploadComplete();
            mFormListLiveEvents.ctrlStatus.Text = ex.Message;
            Console.WriteLine(ex.Message);
            Console.WriteLine(ex.StackTrace);
         }

         public void onSegmentIdAvailable(object closure, UserLiveEventSegment.If segment) {
            mFormListLiveEvents.mSegment = segment;
         }
      };

      private Stream mSource;
      private long mLength;

      private void ctrlUpload_Click(object sender, EventArgs e) {
         UserLiveEvent.If selectedItem = getSelectedLiveEvent();
         if (null == selectedItem || null != mSource) {
            return;
         }

         string fileName = ctrlSelectedFile.Text;
         FileInfo fileInfo = new FileInfo(fileName);
         if (!fileInfo.Exists) {
            ctrlStatus.Text = ResourceStrings.uploadFileDoesNotExist;
            return;
         }
         FileStream stream = null;
         try {
            stream = fileInfo.OpenRead();
         } catch (Exception) {
            ctrlStatus.Text = ResourceStrings.uploadFileOpenFailed;
            return;
         }
         long length = fileInfo.Length;
         ctrlStatus.Text = ResourceStrings.requestInProgress;
         if (selectedItem.uploadSegmentFromFD(stream, length, mCallbackUploadSegment, App.getInstance().getHandler(), this)) {
            mSource = stream;
            mLength = length;
            onBeginUpload();

         }
      }

      private void onBeginUpload() {
         ctrlStatus.Text = ResourceStrings.uploadInProgress;
      }

      private void onUploadComplete() {
         mSegment = null;
         if (null != mSource) {
            mSource.Close();
            mSource = null;
         }
      }

   }
}
