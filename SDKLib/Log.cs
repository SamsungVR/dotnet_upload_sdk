using System;
using System.Threading;
using System.Diagnostics;

namespace SDKLib {

   public static class Log {

      [Conditional("DEBUG")]
      public static void d(string tag, string msg) {
         Debug.WriteLine(msg, tag + "/" + Thread.CurrentThread.ManagedThreadId);
      }

   }
}
