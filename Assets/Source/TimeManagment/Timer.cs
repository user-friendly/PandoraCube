namespace PandoraCube.TimeManagment
{
    /**
     * I would like to hide the Tick() method and make it only available
     * in the TimeManager class. Perhaps there is some OOP design patter
     * involving multiple interfaces, but this will have to wait.
     */

    public delegate void TimeElapsed<T>(T timer);

    abstract public class Timer
    {
        protected float elapsed = 0.0f;
        protected float end = 0.0f;

        public event TimeElapsed<Timer> onTimeElapsed;

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
         * Notifies all event subscribers.
         * 
         * This event handler should be called in the Tick() method.
         * All event subscribers will be removed on the first call.
         */
        protected void OnTimeElapsed()
        {
            onTimeElapsed?.Invoke(this);
        }
    }
}
