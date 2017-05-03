using System.Windows.Forms;

namespace SampleApp {

   public class FormBase : UserControl {

      virtual public void onLoad() {
      }

      virtual public void onUnload() {
      }

      private void InitializeComponent() {
         this.SuspendLayout();
         // 
         // FormBase
         // 
         this.Name = "FormBase";
         this.Size = new System.Drawing.Size(100, 100);
         this.ResumeLayout(false);

      }
   }
}
