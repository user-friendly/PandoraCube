using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace PandoraCube.TimeManagment {
    public class TimeManager : MonoBehaviour
    {
        // Frame timers;
        protected List<FrameTimer> frame_timers = new List<FrameTimer>();

        // Fixed Countdown Timer
        protected List<CountdownTimer> cd_timer = new List<CountdownTimer>();

        protected List<Timer> finished_timers = new List<Timer>();

        protected void Update()
        {
            // Advance time for all timers.
            foreach (FrameTimer timer in frame_timers)
            {
                if (!timer.Tick())
                {
                    finished_timers.Add(timer);
                }
            }
            // Remove finished timers.
            if (finished_timers.Count > 0)
            {
                foreach (FrameTimer timer in finished_timers)
                {
                    frame_timers.Remove(timer);
                    // Debug.Log("TimeManager: Removed FrameTimer: " + timer.GetHashCode());
                }
                finished_timers.Clear();
            }
        }

        protected void FixedUpdate()
        {
            
        }

        /**
         * Create and subscrube to a FrameTimer.
         */
        public void CreateFrameTimer(float seconds, UnityAction<Timer> callback)
        {
            FrameTimer timer = new FrameTimer(seconds);
            timer.Subscribe(callback);
            frame_timers.Add(timer);

            // Debug.Log("TimeManager: New FrameTimer: " + timer.GetHashCode());
        }

        /**
         * Unsubscribe from all timers.
         * 
         * For a large number of timers, this is a very expensive method.
         */
        public void UnsubscribeAll(UnityAction<Timer> callback)
        {
            foreach (FrameTimer timer in frame_timers)
            {
                timer.Unsubscribe(callback);
            }
        }
    }
}