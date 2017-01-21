using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace SDKLib {

   internal sealed class Container {

      private Container() {
      }

      public interface If {
         List<Contained.If> containerOnQueryListOfContainedFromServiceLocked(Contained.CType type, JObject jsonObject);
         bool containerOnQueryOfContainedFromServiceLocked(Contained.CType type, Contained.If contained, JObject jsonObject);
         Contained.If containerOnCreateOfContainedInServiceLocked(Contained.CType type, JObject jsonObject);
         Contained.If containerOnUpdateOfContainedToServiceLocked(Contained.CType type, Contained.If contained);
         Contained.If containerOnDeleteOfContainedFromServiceLocked(Contained.CType type, Contained.If contained);
      }

      public abstract class BaseImpl<T> : Observable.BaseImpl<T>, Container.If where T : class {

         private Dictionary<Contained.CType, Dictionary<object, Contained.If>> mContainedMap =
            new Dictionary<Contained.CType, Dictionary<object, Contained.If>>();
         private bool mRememberContained;
         private Container.If mSelf;

         protected BaseImpl(Container.If self, bool rememberContained) {
            mRememberContained = rememberContained;
            mSelf = null == self ? this : self;
         }

         protected BaseImpl(bool rememberContained)
            : this(null, rememberContained) {
         }

         protected virtual Dictionary<object, Contained.If> getContainedItemsNoLock(Contained.CType type, bool create) {
            Dictionary<object, Contained.If> containedItems;

            if (!mContainedMap.TryGetValue(type, out containedItems) && create) {
               containedItems = new Dictionary<object, Contained.If>();
               mContainedMap.Add(type, containedItems);
            }
            return containedItems;
         }

         protected virtual Dictionary<object, Contained.If> getContainedItemsLocked(Contained.CType type, bool create) {
            lock (mContainedMap) {
               return getContainedItemsNoLock(type, create);
            }
         }

         protected virtual Dictionary<object, Contained.If> getContainedItemsLocked(Contained.CType type) {
            return getContainedItemsLocked(type, true);
         }


         protected virtual Contained.If newInstance(Contained.CType type, JObject jsonObject) {
            Contained.If result = type.newInstance(mSelf, jsonObject);
            if (null != result) {
               result.containedOnCreateInServiceLocked();
            }
            return result;
         }

         /*
          * Contained List
          */

         protected virtual List<Contained.If> processQueryListOfContainedFromServiceNoLock(Contained.CType type,
            Dictionary<object, Contained.If> containedItems, JArray jsonItems, List<Contained.If> result) {
            if (null == jsonItems) {
               return null;
            }

            if (null == result) {
               result = new List<Contained.If>();
            } else {
               result.Clear();
            }

            List<Contained.If> toRemove = new List<Contained.If>();
            if (mRememberContained) {
               if (null == containedItems) {
                  containedItems = getContainedItemsLocked(type);
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
                  Contained.If contained = newInstance(type, jsonObject);
                  if (null != contained) {
                     result.Add(contained);
                  }
               } else {
                  Contained.If contained = containedItems[id];
                  if (null == contained) {
                     contained = newInstance(type, jsonObject);
                     if (null == contained) {
                        continue;
                     }
                     containedItems.Add(id, contained);
                  } else {
                     contained.containedOnQueryFromServiceLocked(jsonObject);
                     toRemove.Remove(contained);
                  }

               }
            }
            if (mRememberContained) {

               for (int i = 0; i < toRemove.Count; i += 1) {
                  Contained.If item = toRemove[i];
                  if (containedItems.Remove(item.containedGetIdLocked())) {
                     item.containedOnDeleteFromServiceLocked();
                  }
               }
               result.AddRange(containedItems.Values);
            }
            return result;
         }

         protected virtual List<Contained.If> processQueryListOfContainedFromServiceLocked(
                 Contained.CType type, JArray jsonItems, List<Contained.If> result) {
            Dictionary<object, Contained.If> containedItems = getContainedItemsLocked(type);
            lock (containedItems) {
               return processQueryListOfContainedFromServiceNoLock(type, containedItems, jsonItems, result);
            }
         }

         protected virtual Contained.If getContainedByIdNoLock(Contained.CType type, Dictionary<object, Contained.If> containedItems, object id) {
            if (!mRememberContained) {
               return null;
            }
            if (null == containedItems) {
               containedItems = getContainedItemsLocked(type);
            }

            Contained.If contained;
            if (containedItems.TryGetValue(id, out contained)) {
               return contained;
            }
            return null;
         }

         protected virtual Contained.If getContainedByIdLocked(Contained.CType type, object id) {
            Dictionary<object, Contained.If> containedItems = getContainedItemsLocked(type);
            lock (containedItems) {
               return getContainedByIdNoLock(type, containedItems, id);
            }
         }

         /*
          * Contained Delete
          */


         protected virtual Contained.If processDeleteOfContainedFromServiceNoLock(
                 Contained.CType type, Dictionary<object, Contained.If> containedItems, Contained.If contained) {
            if (mRememberContained) {
               if (null == containedItems) {
                  containedItems = getContainedItemsLocked(type);
               }
               if (!containedItems.Remove(contained.containedGetIdLocked())) {
                  contained = null;
               }
            }
            if (null != contained) {
               contained.containedOnDeleteFromServiceLocked();
            }
            return contained;
         }

         protected virtual Contained.If processDeleteOfContainedFromServiceLocked(
                 Contained.CType type, Contained.If contained) {
            Dictionary<object, Contained.If> containedItems = getContainedItemsLocked(type);
            lock (containedItems) {
               return processDeleteOfContainedFromServiceNoLock(type, containedItems, contained);
            }
         }

         /*
          * Contained Create
          */

         protected virtual Contained.If processCreateOfContainedInServiceNoLock(
                 Contained.CType type, Dictionary<object, Contained.If> containedItems, JObject jsonObject,
                 bool updateIfExists) {

            object id = type.getContainedId(jsonObject);
            if (null == id) {
               return null;
            }
            Contained.If contained;

            if (mRememberContained) {
               if (null == containedItems) {
                  containedItems = getContainedItemsLocked(type);
               }
               if (containedItems.TryGetValue(id, out contained)) {
                  if (!updateIfExists) {
                     return null;
                  }
                  contained.containedOnQueryFromServiceLocked(jsonObject);
                  return contained;
               }
            }
            contained = newInstance(type, jsonObject);
            if (null != contained && mRememberContained) {
               containedItems.Add(id, contained);
            }
            return contained;
         }

         protected virtual Contained.If processCreateOfContainedInServiceLocked(
                 Contained.CType type, JObject jsonObject, bool updateIfExists) {
            Dictionary<object, Contained.If> containedItems = getContainedItemsLocked(type);
            lock (containedItems) {
               return processCreateOfContainedInServiceNoLock(type, containedItems, jsonObject, updateIfExists);
            }
         }

         /*
          * Contained Update
          */

         protected virtual Contained.If processUpdateOfContainedToServiceNoLock(Contained.CType type,
            Dictionary<object, Contained.If> containedItems, Contained.If contained) {
            if (mRememberContained) {
               if (null == containedItems) {
                  containedItems = getContainedItemsLocked(type);
               }
               object id = contained.containedGetIdLocked();
               if (!containedItems.TryGetValue(id, out contained)) {
                  return null;
               }
            }
            if (null != contained) {
               contained.containedOnUpdateToServiceLocked();
            }
            return contained;
         }

         protected virtual Contained.If processUpdateOfContainedToServiceLocked(
                 Contained.CType type, Contained.If contained) {
            Dictionary<object, Contained.If> containedItems = getContainedItemsLocked(type);
            lock (containedItems) {
               return processUpdateOfContainedToServiceNoLock(type, containedItems, contained);
            }
         }

         /*
          * Contained Query
          */

         protected virtual bool processQueryOfContainedFromServiceNoLock(
                 Contained.CType type, Dictionary<object, Contained.If> containedItems, Contained.If contained,
                 JObject jsonObject, bool addIfMissing) {

            if (mRememberContained) {
               if (null == containedItems) {
                  containedItems = getContainedItemsLocked(type);
               }

               object id = contained.containedGetIdLocked();
               Contained.If existing;
               if (!containedItems.TryGetValue(id, out existing)) {
                  if (!addIfMissing) {
                     return false;
                  }
                  containedItems.Add(id, contained);
               } else {
                  contained = existing;
               }
            }
            if (contained.containedOnQueryFromServiceLocked(jsonObject)) {
               return true;
            }
            return false;
         }

         protected virtual bool processQueryOfContainedFromServiceLocked(
                 Contained.CType type, Contained.If contained, JObject jsonObject, bool addIfMissing) {
            Dictionary<object, Contained.If> containedItems = getContainedItemsLocked(type);
            lock (containedItems) {
               return processQueryOfContainedFromServiceNoLock(type, containedItems, contained,
                       jsonObject, addIfMissing);
            }
         }

         public abstract List<Contained.If> containerOnQueryListOfContainedFromServiceLocked(Contained.CType type, JObject jsonObject);
         public abstract bool containerOnQueryOfContainedFromServiceLocked(Contained.CType type, Contained.If contained, JObject jsonObject);
         public abstract Contained.If containerOnCreateOfContainedInServiceLocked(Contained.CType type, JObject jsonObject);
         public abstract Contained.If containerOnUpdateOfContainedToServiceLocked(Contained.CType type, Contained.If contained);
         public abstract Contained.If containerOnDeleteOfContainedFromServiceLocked(Contained.CType type, Contained.If contained);

      }
   }

}
