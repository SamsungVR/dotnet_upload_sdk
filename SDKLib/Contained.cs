using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace SDKLib {

   internal sealed class Contained {

      private Contained() {
      }

      public interface If {

         bool containedOnQueryFromServiceLocked(JObject jsonObject);
         void containedOnCreateInServiceLocked();
         void containedOnDeleteFromServiceLocked();
         void containedOnUpdateToServiceLocked();

         object containedGetIdLocked();
         Container.If getContainer();
      }

      public abstract class CType {

         public abstract Contained.If newInstance(Container.If container, JObject jsonObject);
         public abstract object getContainedId(JObject jsonObject);

         public abstract void notifyCreate(object callback, Container.If container, Contained.If contained);
         public abstract void notifyUpdate(object callback, Container.If container, Contained.If contained);
         public abstract void notifyDelete(object callback, Container.If container, Contained.If contained);
         public abstract void notifyQueried(object callback, Container.If container, Contained.If contained);
         public abstract void notifyListQueried(object callback, Container.If container, List<Contained.If> contained);

         private readonly List<string> mProperties = new List<string>();

         protected CType(string[] properties) {
            for (int i = 0; i < properties.Length; i += 1) {
               mProperties.Add(properties[i]);
            }
         }

         public List<string> getProperties() {
            return mProperties;
         }

         public virtual bool validateValue(out string outKey, out object outValue, string inKey, JObject jsonObject) {
            outKey = inKey;
            outValue = validateValue(outKey, jsonObject);
            return true;
         }

         public virtual object validateValue(string key, JObject jsonObject) {
            object rawValue = null;
            JToken token;

            if (jsonObject.TryGetValue(key, out token)) {
               rawValue = token.ToObject<object>();
            }
            return validateValue(key, token, rawValue);
         }

         public virtual object validateValue(string key, JToken token, object rawValue) {
            return rawValue;
         }
      }


      public abstract class BaseImpl<T> : Observable.BaseImpl<T>, If where T : class {

         private readonly Dictionary<string, object> mValues = new Dictionary<string, object>();
         protected readonly Container.If mContainer;
         private readonly Contained.If mSelf;
         private readonly Contained.CType mType;

         public BaseImpl(Contained.CType type, Contained.If self, Container.If container, JObject jsonObject) {
            mContainer = container;
            mSelf = null == self ? this : self;
            mType = type;
            if (null != jsonObject && !processQueryFromServiceLocked(jsonObject)) {
               throw new Exception();
            }
         }

         public BaseImpl(Contained.CType type, Container.If container, JObject jsonObject) :
            this(type, null, container, jsonObject) {
         }

         public virtual bool processQueryFromServiceNoLock(JObject jsonObject) {
            bool changed = false;
            List<string> properties = mType.getProperties();
            foreach (string key in properties) {
               string outKey = null;
               object outValue = null;

               if (mType.validateValue(out outKey, out outValue, key, jsonObject)) {
                  changed |= setNoLock(outKey, outValue);
               }
            }
            return changed;
         }

         public virtual bool processQueryFromServiceLocked(JObject jsonObject) {
            lock (mValues) {
               return processQueryFromServiceNoLock(jsonObject);
            }
         }

         public virtual bool isSameNoLock(object oldValue, object newValue) {
            return (oldValue == newValue || null != oldValue && oldValue.Equals(newValue) ||
                    null != newValue && newValue.Equals(oldValue));
         }

         private static readonly String TAG = Util.getLogTag(typeof(Contained));

         public virtual bool deleteNoLock(string key) {
            Log.d(TAG, "Deleting key: " + key + " on: " + this);
            mValues.Remove(key);
            return true;
         }

         public virtual bool addNoLock(string key, object newValue) {
            Log.d(TAG, "Adding key: " + key + " value: " + newValue + " on: " + this);

            mValues.Add(key, newValue);
            return true;
         }

         public virtual bool changeNoLock(string key, object oldValue, object newValue) {
            Log.d(TAG, "Changing key: " + key + " oldValue: " + oldValue + " newValue: " + newValue + " on: " + this);
            mValues.Remove(key);
            mValues.Add(key, newValue);
            return true;
         }

         public virtual bool setNoLock(string key, object newValue) {
            object oldValue;
            
            if (!mValues.TryGetValue(key, out oldValue)) {
               oldValue = null;
            }
            if (isSameNoLock(oldValue, newValue)) {
               return false;
            }
            if (null == newValue) {
               return deleteNoLock(key);
            }
            if (null == oldValue) {
               return addNoLock(key, newValue);
            }
            return changeNoLock(key, oldValue, newValue);
         }

         public virtual object getNoLock(string key) {
            object value;
            if (!mValues.TryGetValue(key, out value)) {
               return null;
            }
            return value;
         }

         public virtual object getLocked(string key) {
            lock (mValues) {
               return getNoLock(key);
            }
         }

         public Container.If getContainer() {
            return mContainer;
         }

         public abstract bool containedOnQueryFromServiceLocked(JObject jsonObject);
         public abstract void containedOnCreateInServiceLocked();
         public abstract void containedOnDeleteFromServiceLocked();
         public abstract void containedOnUpdateToServiceLocked();
         public abstract object containedGetIdLocked();
      }

   }

}
