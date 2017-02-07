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

      public class CallbackQueryLiveEvents : User.Result.QueryLiveEvents.If {

         private readonly FormListLiveEvents mFormListLiveEvents;

         public CallbackQueryLiveEvents(FormListLiveEvents formListLiveEvents) {
            mFormListLiveEvents = formListLiveEvents;
         }

         public void onSuccess(object closure, List<UserLiveEvent.If> liveEvents) {
            Log.d(TAG, "onSuccess event: " + liveEvents);
            mFormListLiveEvents.ctrlEventsList.Clear();
            for (int i = 0; i < liveEvents.Count; i += 1) {
               UserLiveEvent.If liveEvent = liveEvents[i];
               mFormListLiveEvents.ctrlEventsList.Items.Add(liveEvent.getId());
            }
         }

         public void onFailure(object closure, int status) {
            Log.d(TAG, "onError status: " + status);
         }

         public void onCancelled(object closure) {
            Log.d(TAG, "onCancelled");
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
   }
}
