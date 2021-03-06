﻿using Newtonsoft.Json.Linq;
using System;
using System.ComponentModel;
using System.Reflection;
using System.Threading;

namespace SDKLib {

   public static class Util {

      internal static readonly bool DEBUG = true;

      public static string getLogTag(object who) {
         return Log.getLogTag(who);
      }

      private static System.Globalization.CultureInfo sCultureENUS = System.Globalization.CultureInfo.CreateSpecificCulture("en-US");

      public static string enum2String(Enum enumValue) {
         Type type = enumValue.GetType();
         string name = enumValue.ToString();
         string nameLower = name.ToLower(sCultureENUS);

         MemberInfo[] allMembers = type.GetMembers();
         for (int i = allMembers.Length - 1; i >= 0; i -= 1) {
            MemberInfo memberInfo = allMembers[i];
            string memberNameLower = memberInfo.Name.ToLower(sCultureENUS);
            if (null == memberNameLower || !memberNameLower.Equals(nameLower)) {
               continue;
            }
            object[] attributes = memberInfo.GetCustomAttributes(typeof(DescriptionAttribute), false);
            if (null == attributes || attributes.Length < 1) {
               continue;
            }
            DescriptionAttribute attribute = (DescriptionAttribute)attributes[0];
            return attribute.Description;
         }
         return name;
      }

      public static bool string2Enum<T>(out T result, string enumName, T def) {
         if (null != enumName) {
            string enumNameLower = enumName.ToLower(sCultureENUS);
            Type enumClass = typeof(T);
            MemberInfo[] allMembers = enumClass.GetMembers();
            for (int i = allMembers.Length - 1; i >= 0; i -= 1) {
               MemberInfo memberInfo = allMembers[i];
               string memberName = memberInfo.Name;
               string memberNameLower = memberName.ToLower(sCultureENUS);
               if (enumNameLower.Equals(memberNameLower)) {
                  result = (T)Enum.Parse(enumClass, memberName, true);
                  return true;
               }
               object[] attributes = memberInfo.GetCustomAttributes(typeof(DescriptionAttribute), false);
               for (int j = attributes.Length - 1; j >= 0; j -= 1) {
                  DescriptionAttribute attribute = (DescriptionAttribute)attributes[j];
                  string descName = attribute.Description;
                  string descNameLower = descName.ToLower(sCultureENUS);
                  if (enumNameLower.Equals(descNameLower)) {
                     result = (T)Enum.Parse(enumClass, memberName, true);
                     return true;
                  }
               }
            }
         }
         result = def;
         return false;
      }

      public static bool string2Enum<T>(out T result, string enumName) {
         return string2Enum<T>(out result, enumName, default(T));
      }

      public static T jsonOpt<T>(JObject jsonObject, string attr, T defValue) {
         JToken token;
         if (jsonObject.TryGetValue(attr, out token)) {
            return token.Value<T>();
         }
         return defValue;
      }

      public static T jsonGet<T>(JObject jsonObject, string attr) {
         JToken token;
         token = jsonObject.GetValue(attr);
         return token.Value<T>();
      }

      internal static bool checkEquals(object a, object b) {
         return (a == b) ||
                 ((null != a) && a.Equals(b)) ||
                 ((null != b) && b.Equals(a));
      }

      internal abstract class CallbackNotifier : ResultCallbackHolder {

         public CallbackNotifier(object callback, SynchronizationContext handler, object closure)
            : base(callback, handler, closure) {
         }

         public CallbackNotifier(ResultCallbackHolder callbackHolder)
            : base(callbackHolder) {
         }

         private static readonly string TAG = Util.getLogTag(typeof(CallbackNotifier));

         public virtual void run(object args) {
            acquireLock();
            object callback = getCallbackNoLock();
            object closure = getClosureNoLock();
            releaseLock();
            Log.d(TAG, "Notifying " + this + " cb: " + callback + " closure: " + closure);
            if (null != callback) {
               notify(callback, closure);
            }
         }

         public virtual bool post() {
            System.Threading.SynchronizationContext handler;

            acquireLock();
            handler = getHandlerNoLock();
            releaseLock();

            if (null != handler) {
               handler.Post(run, this);
            }
            return true;
         }

         protected abstract void notify(object callback, object closure);

      }

      internal class SuccessCallbackNotifier : Util.CallbackNotifier {

         public SuccessCallbackNotifier(ResultCallbackHolder callbackHolder)
            : base(callbackHolder) {
         }

         public SuccessCallbackNotifier(object callback, SynchronizationContext handler, object closure)
            : base(callback, handler, closure) {
         }

         protected override void notify(object callback, object closure) {
            VR.Result.SuccessCallback.If tCallback = callback as VR.Result.SuccessCallback.If;
            if (null != tCallback) {
               tCallback.onSuccess(closure);
            }
         }
      }

      internal class SuccessWithResultCallbackNotifier<Y> : Util.CallbackNotifier {

         private readonly Y mRef;

         public SuccessWithResultCallbackNotifier(ResultCallbackHolder callbackHolder, Y rf)
            : base(callbackHolder) {
            mRef = rf;
         }

         public SuccessWithResultCallbackNotifier(object callback, SynchronizationContext handler, object closure, Y rf)
            : base(callback, handler, closure) {
            mRef = rf;
         }

         protected override void notify(object callback, object closure) {
            VR.Result.SuccessWithResultCallback.If<Y> tCallback = callback as VR.Result.SuccessWithResultCallback.If<Y>;
            if (null != tCallback) {
               tCallback.onSuccess(closure, mRef);
            }
         }
      }

      /* http://stackoverflow.com/questions/9655181/how-to-convert-a-byte-array-to-a-hex-string-in-java */

      internal static string bytesToHex(byte[] bytes, char[] hexArray) {
         char[] hexChars = new char[bytes.Length * 2];
         for (int j = 0; j < bytes.Length; j += 1) {
            int offset = j * 2;
            int v = bytes[j] & 0xFF;
            hexChars[offset] = hexArray[(v >> 4) & 0x0F];
            hexChars[offset + 1] = hexArray[v & 0x0F];
         }
         return new string(hexChars);
      }

      private static readonly char[] sUCHexArray = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'A', 'B', 'C', 'D', 'E', 'F' };
      private static readonly char[] sLCHexArray = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'a', 'b', 'c', 'd', 'e', 'f' };

      internal static string bytesToHex(byte[] bytes, bool upperCase) {
         return bytesToHex(bytes, upperCase ? sUCHexArray : sLCHexArray);
      }

      internal class FailureCallbackNotifier : Util.CallbackNotifier {

         private readonly int mStatus;

         public FailureCallbackNotifier(ResultCallbackHolder callbackHolder, int status)
            : base(callbackHolder) {
            mStatus = status;
         }


         protected override void notify(object callback, object closure) {
            VR.Result.BaseCallback.If tCallback = callback as VR.Result.BaseCallback.If;
            if (null != tCallback) {
               tCallback.onFailure(closure, mStatus);
            }
         }
      }

   }

}
