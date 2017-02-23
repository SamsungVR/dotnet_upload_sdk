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
      private readonly string mPostData;
      private readonly LoginProvider.SSO mProviderSSO;

      private static readonly string sLocalhost = "localhost";

      public FormLoginSSO(LoginProvider.SSO providerSSO, string ssoAppId, string ssoAppSecret) {
         InitializeComponent();

         mProviderSSO = providerSSO;
         mSSOAppId = ssoAppId;
         mSSOAppSecret = ssoAppSecret;
         mPostData = string.Format("serviceID={0}&actionID=StartOAuth2&redirect_uri=http://{1}&accessToken=Y", mSSOAppId, sLocalhost);
         mLoginUri = new Uri("https://account.samsung.com/mobile/account/check.do");
      }

      private void toLoginPage() {
         ctrlWebView.Navigate(mLoginUri, "", Encoding.UTF8.GetBytes(mPostData), "Content-Type: application/x-www-form-urlencoded" + Environment.NewLine);
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

      private void ctrlWebView_onNavigating(object sender, WebBrowserNavigatingEventArgs e) {
         if (sLocalhost.Equals(e.Url.Authority)) {
            e.Cancel = true;

            HtmlElementCollection inputs = ctrlWebView.Document.GetElementsByTagName("input");
            JObject json = null;
            if (null != inputs && 1 == inputs.Count) {
               
               for (int i = inputs.Count - 1; i >= 0; i -= 1) {
                  if (!"code".Equals(inputs[i].Name)) {
                     continue;
                  }
                  try {
                     json = JObject.Parse(inputs[i].GetAttribute("value"));
                  } catch (Exception ex) {
                     SDKLib.Log.d(TAG, inputs[i].Id + " " + inputs[i].Name + " " + ex);
                     json = null;
                  }
                  
               }
            }
            if (null == json) {
               toLoginPage();
            } else {
               mProviderSSO.onCredsAvailable(json);
            }

         }
      }

   }
}
