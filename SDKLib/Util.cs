using Newtonsoft.Json.Linq;
using System;
using System.Threading;

namespace SDKLib {

   public static class Util {

      public static string getLogTag(object who) {
         System.Type type;

         if (who is System.Type) {
            type = who as System.Type;
         } else {
            type = who.GetType();
         }
         return type.Name;
      }


      internal static T jsonOpt<T>(JObject jsonObject, string attr, T defValue) {
         JToken token;
         if (jsonObject.TryGetValue(attr, out token)) {
            return token.Value<T>();
         }
         return defValue;
      }

      internal static T jsonGet<T>(JObject jsonObject, string attr) {
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
            tCallback.onSuccess(closure);
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
            tCallback.onSuccess(closure, mRef);
         }
      }

      internal class FailureCallbackNotifier : Util.CallbackNotifier {

         private readonly int mStatus;

         public FailureCallbackNotifier(ResultCallbackHolder callbackHolder, int status)
            : base(callbackHolder) {
            mStatus = status;
         }


         protected override void notify(object callback, object closure) {
            VR.Result.BaseCallback.If tCallback = callback as VR.Result.BaseCallback.If;
            tCallback.onFailure(closure, mStatus);
         }
      }

   }

}
