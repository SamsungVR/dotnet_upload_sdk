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

         FormLoginSSO mLoginSSO;

         public SSO() : base(ResourceStrings.loginSSO) {
         }

         override public UserControl getUI() {
            mLoginSSO = new FormLoginSSO("", "");
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
