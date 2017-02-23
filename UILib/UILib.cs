using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace UILib {

   public static class UILib {

      internal class Impl {

         private FormMain mFormMain;

         public Impl() {
         }

         public bool loginInternal() {
            if (null != mFormMain) {
               return false;
            }
            mFormMain = new FormMain(this);
            mFormMain.setControl(new FormLogin());
            mFormMain.Show();
            return true;
         }

         public void onFormClosed(FormClosedEventArgs e) {
            mFormMain = null;
         }

      }

      private static UILib.Impl sInstance;

      public static bool login() {
         if (null == sInstance) {
            sInstance = new Impl();
         }
         return sInstance.loginInternal();
      }
   }
}
