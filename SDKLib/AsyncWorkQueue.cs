﻿using System;
using System.Collections.Generic;
using System.Threading;

namespace SDKLib {

   internal class AsyncWorkQueue {

      private static string TAG = Util.getLogTag(typeof(APIClient));

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
      private AsyncWorkItem mActiveWorkItem = null;

      private readonly AsyncWorkItemFactory mAsyncWorkItemFactory;

      public AsyncWorkQueue(AsyncWorkItemFactory factory, int ioBufSize) {
         mStateManager = new StateManager<AsyncWorkQueue>(this, State.INITIAILIZING);
         mIOBuf = new byte[ioBufSize];
         mAsyncWorkItemFactory = factory;
         mThread = new Thread(processRequests);
         mThread.Name = "AsyncWorkQueue " + this; 
         mThread.Start();
      }

      private const int TIMEOUT = Timeout.Infinite;

      private void processRequests() {
         Log.d(TAG, "Started processing requests");
         mStateManager.setState(State.INITIALIZED);
         while (mStateManager.isInState(State.INITIALIZED)) {
            if (!mResetEvent.WaitOne(TIMEOUT)) {
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
            mActiveWorkItem.run();
            lock (mWorkItems) {
               mActiveWorkItem = null;
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

      public void clear() {
         lock (mWorkItems) {
            mWorkItems.Clear();
            if (null != mActiveWorkItem) {
               mActiveWorkItem.cancel();
            }
         }
      }

      public bool destroy() {
         lock (mWorkItems) { 
            if (!mStateManager.isInState(State.INITIALIZED)) {
               return false;
            }
            mStateManager.setState(State.DESTROYING);
            clear();
            mResetEvent.Set();
         }
         return true;
      }

      public void join() {
         mThread.Join(TIMEOUT);
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
