using UnityEngine.Events;

namespace PandoraCube.TimeManagment
{
    interface ITimer
    {
        /**
         * Set the timer to go off after seconds.
         */
        void Set(float seconds);

        /**
         * Get the elapsed time.
         * 
         * The elapsed time will not significantly increase past the
         * end time.
         */
        float Get();

        /**
         * Get the seconds the timer was set for.
         */
        float GetEndTime();

        /**
         * Subscribe to the timer's end time event.
         */
        void Subscribe(UnityAction call);
    }
}
