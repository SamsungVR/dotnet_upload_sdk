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


   public partial class FormCreateLiveEvent : UserControl {

      private static readonly string TAG = Util.getLogTag(typeof(FormCreateLiveEvent));

      private readonly CallbackCreateLiveEvent mCallbackCreateLiveEvent;

      public FormCreateLiveEvent() {
         InitializeComponent();

         mCallbackCreateLiveEvent = new CallbackCreateLiveEvent(this);
      }

      public class CallbackCreateLiveEvent : User.Result.CreateLiveEvent.If {

         private readonly FormCreateLiveEvent mFormCreateLiveEvent;

         public CallbackCreateLiveEvent(FormCreateLiveEvent formCreateLiveEvent) {
            mFormCreateLiveEvent = formCreateLiveEvent;
         }

         public void onSuccess(object closure, UserLiveEvent.If eventObj) {
            Log.d(TAG, "onSuccess event: " + eventObj);
            mFormCreateLiveEvent.ctrlStatus.Text = string.Format(ResourceStrings.liveCreateSuccess, eventObj.getId());
         }

         public void onFailure(object closure, int status) {
            Log.d(TAG, "onError status: " + status);
         }

         public void onCancelled(object closure) {
            Log.d(TAG, "onCancelled");
         }

         public void onException(object closure, Exception ex) {
            mFormCreateLiveEvent.ctrlStatus.Text = ex.Message;
            Console.WriteLine(ex.Message);
            Console.WriteLine(ex.StackTrace);
         }


      };

      


      private void ctrlCreate_Click(object sender, EventArgs e) {
         User.If user = App.getInstance().getUser();
         if (null == user) {
            return;
         }
         UserVideo.Permission permission = Util.string2Enum(typeof(UserVideo.Permission), ctrlPermission.Text, UserVideo.Permission.PRIVATE);
         UserLiveEvent.Source source = Util.string2Enum(typeof(UserLiveEvent.Source), ctrlSource.Text, UserLiveEvent.Source.RTMP);
         UserVideo.VideoStereoscopyType stereoscopyType = Util.string2Enum(typeof(UserVideo.VideoStereoscopyType), ctrlVideoStereoscopyType.Text, UserVideo.VideoStereoscopyType.DEFAULT);
         user.createLiveEvent(ctrlTitle.Text, ctrlDescription.Text, permission, source, stereoscopyType,
            mCallbackCreateLiveEvent, App.getInstance().getHandler(), null);
      }
   }
}
