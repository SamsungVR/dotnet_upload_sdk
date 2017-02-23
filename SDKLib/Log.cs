using System;
using System.Threading;
using System.Diagnostics;
using System.IO;

namespace SDKLib {

   public static class Log {


      public static void d(string tag, string msg) {
         d(tag, msg, false);
      }

      [Conditional("DEBUG")]
      public static void d(string tag, string msg, bool flush) {
         lock (sLock) {
            Debug.WriteLine(msg, tag + "/" + Thread.CurrentThread.ManagedThreadId);
            if (null != sLogFile) {
               sLogFile.WriteLine(tag + "/" + Thread.CurrentThread.ManagedThreadId + ": " + msg);
               if (flush) {
                  sLogFile.Flush();
               }
            }
         }
      }

      private static StreamWriter sLogFile = null;
      private static readonly object sLock = new object();
      private static string sCurrentLogFile;

      public static void setLogFilePath(string logFilePath) {

         lock (sLock) {
            closeLogFileNoLock();
            sCurrentLogFile = logFilePath;
            openLogFileNoLock();
         }
      }

      public static void closeLogFile() {
         lock (sLock) {
            closeLogFileNoLock();
         }
      }

      private static void openLogFileNoLock() {
         if (null != sCurrentLogFile) {
            sLogFile = File.AppendText(sCurrentLogFile);
            if (null != sLogFile) {
               sLogFile.WriteLine(Environment.NewLine);
               sLogFile.WriteLine("Date/Time       : " + DateTime.Now.ToString());
               sLogFile.WriteLine("OSVer           : " + Environment.OSVersion);
               sLogFile.WriteLine("CmdLine         : " + Environment.CommandLine);
               sLogFile.WriteLine("PWD             : " + Environment.CurrentDirectory);
               sLogFile.WriteLine("Managed version : " + Environment.Version);
               sLogFile.WriteLine("Working set     : " + Environment.WorkingSet);
               sLogFile.WriteLine("Interactive?    : " + Environment.UserInteractive);
               sLogFile.WriteLine("Processor count : " + Environment.ProcessorCount);
               sLogFile.WriteLine("Network avail?  : " + System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable());
               sLogFile.WriteLine(Environment.NewLine);
            }
         }
      }

      private static void closeLogFileNoLock() {
         if (null != sLogFile) {
            sLogFile.Close();
            sLogFile = null;
         }
      }

      public static void clearLogFile() {
         lock (sLock) {
            closeLogFileNoLock();
            if (null != sCurrentLogFile) {
               File.WriteAllText(sCurrentLogFile, String.Empty);
               openLogFileNoLock();
            }
         }
      }

      public static void flushLogFile() {
         lock (sLock) {
            if (null != sLogFile) {
               sLogFile.Flush();
            }
         }

      }
   }
}
