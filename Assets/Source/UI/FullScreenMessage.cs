using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FullScreenMessage : MonoBehaviour
{
    public string win_message = "YOU WIN";
    public Color win_color = new Color();

    public string lose_message = "YOU LOSE";
    public Color lose_color = new Color();

    // Panel image.
    protected Image panel_image;

    // Text to be displayed.
    protected Text text;

    protected CanvasGroup group;

    static public FullScreenMessage instance
    {
        get { return GameObject.Find("FullScreenMessage").GetComponent<FullScreenMessage>(); }
    }

    protected void Start()
    {
        text = transform.Find("Text").GetComponent<Text>();
        panel_image = GetComponent<Image>();
        group = GetComponent<CanvasGroup>();
        
        Hide();
    }

    protected void Hide()
    {
        group.alpha = 0;
        group.blocksRaycasts = false;
    }

    protected void Show()
    {
        group.alpha = 1;
        group.blocksRaycasts = true;
    }

    public void OnPlayerWin()
    {

    }

    public void OnPlayerLose()
    {

    }
}
