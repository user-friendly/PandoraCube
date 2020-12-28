using UnityEngine;
using UnityEngine.UI;
using PandoraCube.TimeManagment;

namespace PandoraCube
{
    using CDTimer = TimeManagment.CountdownTimer;

    public class CountdownTimer : MonoBehaviour
    {
        public Text text;
        public float initial_seconds = 10.0f;
        protected float cur_seconds = 0.0f;

        protected CDTimer timer;

        // Start is called before the first frame update
        void Start()
        {
            timer = PandoraCube.instance.GetComponent<TimeManager>()
                .CreateCountdownTimer(initial_seconds);
        }

        // Update is called once per frame
        void Update()
        {
            cur_seconds = timer.Get();
            text.text = string.Format("{0:d2}:{1:d2}", (int)(cur_seconds / 60.0f), (int)(cur_seconds % 60.0f));
        }
    }
}
