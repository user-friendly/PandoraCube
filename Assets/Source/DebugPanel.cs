using UnityEngine;
using UnityEngine.UI;

public class DebugPanel : MonoBehaviour
{
    public Text fpsCounter;
    private float fpsCounterRefreshRate = 1f;
    private float fpsTimer = 0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        fpsTimer += Time.unscaledDeltaTime;
        if (fpsTimer >= fpsCounterRefreshRate)
        {
            // Averaged out values over ?several? frames.
            fpsCounter.text = string.Format("{0} FPS", (int) (1f / Time.smoothDeltaTime));
            fpsTimer = 0;
        }
    }
}
