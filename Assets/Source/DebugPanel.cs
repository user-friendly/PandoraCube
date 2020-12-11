using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace PandoraCube
{
    public class DebugPanel : MonoBehaviour
    {
        public Text fpsCounter;
        private float fpsCounterRefreshRate = 1f;
        private float fpsTimer = 0f;

        public Text faceSequence;

        protected void Awake()
        {
            
        }

        // Update is called once per frame
        protected void Update()
        {
            fpsTimer += Time.unscaledDeltaTime;
            if (fpsTimer >= fpsCounterRefreshRate)
            {
                // Averaged out values over ?several? frames.
                fpsCounter.text = string.Format("{0} FPS", (int)(1f / Time.smoothDeltaTime));
                fpsTimer = 0;
            }
        }

        public void OnCubeSequenceChanged(List<GameObject> sequence)
        {
            faceSequence.text = "Sequence: ";
            
            if (sequence == null || sequence.Count == 0)
            {
                return;
            }

            for (int i = 0; i < sequence.Count-1; i++)
            {
                faceSequence.text += sequence[i].GetComponent<CubeFace>().human_name + ", ";
            }
            faceSequence.text += sequence[sequence.Count - 1].GetComponent<CubeFace>().human_name;
        }
    }
}