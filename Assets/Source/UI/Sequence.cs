using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace PandoraCube.UI
{

    public class Sequence : MonoBehaviour
    {
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
    }
}
