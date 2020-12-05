using UnityEngine;

namespace PandoraCube
{
    class CubeFace : MonoBehaviour
    {
        protected void Awake()
        {
            Debug.Log("CubeFace: Awake, id: " + GetInstanceID());
        }
    }
}
