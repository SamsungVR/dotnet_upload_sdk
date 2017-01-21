using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace SampleApp {

   public partial class FormEndPointConfigItem : UserControl {
      
      private readonly string mId;
      private readonly FormEndPointConfig mForm;

      public FormEndPointConfigItem(FormEndPointConfig form, EndPointConfig config, bool isSelected) {
         InitializeComponent();
         mForm = form;
         ctrlApiKey.Text = config.getApiKey();
         ctrlUrl.Text = config.getUrl();
         mId = config.getId();
         if (isSelected) {
            base.BackColor = Color.Orange;
         }
      }

      private void FormEndPointConfigItem_Load(object sender, EventArgs e) {
      }

      private void ctrlDelete_Click(object sender, EventArgs e) {
         EndPointConfigManager mgr = App.getInstance().getEndPointCfgMgr();
         mgr.deleteConfig(mId);
      }

      private void ctrlEdit_Click(object sender, EventArgs e) {
         mForm.editConfig(mId);
      }

      private void ctrlSelect_Click(object sender, EventArgs e) {
         EndPointConfigManager mgr = App.getInstance().getEndPointCfgMgr();
         mgr.selectConfig(mId);
      }
   }
}
