using UnityEngine;

namespace PandoraCube
{
    class CubeFace : MonoBehaviour
    {
        // Offset faces, relative to cube's center.
        protected float offset = 1.05f;

        // Initial rotation.
        //protected Quaternion q_original;

        protected void Awake()
        {
            Debug.Log("CubeFace: Awake, id: " + GetInstanceID());
        }

        /**
         * Set face's variant.
         * 
         * Variants are based on the vector direction's orientation.
         * There really should be only 6 variants - cube's sides.
         */
        public void SetFaceVariant(Vector3 direction)
        {
            direction.Normalize();

            Debug.Log("CubeFace: set variant to: " + direction + ", dot: " + (int)Vector3.Dot(Vector3.forward, direction));

            if (1 != Mathf.Abs((int) Vector3.Dot(Vector3.forward, direction)))
            {
                transform.localRotation = Quaternion.LookRotation(Vector3.forward, direction);
            }
            else
            {
                transform.localRotation = Quaternion.LookRotation(Vector3.up, direction);
            }

            
            transform.localPosition = (direction * offset);
        }
    }
}
