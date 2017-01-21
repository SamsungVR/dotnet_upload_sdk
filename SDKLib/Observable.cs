
using System.Threading;
using System.Collections.Generic;

namespace SDKLib {

   public sealed class Observable {

      private Observable() {
      }

      public interface If<T> {
         bool addObserver(T observer, SynchronizationContext handler);
         bool removeObserver(T observer, SynchronizationContext handler);
      }


      internal class BaseImpl<T> : If<T> where T : class {


         internal interface IterationObserver {
            bool onIterate(Block block, object closure);
         }

         internal class Block {
            public readonly T mObserver;
            public readonly SynchronizationContext mHandler;

            public Block(T observer, SynchronizationContext handler) {
               mObserver = observer;
               mHandler = handler;
            }
         }

         private readonly List<Block> mObservers = new List<Block>();

         protected BaseImpl() {
         }

         protected virtual Block newBlock(T observer, SynchronizationContext handler) {
            return new Block(observer, handler);
         }

         protected virtual bool addObserverNoLock(T observer, SynchronizationContext handler) {
            Block block;

            for (int i = mObservers.Count - 1; i >= 0; i -= 1) {
               block = mObservers[i];
               if (block.mObserver == observer && block.mHandler == handler) {
                  return false;
               }
            }
            block = newBlock(observer, handler);
            mObservers.Add(block);
            return true;
         }


         public virtual bool addObserver(T observer, SynchronizationContext handler) {
            lock (mObservers) {
               return addObserverNoLock(observer, handler);
            }
         }


         public virtual bool addThreadSafeObserver(T observer) {
            lock (mObservers) {
               return addObserverNoLock(observer, null);
            }
         }

         public virtual bool removeObserver(T observer, SynchronizationContext handler) {
            Block block;

            bool result = false;

            lock (mObservers) {
               for (int i = mObservers.Count - 1; i >= 0; i -= 1) {
                  block = mObservers[i];
                  if (block.mObserver == observer && block.mHandler == handler) {
                     mObservers.RemoveAt(i);
                     result = true;
                  }
               }
               return result;
            }
         }


         public virtual void removeAllObservers(T observer) {
            Block block;

            lock (mObservers) {
               for (int i = mObservers.Count - 1; i >= 0; i -= 1) {
                  block = mObservers[i];
                  if (block.mObserver == observer) {
                     mObservers.RemoveAt(i);
                  }
               }
            }
         }


         public virtual void clear() {
            lock (mObservers) {
               mObservers.Clear();
            }
         }

         public virtual void iterate(IterationObserver iterationObserver, object closure) {
            lock (mObservers) {
               for (int i = mObservers.Count - 1; i >= 0; i -= 1) {
                  Block block = mObservers[i];
                  if (!iterationObserver.onIterate(block, closure)) {
                     break;
                  }
               }
            }
         }

         public virtual void dispatch(SendOrPostCallback callback, object closure) {
            lock (mObservers) {
               for (int i = mObservers.Count - 1; i >= 0; i -= 1) {
                  Block block = mObservers[i];
                  object[] args = { block.mObserver, closure };
                  if (null == block.mHandler) {
                     callback(args);
                  } else {
                     block.mHandler.Post(callback, args);
                  }
               }
            }
         }
      }
   }
}
