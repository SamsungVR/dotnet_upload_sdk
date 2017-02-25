using System;
using System.Collections.Generic;
using System.Threading;

namespace SDKLib {

   internal class AsyncWorkQueue {

      private static string TAG = Util.getLogTag(typeof(AsyncWorkQueue));

      public delegate AsyncWorkItem AsyncWorkItemFactory(AsyncWorkItemType type);

      public enum State : int {
         INITIAILIZING,
         INITIALIZED,
         DESTROYING,
         DESTROYED
      }

      private readonly StateManager<AsyncWorkQueue> mStateManager;

      private readonly Thread mThread;
      private readonly ManualResetEvent mResetEvent = new ManualResetEvent(false);
      private readonly Queue<AsyncWorkItem> mWorkItems = new Queue<AsyncWorkItem>();
      private readonly byte[] mIOBuf;
      private readonly int mTimeout;
      private AsyncWorkItem mActiveWorkItem = null;

      private readonly AsyncWorkItemFactory mAsyncWorkItemFactory;

      public AsyncWorkQueue(AsyncWorkItemFactory factory, int ioBufSize, int timeout,
         StateManager<AsyncWorkQueue>.StateChangeObserver stateChangeObserver, SynchronizationContext handler) {
         mStateManager = new StateManager<AsyncWorkQueue>(this, State.INITIAILIZING);
         mTimeout = timeout;
         mIOBuf = new byte[ioBufSize];
         mAsyncWorkItemFactory = factory;
         mThread = new Thread(processRequests);
         mThread.Name = "AsyncWorkQueue " + this; 
         mStateManager.addObserver(stateChangeObserver, handler);
         mThread.Start();
      }

      private void processRequests() {
         Log.d(TAG, "Started processing requests");
         if (mStateManager.isInState(State.INITIAILIZING)) {
            mStateManager.setState(State.INITIALIZED);
            
            while (mStateManager.isInState(State.INITIALIZED)) {
               Log.d(TAG, "Waiting for requests " + this);
               if (!mResetEvent.WaitOne(Timeout.Infinite)) {
                  continue;
               }
               mResetEvent.Reset();
               if (!mStateManager.isInState(State.INITIALIZED)) {
                  break;
               }
               lock (mWorkItems) {
                  try {
                     mActiveWorkItem = mWorkItems.Dequeue();
                  } catch (InvalidOperationException) {
                     mActiveWorkItem = null;
                     continue;
                  }
               }
               Log.d(TAG, "Running work item " + mActiveWorkItem);
               mActiveWorkItem.run();
               Log.d(TAG, "Done running work item " + mActiveWorkItem);
               lock (mWorkItems) {
                  mActiveWorkItem = null;
               }
            }
         }
         mResetEvent.Close();
         mStateManager.setState(State.DESTROYED);
         Log.d(TAG, "Done processing requests " + this);
      }

      public StateManager<AsyncWorkQueue> getStateManager() {
         return mStateManager;
      }

      public bool enqueue(AsyncWorkItem workItem) {
         lock (mWorkItems) {
            if (State.INITIALIZED.CompareTo((State)mStateManager.getState()) < 0) {
               return false;
            }
            mWorkItems.Enqueue(workItem);
            mResetEvent.Set();
         }
         return true;
      }

      private void clearNoLock() {
         mWorkItems.Clear();
         if (null != mActiveWorkItem) {
            mActiveWorkItem.cancel();
         }
      }

      public void clear() {
         lock (mWorkItems) {
            clearNoLock();
         }
      }

      public bool destroy() {
         lock (mWorkItems) { 
            if (!mStateManager.isInState(State.INITIALIZED)) {
               Log.d(TAG, "Attempting to destroy in state " + mStateManager.getState());
               return false;
            }
            mStateManager.setState(State.DESTROYING);
            clearNoLock();
            mResetEvent.Set();
         }
         Log.d(TAG, "Destroyed " + this + " successfully");
         return true;
      }

      public void join() {
         mThread.Join(mTimeout);
      }

      public AsyncWorkItem obtainWorkItem(AsyncWorkItemType type) {
         AsyncWorkItem result = mAsyncWorkItemFactory(type);
         if (null != result) {
            result.renew(mIOBuf);
         }
         return result;
      }

      public interface IterationObserver {
         bool onIterate(AsyncWorkItem workItem, object args);
      }

      public void iterateWorkItems(IterationObserver observer, object args) {
         lock (mWorkItems) {
            if (null != mActiveWorkItem) {
               if (!observer.onIterate(mActiveWorkItem, args)) {
                  return;
               }
            }
            foreach (AsyncWorkItem workItem in mWorkItems) {
               if (!observer.onIterate(workItem, args)) {
                  break;
               }
            }
         }
      }

   }
}
