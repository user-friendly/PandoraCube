using UnityEngine;

namespace PandoraCube.TimeManagment
{
    public class FrameTimer : Timer
    {
        public FrameTimer(float seconds)
            : base(seconds) { }

        //~FrameTimer()
        //{
        //    Debug.Log("FrameTimer: finilized: " + this.GetHashCode());
        //}

        public override bool Tick()
        {
            if (elapsed < end)
            {
                elapsed += Time.deltaTime;
                return true;
            }
            else
            {
                OnTimeElapsed();
            }
            return false;
        }
    }
}
