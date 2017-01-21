using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;

namespace SampleApp {

   public partial class FormEndPointConfig : UserControl {

      private static string TAG = "FormEndPointConfig";
      private readonly EndPointConfigManager mEndPointCfgMgr;

      public FormEndPointConfig() {
         InitializeComponent();
         mEndPointCfgMgr = App.getInstance().getEndPointCfgMgr();
      }

      private void ctrlBack_Click(object sender, EventArgs e) {
         App.getInstance().getFormMain().setControl(new FormLogin());
      }

      private void FormEndPointConfig_Load(object sender, EventArgs e) {
         initialControlsEnabledState();
         mEndPointCfgMgr.addObserver(onEndPointEvent);
         updateUI();
      }

      public void onEndPointEvent(EndPointConfigManager.Event ev) {
         switch (ev) {
            case EndPointConfigManager.Event.CHANGED:
               updateUI();
               break;
         }
      }

      private void updateUI() {
         ctrlConfigList.Controls.Clear();
         EndPointConfig selected = mEndPointCfgMgr.getSelectedConfig();
         List<EndPointConfig> list = mEndPointCfgMgr.getList();

         int len = list.Count;

         for (int i = 0; i < len; i += 1) {
            EndPointConfig config = list[i];
            ctrlConfigList.Controls.Add(new FormEndPointConfigItem(this,  config, selected == config));
         }
      }

      public void editConfig(string argId) {
         EndPointConfig config = mEndPointCfgMgr.getConfig(argId);
         if (null == config) {
            return;
         }
         mEditId = argId;
         ctrlAPIKey.Text = config.getApiKey();
         ctrlUrl.Text = config.getUrl();
         enableControlsForAddOrEdit();
      }

      private void enableControlsForAddOrEdit() {
         ctrlConfigList.Enabled = false;
         ctrlAdd.Enabled = false;
         ctrlEditGroup.Enabled = true;
      }

      private void ctrlAdd_Click(object sender, EventArgs e) {
         enableControlsForAddOrEdit();
      }

      private void initialControlsEnabledState() {
         ctrlAdd.Enabled = true;
         ctrlEditGroup.Enabled = false;
         ctrlConfigList.Enabled = true;
         ctrlUrl.Text = string.Empty;
         ctrlAPIKey.Text = string.Empty;
         mEditId = null;
      }

      private void ctrlCancel_Click(object sender, EventArgs e) {
         initialControlsEnabledState();
      }

      private string mEditId = null;

      private void ctrlApply_Click(object sender, EventArgs e) {
         string url = ctrlUrl.Text;
         if (null == url || 0 == url.Trim().Length) {
            ctrlApplyStatus.Text = ResourceStrings.endPointUrlEmpty;
            ctrlUrl.Focus();
            return;
         }
         string apiKey = ctrlAPIKey.Text;
         if (null == apiKey || 0 == apiKey.Trim().Length) {
            ctrlApplyStatus.Text = ResourceStrings.apiKeyEmpty;
            ctrlAPIKey.Focus();
            return;
         }
         EndPointConfig config = null;
         if (null != mEditId) {
            config = mEndPointCfgMgr.getConfig(mEditId);
         }
         if (null == config) {
            config = new EndPointConfig();
         }
         config.setApiKey(apiKey);
         config.setUrl(url);
         mEndPointCfgMgr.addOrUpdateConfig(config);
      }

      private void FormEndPointConfig_Leave(object sender, EventArgs e) {
         mEndPointCfgMgr.removeObserver(onEndPointEvent);
      }

   }
}
