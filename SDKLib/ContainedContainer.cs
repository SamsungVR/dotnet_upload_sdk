using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace SDKLib {

   internal sealed class ContainedContainer {

      private ContainedContainer() {
      }

      public abstract class BaseImpl<T> : Observable.BaseImpl<T>, Contained.If, Container.If where T : class {

         private class MyContainer : Container.BaseImpl<T> {

            public MyContainer(Container.If self, bool rememberContained)
               : base(self, rememberContained) {
            }

            public override List<Contained.If> containerOnQueryListOfContainedFromServiceLocked(Contained.CType type, JObject jsonObject) {
               throw new NotImplementedException();
            }

            public override bool containerOnQueryOfContainedFromServiceLocked(Contained.CType type, Contained.If contained, JObject jsonObject) {
               throw new NotImplementedException();
            }

            public override Contained.If containerOnCreateOfContainedInServiceLocked(Contained.CType type, JObject jsonObject) {
               throw new NotImplementedException();
            }

            public override Contained.If containerOnUpdateOfContainedToServiceLocked(Contained.CType type, Contained.If contained) {
               throw new NotImplementedException();
            }

            public override Contained.If containerOnDeleteOfContainedFromServiceLocked(Contained.CType type, Contained.If contained) {
               throw new NotImplementedException();
            }
         }

         private class MyContained : Contained.BaseImpl<T> {

            public MyContained(Contained.CType type, Contained.If self, Container.If container, JObject jsonObject) :
               base(type, self, container, jsonObject) {
            }

            public override bool containedOnQueryFromServiceLocked(JObject jsonObject) {
               throw new NotImplementedException();
            }

            public override void containedOnCreateInServiceLocked() {
               throw new NotImplementedException();
            }

            public override void containedOnDeleteFromServiceLocked() {
               throw new NotImplementedException();
            }

            public override void containedOnUpdateToServiceLocked() {
               throw new NotImplementedException();
            }

            public override object containedGetIdLocked() {
               throw new NotImplementedException();
            }
         }

         protected readonly Contained.BaseImpl<T> mContainedImpl;
         protected readonly Container.BaseImpl<T> mContainerImpl;

         protected BaseImpl(Contained.CType type, Container.If container, JObject jsonObject, bool rememberContained) {
            mContainedImpl = new MyContained(type, this, container, jsonObject);
            mContainerImpl = new MyContainer(this, rememberContained);
         }

         public virtual Container.If getContainer() {
            return mContainedImpl.getContainer();
         }

         public Container.BaseImpl<T> getContainerImpl() {
            return mContainerImpl;
         }

         public Contained.BaseImpl<T> getContainedImpl() {
            return mContainedImpl;
         }

         public virtual bool setNoLock(string key, object newValue) {
            return mContainedImpl.setNoLock(key, newValue);
         }

         public virtual object getNoLock(string attr) {
            return mContainedImpl.getNoLock(attr);
         }

         public virtual object getLocked(string attr) {
            return mContainedImpl.getLocked(attr);
         }

         public virtual bool processQueryFromServiceNoLock(JObject jsonObject) {
            return mContainedImpl.processQueryFromServiceLocked(jsonObject);
         }

         public virtual bool processQueryFromServiceLocked(JObject jsonObject) {
            return mContainedImpl.processQueryFromServiceLocked(jsonObject);
         }


         abstract public bool containedOnQueryFromServiceLocked(JObject jsonObject);
         abstract public void containedOnCreateInServiceLocked();
         abstract public void containedOnDeleteFromServiceLocked();
         abstract public void containedOnUpdateToServiceLocked();
         abstract public object containedGetIdLocked();

         abstract public List<Contained.If> containerOnQueryListOfContainedFromServiceLocked(Contained.CType type, JObject jsonObject);
         abstract public bool containerOnQueryOfContainedFromServiceLocked(Contained.CType type, Contained.If contained, JObject jsonObject);

         abstract public Contained.If containerOnCreateOfContainedInServiceLocked(Contained.CType type, JObject jsonObject);

         abstract public Contained.If containerOnUpdateOfContainedToServiceLocked(Contained.CType type, Contained.If contained);

         abstract public Contained.If containerOnDeleteOfContainedFromServiceLocked(Contained.CType type, Contained.If contained);
      }



   }

}
