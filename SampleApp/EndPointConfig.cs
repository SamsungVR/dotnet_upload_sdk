using System;
using System.Collections.Generic;
using System.Text;

namespace SampleApp {

   [Serializable]
   public class EndPointConfig {

      private readonly string mId;

      public EndPointConfig() {
         mId = Guid.NewGuid().ToString();
      }

      public EndPointConfig(string id) {
         mId = id;
      }

      private string mUrl, mApiKey, mSSOAppId;

      public void setUrl(string url) {
         mUrl = url;
      }

      public void setApiKey(string apiKey) {
         mApiKey = apiKey;
      }

      public void setSSOAppId(string ssoAppId) {
         mSSOAppId = ssoAppId;
      }

      public string getId() {
         return mId;
      }

      public string getUrl() {
         return mUrl;
      }

      public string getApiKey() {
         return mApiKey;
      }

      public string getSSOAppId() {
         return mSSOAppId;
      }

      public string getSSOAppSecret() {
         return null;
      }
   }

}
