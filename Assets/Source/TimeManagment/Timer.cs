using UnityEngine.Events;

namespace PandoraCube.TimeManagment
{
    abstract public class Timer
    {
        protected UnityEvent<Timer> event_handler = new UnityEvent<Timer>();
        protected float elapsed = 0.0f;
        protected float end = 0.0f;

        /**
         * Create and set the timer to go off after seconds.
         */
        public Timer(float seconds) => end = seconds;

        /**
         * Get the elapsed time.
         * 
         * The elapsed time will not significantly increase past the
         * end time.
         */
        public virtual float Get()
        {
            return elapsed;
        }

        /**
         * Get the seconds the timer was set for.
         */
        public virtual float GetEndTime()
        {
            return end;
        }

        /**
         * Check whether the timer is done.
         */
        public virtual bool IsDone()
        {
            return (elapsed >= end) ? true : false;
        }

        /**
         * Call this method in order to advance the timer.
         * 
         * Returns true if the there was a tick or false
         * otherwise. A false also means the timer is done
         * and all event subscribers have been notified.
         */
        abstract public bool Tick();

        /**
         * Subscribe to the timer's end time event.
         */
        public void Subscribe(UnityAction<Timer> callback)
        {
            event_handler.AddListener(callback);
        }

        /**
         * Unsubscribe to the timer's end time event.
         */
        public void Unsubscribe(UnityAction<Timer> callback)
        {
            event_handler.RemoveListener(callback);
        }

        /**
         * Notifies all event subscribers.
         * 
         * This event handler should be called in the Tick() method.
         * All event subscrubers will be removed on the first call.
         */
        protected void OnTimeElapsedEventHandler()
        {
            if (event_handler != null)
            {
                event_handler.Invoke(this);
                event_handler = null;
            }
        }
    }
}
