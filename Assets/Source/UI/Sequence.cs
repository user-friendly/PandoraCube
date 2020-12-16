using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace PandoraCube.UI
{

    public class Sequence : MonoBehaviour
    {
        public Color default_color = new Color();
        public Color active_color = new Color();
        public GameObject[] icons = new GameObject[3];
        public Sprite[] icon_set = new Sprite[6];

        // Start is called before the first frame update
        void Start()
        {
            //Image icon1 = icons[0].transform.Find("Thumbnail").GetComponent<Image>();
            //icon1.color = active_color;
            //icon1.sprite = icon_set[5];
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void OnCubeSequenceChanged(List<GameObject> sequence)
        {
            Image icon;
            for (int i = 0; i < 3; i++)
            {
                try
                {
                    if (sequence[i])
                    {
                        icon = icons[i].transform.Find("Thumbnail").GetComponent<Image>();
                        icon.color = i != 0 ? default_color : active_color;
                        icon.sprite = icon_set[sequence[i].GetComponent<CubeFace>().index];
                        // It's kind of redundant, but this is not performance critical.
                        if (!icons[i].activeSelf)
                        {
                            icons[i].SetActive(true);
                        }
                    }
                }
                catch (ArgumentOutOfRangeException e)
                {
                    // Out of range means hide.
                    icons[i].SetActive(false);
                }
            }
        }
    }
}
