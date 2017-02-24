using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using SDKLib;
using System.IO;

namespace SampleApp {


   public partial class FormManageLogs : UserControl {

      private static readonly string TAG = Util.getLogTag(typeof(FormListLiveEvents));

      public FormManageLogs() {
         InitializeComponent();
      }

      private static readonly string LOG_FILE_NAME = "vrsdk.log";

      internal static string getCurrentLogFile() {
         string settingsLogFile = AppSettings.Default.logFileFullPath;
         if (null == settingsLogFile || string.Empty.Equals(settingsLogFile)) {
            settingsLogFile = Path.Combine(Application.StartupPath, LOG_FILE_NAME);
         }
         return settingsLogFile;
      }

      private void setLogFile(string logFile) {
         AppSettings.Default.logFileFullPath = logFile;
         AppSettings.Default.Save();
         ctrlSelectedFile.Text = logFile;
         SDKLib.Log.setLogFilePath(ctrlSelectedFile.Text);
      }

      private void ctrlSelectFile_Click(object sender, EventArgs e) {
         DialogResult result = ctrlLogFileChooser.ShowDialog();
         if (DialogResult.OK == result) {
            setLogFile(ctrlLogFileChooser.FileName);
         }

      }

      private void button1_Click(object sender, EventArgs e) {
         Clipboard.SetText(ctrlSelectedFile.Text);
      }

      private void ctrlPreview_Click(object sender, EventArgs e) {
         SDKLib.Log.flushLogFile();
         try {
            System.Diagnostics.Process.Start(ctrlSelectedFile.Text);
         } catch (Exception ex) {
            ctrlStatus.Text = ex.ToString();
         }
      }

      private void ctrlClear_Click(object sender, EventArgs e) {
         SDKLib.Log.clearLogFile();
      }

      private void FormManageLogs_Load(object sender, EventArgs e) {
         ctrlSelectedFile.Text = getCurrentLogFile();
      }

   }
}
