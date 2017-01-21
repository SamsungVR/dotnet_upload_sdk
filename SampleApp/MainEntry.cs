using System;
using SDKLib;
using System.Windows.Forms;
using System.Threading;

namespace SampleApp {

   public class MainEntry {

      private static readonly string TAG = Util.getLogTag(typeof(MainEntry));

      [STAThread]
      static void Main(string[] args) {
         App.getInstance().main();
      }
   }

}
