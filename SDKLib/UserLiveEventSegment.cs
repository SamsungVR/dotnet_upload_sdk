using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Text;
using System.Threading;

namespace SDKLib {

   public sealed class UserLiveEventSegment {

      private UserLiveEventSegment() {
      }

      public interface If {


         /**
          * Cancel an ongoing upload. This yields the same result as calling UserLiveEvent.cancelSegmentUpload()
          *
          * @param closure An object that the application can use to uniquely identify this request.
          *                See callback documentation.
          * @return true if a cancel was scheduled, false if the upload could not be cancelled, which
          * can happen because the upload failed or completed even before the cancel could be reqeusted.
          */

         bool cancelUpload(object closure);

      }
   }
}
