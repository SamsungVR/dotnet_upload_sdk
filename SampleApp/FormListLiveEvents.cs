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

      public FormListLiveEvents() {
         InitializeComponent();

         mCallbackQueryLiveEvents = new CallbackQueryLiveEvents(this);
      }

      List<UserLiveEvent.If> mLiveEvents;

      private void setLiveEvents(List<UserLiveEvent.If> liveEvents) {
         mLiveEvents = liveEvents;
         ctrlEventsList.Items.Clear();
         if (null != mLiveEvents) {
            for (int i = 0; i < mLiveEvents.Count; i += 1) {
               UserLiveEvent.If liveEvent = mLiveEvents[i];
               ctrlEventsList.Items.Add(liveEvent.getId());
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
         User.If user = App.getInstance().getUser();
         if (null == user) {
            return;
         }
         user.queryLiveEvents(mCallbackQueryLiveEvents, App.getInstance().getHandler(), null);
      }

      private void onSelected(UserLiveEvent.If liveEvent) {
      }

      private void ctrlEventsList_SelectedIndexChanged(object sender, EventArgs e) {
         UserLiveEvent.If selectedItem = null;

         int index = ctrlEventsList.SelectedIndex;
         if (null != mLiveEvents && index < mLiveEvents.Count) {
            selectedItem = mLiveEvents[index];
         }
         
      }
   }
}
