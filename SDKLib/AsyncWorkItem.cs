namespace SDKLib {

   internal interface AsyncWorkItemType {
      AsyncWorkItem newInstance(APIClientImpl apiClient);
   }

   internal abstract class AsyncWorkItem {

      private readonly AsyncWorkItemType mType;

      public AsyncWorkItem(AsyncWorkItemType type) {
         mType = type;
      }

      public AsyncWorkItemType getType() {
         return mType;
      }

      private bool mCancelled = false;

      public virtual void setCancelled(bool cancelled) {
         mCancelled = cancelled;
      }

      public virtual bool isCancelled() {
         return mCancelled;
      }

      public virtual void cancel() {
         setCancelled(true);
      }

      protected virtual void recycle() {
         setCancelled(false);
      }

      public abstract void run();

      protected byte[] mIOBuf;

      public virtual void renew(byte[] buf) {
         mIOBuf = buf;
         mCancelled = false;
      }


   }

}
