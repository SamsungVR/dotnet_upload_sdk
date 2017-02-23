using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace UILib {

   internal abstract class LoginProvider {

      internal interface If {
         UserControl getUI();
         string getId();
      }

      internal class BaseImpl : If {

         private readonly string mId;

         public BaseImpl(string id) {
            mId = id;
         }

         public virtual string getId() {
            return mId;
         }

         public virtual UserControl getUI() {
            return null;
         }

      }

      internal class SSO : BaseImpl {

         private readonly FormLoginSSO mLoginSSO;

         public SSO() : base(ResourceStrings.loginSSO) {
            mLoginSSO = new FormLoginSSO(this, "2269tcup3k", "D2C4F779BF5A8E0FD2AF120C1357B1C9");
         }

         internal void onCredsAvailable(JObject credsObject) {

         }

         override public UserControl getUI() {
            mLoginSSO.reload();
            return mLoginSSO;
         }

      }

      internal class SVR : BaseImpl {

         public SVR() : base(ResourceStrings.loginSVR) {
         }
      }

   }

}
