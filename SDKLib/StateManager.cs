using System.Threading;

namespace SDKLib {

   internal class StateManager<R> : Observable.BaseImpl<StateManager<R>.StateChangeObserver> {

      public delegate void StateChangeObserver(R obj, System.Enum oldState, System.Enum newState);

      private System.Enum mCurrentState;
      private readonly R mObj;

      private readonly SendOrPostCallback mNotifier = delegate (object args) {
         object[] argsAsArray = args as object[];
         StateChangeObserver observer = argsAsArray[0] as StateChangeObserver;
         object[] stateArgs = argsAsArray[1] as object[];

         R obj = (R)stateArgs[0];
         System.Enum oldState = (System.Enum)stateArgs[1];
         System.Enum newState = (System.Enum)stateArgs[2];
         Log.d(TAG, "Notifying of state change observer: " + observer + " obj " + obj + " os " + oldState + " ns " + newState);
         observer(obj, oldState, newState);
      };

      public StateManager(R obj, System.Enum initialState) {
         mCurrentState = initialState;
         mObj = obj;
      }

      public System.Enum getState() {
         lock (mLock) {
            return mCurrentState;
         }
      }

      public bool isInState(System.Enum state) {
         lock (mLock) {
            return mCurrentState.Equals(state);
         }
      }

      private readonly object mLock = new object();
      private static readonly string TAG = Util.getLogTag(typeof(StateManager<R>));

      public bool setState(System.Enum newState) {
         lock (mLock) {
            if (System.Enum.Equals(newState, mCurrentState)) {
               return false;
            }
            Log.d(TAG, "Setting state to " + newState + " from " + mCurrentState + " on " + mObj);
            System.Enum oldState = mCurrentState;
            mCurrentState = newState;

            object[] stateArgs = { mObj, oldState, newState };
            dispatch(mNotifier, stateArgs);
         }
         return true;
      }

   }
}
