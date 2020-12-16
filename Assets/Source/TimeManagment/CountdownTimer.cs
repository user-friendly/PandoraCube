using UnityEngine;

namespace PandoraCube.TimeManagment
{
    public class CountdownTimer : Timer
    {
        public CountdownTimer(float seconds)
            : base(0)
        {
            elapsed = seconds;
        }

        public override bool IsDone()
        {
            return (elapsed <= 0) ? true : false;
        }

        public override bool Tick()
        {
            if (elapsed > 0)
            {
                elapsed -= Time.fixedDeltaTime;
                return true;
            }
            else
            {
                OnTimeElapsedEventHandler();
            }
            return false;
        }
    }
}
