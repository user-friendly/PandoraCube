using UnityEngine;

namespace PandoraCube
{
    class CubeFace : MonoBehaviour
    {
        [HideInInspector]
        public Vector3 direction;

        // TODO This is not ideal.
        // Offset faces, relative to cube's center.
        protected const float offset = 1.05f;

        // Initial rotation.
        //protected Quaternion q_original;

        protected void Awake()
        {
            Debug.Log("CubeFace: Awake, id: " + GetInstanceID() + ", gameObject: " + gameObject.name);
        }

        protected void Start()
        {
            //Debug.Log("CubeFace: Start()");

            if (direction != null)
            {
                SetFaceDirection(direction);
            }
        }

        /**
         * Set face's variant.
         * 
         * Variants are based on the vector direction's orientation.
         * There really should be only 6 variants - cube's sides.
         */
        public void SetFaceDirection(Vector3 direction)
        {
            direction.Normalize();
            this.direction = direction;

            //Debug.Log("CubeFace: set variant to: " + direction + ", dot: " + (int)Vector3.Dot(Vector3.forward, direction));

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
