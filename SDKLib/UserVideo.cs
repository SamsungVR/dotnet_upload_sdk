using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Text;

namespace SDKLib {

   public sealed class UserVideo {

      private UserVideo() {
      }

      public enum VideoStereoscopyType {
         DEFAULT,
         MONOSCOPIC,
         TOP_BOTTOM_STEREOSCOPIC,
         LEFT_RIGHT_STEREOSCOPIC,
         DUAL_FISHEYE
      };

      public enum Permission {
         [Description("Private")]
         PRIVATE,
         [Description("Unlisted")]
         UNLISTED,
         [Description("Public")]
         PUBLIC,
         [Description("VR Only")]
         VR_ONLY,
         [Description("Web Only")]
         WEB_ONLY
      };

      internal static string toString(Permission perm) {
         switch (perm) {
            case Permission.PRIVATE:
               return "Private";
            case Permission.UNLISTED:
               return "Unlisted";
            case Permission.PUBLIC:
               return "Public";
            case Permission.VR_ONLY:
               return "VR Only";
            case Permission.WEB_ONLY:
               return "Web Only";
         }
         return null;
      }

      private static readonly string TAG = Util.getLogTag(typeof(UserVideo));

      /**
       * Given a permission string, return the corresponding enum value. This method
       * does a lower case compare using the en-US Locale.
       *
       * @param perm The permission string. Must be one of private, public, unlisted, vr only, web only
       * @return A matching value from the Permission Enumeration or NULL for no match
       */

      public static Permission fromString(string perm) {
         System.Globalization.CultureInfo culture = System.Globalization.CultureInfo.CreateSpecificCulture("en-US");
         Type type = typeof(Permission);
         Permission result = Permission.PRIVATE;
         Array perms = Enum.GetValues(type);

         if (null != perm) {
            string permInLower = perm.ToLower(culture);
            for (int i = perms.Length - 1; i >= 0; i -= 1) {
               Permission ePerm = (Permission)perms.GetValue(i);
               string mineInLower = toString(ePerm).ToLower(culture);
               if (mineInLower.Equals(permInLower)) {
                  result = ePerm;
                  break;
               }
            }
         }
         Log.d(TAG, "permissionFromString str: " + perm + " result: " + result);
         return result;
      }

      public interface If {
         /**
          * Cancel an ongoing upload. This yields the same result as calling User.cancelVideoUpload()
          *
          * @param closure An object that the application can use to uniquely identify this request.
          *                See callback documentation.
          * @return true if a cancel was scheduled, false if the upload could not be cancelled, which
          * can happen because the upload failed or completed even before the cancel could be reqeusted.
          */

         bool cancelUpload(Object closure);

         /**
          * Retry a failed upload. The params are similar to those of User.uploadVideo. No check is
          * made to ensure that the parcel file descriptor points to the same file as the failed
          * upload.
          *
          *
          * @return true if a retry was scheduled, false if the upload is already in progress or cannot
          * be retried. An upload cannot be retried if it already completed successfully.
          */

         bool retryUpload(Stream source, long length, User.Result.UploadVideo.If callback,
                             System.Threading.SynchronizationContext handler, object closure);


      }

   }

}
