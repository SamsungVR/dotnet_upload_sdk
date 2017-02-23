using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Newtonsoft.Json.Linq;

namespace UILib {

   internal partial class FormLoginSSO : UserControl {

      private string mSSOAppId, mSSOAppSecret;
      private Uri mLoginUri;

      public FormLoginSSO(string ssoAppId, string ssoAppSecret) {
         InitializeComponent();
         mSSOAppId = ssoAppId;
         mSSOAppSecret = ssoAppSecret;
         mLoginUri = new Uri(
            string.Format("https://account.samsung.com/account/check.do?serviceID={0}&actionID=StartOAuth2&countryCode=US&languageCode=en&accessToken=Y&serviceChannel=PC_APP",
            mSSOAppId));
      }

      private void toLoginPage() {
         ctrlWebView.Navigate(mLoginUri);
      }

      public void reload() {
         toLoginPage();
      }

      private void ctrlLoginPage_Click(object sender, EventArgs e) {
         toLoginPage();
      }

      private void ctrlBack_Click(object sender, EventArgs e) {
         if (ctrlWebView.CanGoBack) {
            ctrlWebView.GoBack();
         }
      }

      private static readonly string TAG = SDKLib.Util.getLogTag(typeof(FormLoginSSO));

      private void ctrlWebView_onDocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e) {
         SDKLib.Log.d(TAG, "Url: " + ctrlWebView.Url + " state: " + ctrlWebView.ReadyState);
         string data = ctrlWebView.DocumentText;
         JObject jsonData;
         try {
            jsonData = JObject.Parse(data);
         } catch (Exception ex) {
            SDKLib.Log.d(TAG, ex.ToString());
            return;
         }

         SDKLib.Log.d(TAG, jsonData.ToString());
      }

   }
}
