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

      private string mUrl, mApiKey;

      public void setUrl(string url) {
         mUrl = url;
      }

      public void setApiKey(string apiKey) {
         mApiKey = apiKey;
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


   }

}
