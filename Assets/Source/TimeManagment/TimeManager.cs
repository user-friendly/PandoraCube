using System.Collections.Generic;
using UnityEngine;

namespace PandoraCube.TimeManagment {

    /**
     * TODO Need to rethink how timers are stored and ticks are handled,
     *      in a more uniform manner.
     */
    public class TimeManager : MonoBehaviour
    {
        // Frame step timers;
        protected List<Timer> frame_timers = new List<Timer>();
        // Fixed step timers.
        protected List<Timer> fixed_timers = new List<Timer>();

        // Cleanup timer lists.

        protected List<Timer> finished_timers_frame = new List<Timer>();
        protected List<Timer> finished_timers_fixed = new List<Timer>();

        protected void Update()
        {
            // Advance time for all timers.
            foreach (Timer timer in frame_timers)
            {
                if (!timer.Tick())
                {
                    finished_timers_frame.Add(timer);
                }
            }
        }

        protected void FixedUpdate()
        {
            foreach (Timer timer in fixed_timers)
            {
                if (!timer.Tick())
                {
                    finished_timers_fixed.Add(timer);
                }
            }
        }

        // TODO Is this the method to remove the timers?
        protected void LateUpdate()
        {
            // Remove finished timers.
            if (finished_timers_frame.Count > 0)
            {
                foreach (Timer timer in finished_timers_frame)
                {
                    frame_timers.Remove(timer);
                    // Debug.Log("TimeManager: Removed frame update timer: " + timer.GetHashCode());
                }
                finished_timers_frame.Clear();
            }
            if (finished_timers_fixed.Count > 0)
            {
                foreach (Timer timer in finished_timers_fixed)
                {
                    fixed_timers.Remove(timer);
                    // Debug.Log("TimeManager: Removed fixed update timer: " + timer.GetHashCode());
                }
                finished_timers_fixed.Clear();
            }
        }

        /**
         * Create a FrameTimer.
         */
        public FrameTimer CreateFrameTimer(float seconds)
        {
            FrameTimer timer = new FrameTimer(seconds);
            frame_timers.Add(timer);
            return timer;
            // Debug.Log("TimeManager: New FrameTimer: " + timer.GetHashCode());
        }

        public CountdownTimer CreateCountdownTimer(float seconds)
        {
            CountdownTimer timer = new CountdownTimer(seconds);
            fixed_timers.Add(timer);
            return timer;
        }
    }
}