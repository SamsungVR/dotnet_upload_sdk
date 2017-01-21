using System;
using System.Collections.Generic;
using System.Text;

namespace SDKLib {

   internal class ObjectHolder<T> {

      private T mObj;

      public ObjectHolder(T obj) {
         set(obj);
      }

      public void set(T obj) {
         mObj = obj;
      }

      public T get() {
         return mObj;
      }

   }
}
