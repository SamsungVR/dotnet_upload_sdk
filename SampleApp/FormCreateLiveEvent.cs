using System;
using System.Windows.Forms;
using SDKLib;

namespace SampleApp {


   public partial class FormCreateLiveEvent : FormBase {

      private static readonly string TAG = Util.getLogTag(typeof(FormCreateLiveEvent));

      private readonly CallbackCreateLiveEvent mCallbackCreateLiveEvent;

      public FormCreateLiveEvent() {
         InitializeComponent();

         mCallbackCreateLiveEvent = new CallbackCreateLiveEvent(this);
         ctrlPermission.SelectedIndex = 2;
         ctrlVideoStereoscopyType.SelectedIndex = 0;
         ctrlSource.SelectedIndex = 0;
         ctrlTitle.Focus();
      }

      public class CallbackCreateLiveEvent : User.Result.CreateLiveEvent.If {

         private readonly FormCreateLiveEvent mFormCreateLiveEvent;

         public CallbackCreateLiveEvent(FormCreateLiveEvent formCreateLiveEvent) {
            mFormCreateLiveEvent = formCreateLiveEvent;
         }

         public void onSuccess(object closure, UserLiveEvent.If eventObj) {
            Log.d(TAG, "onSuccess event: " + eventObj);
            mFormCreateLiveEvent.ctrlStatus.Text = string.Format(ResourceStrings.liveCreateSuccess, eventObj.getId());
            mFormCreateLiveEvent.ctrlRTMPUrl.Text = eventObj.getProducerUrl();
         }

         public void onFailure(object closure, int status) {
            Log.d(TAG, "onError status: " + status);
            mFormCreateLiveEvent.ctrlStatus.Text = string.Format(ResourceStrings.failedWithStatus, status);
         }

         public void onCancelled(object closure) {
            Log.d(TAG, "onCancelled");
            mFormCreateLiveEvent.ctrlStatus.Text = ResourceStrings.requestCancelled;
         }

         public void onException(object closure, Exception ex) {
            mFormCreateLiveEvent.ctrlStatus.Text = ex.Message;
            Console.WriteLine(ex.Message);
            Console.WriteLine(ex.StackTrace);
         }


      };

      private void ctrlCreate_Click(object sender, EventArgs e) {
         ctrlRTMPUrl.Text = string.Empty;

         User.If user = App.getInstance().getUser();
         if (null == user) {
            return;
         }
         
         UserVideo.Permission permission;
         Util.string2Enum<UserVideo.Permission>(out permission, ctrlPermission.Text, UserVideo.Permission.PRIVATE);

         UserLiveEvent.Source source;
         Util.string2Enum<UserLiveEvent.Source>(out source, ctrlSource.Text, UserLiveEvent.Source.RTMP);

         UserVideo.VideoStereoscopyType stereoscopyType;
         Util.string2Enum<UserVideo.VideoStereoscopyType>(out stereoscopyType, ctrlVideoStereoscopyType.Text, UserVideo.VideoStereoscopyType.DEFAULT);

         user.createLiveEvent(ctrlTitle.Text, ctrlDescription.Text, permission, source, stereoscopyType,
            mCallbackCreateLiveEvent, App.getInstance().getHandler(), null);
      }

      private void ctrlCopyRTMPUrlToCB_Click(object sender, EventArgs e) {
         string url = ctrlRTMPUrl.Text; 
         if (null == url || string.Empty.Equals(url))  {
            ctrlStatus.Text = ResourceStrings.noRTMPUrlToCopy;
            Clipboard.Clear();
         } else {
            Clipboard.SetText(url);
            ctrlStatus.Text = string.Format(ResourceStrings.copiedToClipboard, url);
         }
      }

      private void ctrlOptionsGroup_Enter(object sender, EventArgs e) {

      }
   }
}
