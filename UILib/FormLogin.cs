using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace UILib {

   public partial class FormLogin : UserControl {

      private List<LoginProvider.If> mProviders = new List<LoginProvider.If>();

      public FormLogin() {
         InitializeComponent();

         mSelectedProvider = null;
         mProviders.Add(new LoginProvider.SSO());
         mProviders.Add(new LoginProvider.SVR());
      }

      private bool onProviderChanged(string providerId) {
         for (int i = mProviders.Count - 1; i >= 0; i -= 1) {
            LoginProvider.If provider = mProviders[i];
            if (provider.getId().Equals(providerId)) {
               return false;
            }
         }
         return false;
      }

      private void FormLogin_Load(object sender, EventArgs e) {
         string selectedProvider = UILibSettings.Default.selectedProvider;
         int index = mProviders.Count - 1;
         for (int i = 0; i < mProviders.Count; i += 1) {
            string id = mProviders[i].getId();
            ctrlProviders.Items.Add(id);
            if (id.Equals(selectedProvider)) {
               index = i;
            }
         }
         ctrlProviders.SelectedIndex = index;
      }

      private LoginProvider.If mSelectedProvider = null;

      private void ctrlProviders_SelectedIndexChanged(object sender, EventArgs e) {
         mSelectedProvider = null;
         ctrlProviderZone.Controls.Clear();

         int index = ctrlProviders.SelectedIndex;
         if (index >= 0 && index < mProviders.Count) {
            mSelectedProvider = mProviders[index];
            UserControl control = mSelectedProvider.getUI();
            if (null != control) {
               ctrlProviderZone.Controls.Add(control);
            }
         }
      }
   }
}
