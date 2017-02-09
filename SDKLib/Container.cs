using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace SDKLib {

   internal sealed class Container {

      private Container() {
      }

      public interface If {
         List<U> containerOnQueryListOfContainedFromServiceLocked<U>(Contained.CType type, JObject jsonObject) where U : Contained.If;
         U containerOnQueryOfContainedFromServiceLocked<U>(Contained.CType type, U contained, JObject jsonObject) where U : Contained.If;
         U containerOnCreateOfContainedInServiceLocked<U>(Contained.CType type, JObject jsonObject) where U : Contained.If;
         U containerOnUpdateOfContainedToServiceLocked<U>(Contained.CType type, U contained) where U : Contained.If;
         U containerOnDeleteOfContainedFromServiceLocked<U>(Contained.CType type, U contained) where U : Contained.If;
      }

      public abstract class BaseImpl<T> : Observable.BaseImpl<T>, Container.If where T : class {

         private Dictionary<Contained.CType, Dictionary<object, object>> mContainedMap =
            new Dictionary<Contained.CType, Dictionary<object, object>>();
         private bool mRememberContained;
         private Container.If mSelf;

         protected BaseImpl(Container.If self, bool rememberContained) {
            mRememberContained = rememberContained;
            mSelf = null == self ? this : self;
         }

         protected BaseImpl(bool rememberContained)
            : this(null, rememberContained) {
         }

         protected virtual Dictionary<object, object> getContainedItemsNoLock<U>(Contained.CType type, bool create) where U : Contained.If {
            Dictionary<object, object> containedItems;

            if (!mContainedMap.TryGetValue(type, out containedItems) && create) {
               containedItems = new Dictionary<object, object>();
               mContainedMap.Add(type, containedItems);
            }
            return containedItems;
         }

         protected virtual Dictionary<object, object> getContainedItemsLocked<U>(Contained.CType type, bool create) where U : Contained.If {
            lock (mContainedMap) {
               return getContainedItemsNoLock<U>(type, create);
            }
         }

         protected virtual Dictionary<object, object> getContainedItemsLocked<U>(Contained.CType type) where U : Contained.If {
            return getContainedItemsLocked<U>(type, true);
         }


         protected virtual U newInstance<U>(Contained.CType type, JObject jsonObject) where U : Contained.If {
            U result = (U)type.newInstance(mSelf, jsonObject);
            if (null != result) {
               result.containedOnCreateInServiceLocked();
            }
            return result;
         }

         /*
          * Contained List
          */

         protected virtual List<U> processQueryListOfContainedFromServiceNoLock<U>(Contained.CType type,
            Dictionary<object, object> containedItems, JArray jsonItems, List<U> result) where U : Contained.If {
            if (null == jsonItems) {
               return null;
            }

            if (null == result) {
               result = new List<U>();
            } else {
               result.Clear();
            }

            List<object> toRemove = new List<object>();
            if (mRememberContained) {
               if (null == containedItems) {
                  containedItems = getContainedItemsLocked<U>(type);
               }
               toRemove.AddRange(containedItems.Values);
            }
            int len = jsonItems.Count;
            for (int i = 0; i < len; i += 1) {
               JObject jsonObject = jsonItems[i] as JObject;
               object id = type.getContainedId(jsonObject);
               if (null == id) {
                  continue;
               }
               if (!mRememberContained) {
                  U contained = newInstance<U>(type, jsonObject);
                  if (null != contained) {
                     result.Add(contained);
                  }
               } else {
                  object contained = containedItems[id];
                  if (null == contained) {
                     contained = newInstance<U>(type, jsonObject);
                     if (null == contained) {
                        continue;
                     }
                     containedItems.Add(id, contained);
                  } else {
                     ((Contained.If)contained).containedOnQueryFromServiceLocked(jsonObject);
                     toRemove.Remove(contained);
                  }

               }
            }
            if (mRememberContained) {

               for (int i = 0; i < toRemove.Count; i += 1) {
                  Contained.If item = (Contained.If)toRemove[i];
                  if (containedItems.Remove(item.containedGetIdLocked())) {
                     item.containedOnDeleteFromServiceLocked();
                  }
               }
               foreach (object o in containedItems.Values) {
                  result.Add((U)o);
               }
            }
            return result;
         }

         public virtual List<U> processQueryListOfContainedFromServiceLocked<U>(
                 Contained.CType type, JArray jsonItems, List<U> result) where U : Contained.If {
            Dictionary<object, object> containedItems = getContainedItemsLocked<U>(type);
            lock (containedItems) {
               return processQueryListOfContainedFromServiceNoLock<U>(type, containedItems, jsonItems, result);
            }
         }

         protected virtual U getContainedByIdNoLock<U>(Contained.CType type, Dictionary<object, object> containedItems, object id) where U : Contained.If {
            if (!mRememberContained) {
               return default(U);
            }
            if (null == containedItems) {
               containedItems = getContainedItemsLocked<U>(type);
            }

            object contained;
            if (containedItems.TryGetValue(id, out contained)) {
               return (U)contained;
            }
            return default(U);
         }

         protected virtual U getContainedByIdLocked<U>(Contained.CType type, object id) where U : Contained.If {
            Dictionary<object, object> containedItems = getContainedItemsLocked<U>(type);
            lock (containedItems) {
               return getContainedByIdNoLock<U>(type, containedItems, id);
            }
         }

         /*
          * Contained Delete
          */


         protected virtual U processDeleteOfContainedFromServiceNoLock<U>(
                 Contained.CType type, Dictionary<object, object> containedItems, U contained) where U : Contained.If {
            if (mRememberContained) {
               if (null == containedItems) {
                  containedItems = getContainedItemsLocked<U>(type);
               }
               if (!containedItems.Remove(contained.containedGetIdLocked())) {
                  contained = default(U);
               }
            }
            if (null != contained) {
               contained.containedOnDeleteFromServiceLocked();
            }
            return contained;
         }

         public virtual U processDeleteOfContainedFromServiceLocked<U>(Contained.CType type, U contained) where U : Contained.If {
            Dictionary<object, object> containedItems = getContainedItemsLocked<U>(type);
            lock (containedItems) {
               return processDeleteOfContainedFromServiceNoLock(type, containedItems, contained);
            }
         }

         /*
          * Contained Create
          */

         protected virtual U processCreateOfContainedInServiceNoLock<U>(
                 Contained.CType type, Dictionary<object, object> containedItems, JObject jsonObject,
                 bool updateIfExists) where U : Contained.If {

            object id = type.getContainedId(jsonObject);
            if (null == id) {
               return default(U);
            }
            object contained;

            if (mRememberContained) {
               if (null == containedItems) {
                  containedItems = getContainedItemsLocked<U>(type);
               }
               if (containedItems.TryGetValue(id, out contained)) {
                  if (!updateIfExists) {
                     return default(U);
                  }
                  ((Contained.If)contained).containedOnQueryFromServiceLocked(jsonObject);
                  return (U)contained;
               }
            }
            contained = newInstance<U>(type, jsonObject);
            if (null != contained && mRememberContained) {
               containedItems.Add(id, contained);
            }
            return (U)contained;
         }

         public virtual U processCreateOfContainedInServiceLocked<U>(
                 Contained.CType type, JObject jsonObject, bool updateIfExists) where U : Contained.If {
            Dictionary<object, object> containedItems = getContainedItemsLocked<U>(type);
            lock (containedItems) {
               return processCreateOfContainedInServiceNoLock<U>(type, containedItems, jsonObject, updateIfExists);
            }
         }

         /*
          * Contained Update
          */

         protected virtual U processUpdateOfContainedToServiceNoLock<U>(Contained.CType type,
            Dictionary<object, object> containedItems, U contained) where U : Contained.If {
            if (mRememberContained) {
               if (null == containedItems) {
                  containedItems = getContainedItemsLocked<U>(type);
               }
               object id = contained.containedGetIdLocked();
               object check;
               if (!containedItems.TryGetValue(id, out check)) {
                  return default(U);
               }
               contained = (U)check;
            }
            if (null != contained) {
               contained.containedOnUpdateToServiceLocked();
            }
            return contained;
         }

         public virtual U processUpdateOfContainedToServiceLocked<U>(Contained.CType type, U contained) where U : Contained.If {
            Dictionary<object, object> containedItems = getContainedItemsLocked<U>(type);
            lock (containedItems) {
               return processUpdateOfContainedToServiceNoLock<U>(type, containedItems, contained);
            }
         }

         /*
          * Contained Query
          */

         protected virtual U processQueryOfContainedFromServiceNoLock<U>(
                 Contained.CType type, Dictionary<object, object> containedItems, U contained,
                 JObject jsonObject, bool addIfMissing) where U : Contained.If {
            if (null == contained) {
               return processCreateOfContainedInServiceNoLock<U>(type, containedItems, jsonObject, true);
            }
            if (mRememberContained) {
               if (null == containedItems) {
                  containedItems = getContainedItemsLocked<U>(type);
               }

               object id = contained.containedGetIdLocked();
               object existing;
               if (!containedItems.TryGetValue(id, out existing)) {
                  if (!addIfMissing) {
                     return default(U);
                  }
                  containedItems.Add(id, contained);
               } else {
                  contained = (U)existing;
               }
            }
            if (contained.containedOnQueryFromServiceLocked(jsonObject)) {
               //notify
            }
            return contained;
         }

         public virtual U processQueryOfContainedFromServiceLocked<U>(
                 Contained.CType type, U contained, JObject jsonObject, bool addIfMissing) where U : Contained.If {
            Dictionary<object, object> containedItems = getContainedItemsLocked<U>(type);
            lock (containedItems) {
               return processQueryOfContainedFromServiceNoLock<U>(type, containedItems, contained,
                       jsonObject, addIfMissing);
            }
         }

         public abstract List<U> containerOnQueryListOfContainedFromServiceLocked<U>(Contained.CType type, JObject jsonObject) where U : Contained.If;
         public abstract U containerOnQueryOfContainedFromServiceLocked<U>(Contained.CType type, U contained, JObject jsonObject) where U : Contained.If;
         public abstract U containerOnCreateOfContainedInServiceLocked<U>(Contained.CType type, JObject jsonObject) where U : Contained.If;
         public abstract U containerOnUpdateOfContainedToServiceLocked<U>(Contained.CType type, U contained) where U : Contained.If;
         public abstract U containerOnDeleteOfContainedFromServiceLocked<U>(Contained.CType type, U contained) where U : Contained.If;

      }
   }

}
