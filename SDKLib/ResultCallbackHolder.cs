using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace SDKLib {

   internal class ResultCallbackHolder {

      private Object mCallback = null;
      private SynchronizationContext mHandler = null;
      private object mClosure;

      private readonly object mLock;

      private static readonly string TAG = Util.getLogTag(typeof(ResultCallbackHolder));

      public ResultCallbackHolder() {
         mLock = new object();
      }

      public ResultCallbackHolder(object callback, SynchronizationContext handler, object closure)
         : this() {
         setNoLock(callback, handler, closure);
      }

      public ResultCallbackHolder(ResultCallbackHolder callbackHolder) : this(callbackHolder.getCallbackNoLock(),
         callbackHolder.getHandlerNoLock(), callbackHolder.getClosureNoLock()) {
      }

      public virtual void setLocked(object callback, SynchronizationContext handler, object closure) {
         lock (mLock) {
            setNoLock(callback, handler, closure);
         }
      }

      public virtual ResultCallbackHolder setNoLock(object callback, SynchronizationContext handler, object closure) {
         mClosure = closure;
         mHandler = handler;
         mCallback = callback;
         return this;
      }

      public virtual ResultCallbackHolder setNoLock(ResultCallbackHolder other) {
         return setNoLock(other.getCallbackNoLock(), other.getHandlerNoLock(), other.getClosureNoLock());
      }

      public object getClosureNoLock() {
         return mClosure;
      }

      public virtual void clearNoLock() {
         mClosure = null;
         mHandler = null;
         mCallback = null;
      }

      public virtual void copyFromNoLock(ResultCallbackHolder other) {
         mClosure = other.mClosure;
         mHandler = other.mHandler;
         mCallback = other.mCallback;
      }

      public object getCallbackNoLock() {
         return mCallback;
      }

      public SynchronizationContext getHandlerNoLock() {
         return mHandler;
      }

      public void acquireLock() {
         Monitor.Enter(mLock);
      }

      public void releaseLock() {
         Monitor.Exit(mLock);
      }

   }



}
