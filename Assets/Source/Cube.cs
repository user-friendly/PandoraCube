using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

namespace PandoraCube
{
    // TODO This class is becoming quite heavy. Consider refactoring.


    public class Cube : MonoBehaviour
    {
        public PandoraCube game_app;
        protected GameObject debug_axis;

        // Main camera.
        public Camera cam = null;

        [InspectorName("Rotation speed")]
        public float r_speed = 3f;
        // Stores Lerp's time.
        private float r_time = 0.0f;
        // Set rotation angle.
        private const float r_angle = 90.0f;
        // Whether rotation, in any direction, is active.
        private bool r_active = false;
        // Original rotation.
        private Quaternion r_original;
        // Initial rotation.
        private Quaternion r_start;
        // Desired rotation.
        private Quaternion r_end;
        // Debug timer.
        private float r_debug_timer = 0.0f;

        protected Dictionary<Vector3, GameObject> faces = new Dictionary<Vector3, GameObject>(6);

        public CubeFaceSet face_set_asset;
        protected CubeFaceSet face_set;

        public uint sequence_length = 6;
        protected CubeFaceSequence sequence;

        public UnityEvent<GameObject> OnActivated;
        public UnityEvent<GameObject> OnActivatedFailure;
        public UnityEvent<GameObject> OnActivatedSuccess;
        public UnityEvent<List<GameObject>> OnSequenceChanged;

        static public Cube instance
        {
            get { return GameObject.Find("Cube").GetComponent<Cube>(); }
        }

        protected void Awake()
        {
            // Debug.Log("Cube: Awake, id: " + GetInstanceID());
            // Debug.Log("Cube: Face count for cube: " + GetComponent<MeshFilter>().mesh.triangles.Length);

            // TODO This does not look right.
            face_set = Instantiate(face_set_asset);
            face_set.Init(transform);

            GenerateNewSequence();
        }

        // Start is called before the first frame update
        protected void Start()
        {
            // Align cube with main camera rotation and save it, in case
            // the camera changes rotation (e.g. due to animation).
            r_original = cam.transform.rotation;
            // Flip the Z axis.
            r_original = r_original * Quaternion.AngleAxis(180.0f, Vector3.up);
            transform.rotation = r_original;

            if (OnActivated == null)
            {
                OnActivated = new UnityEvent<GameObject>();
            }
            if (OnActivatedFailure == null)
            {
                OnActivatedFailure = new UnityEvent<GameObject>();
            }
            if (OnActivatedSuccess == null)
            {
                OnActivatedSuccess = new UnityEvent<GameObject>();
            }
            if (OnSequenceChanged == null)
            {
                OnSequenceChanged = new UnityEvent<List<GameObject>>();
            }

            // TODO Make conditional on debug buid.
            GameObject helper_axis = game_app.CreateAxisGizmo();
            helper_axis.name = "Cube Helper Game Axis";
            helper_axis.transform.localScale = Vector3.one * 0.05f;
            helper_axis.transform.Translate(new Vector3(-0.203f, 0.644f, 1.349f), Space.World);
            helper_axis.transform.localRotation = r_original;
            debug_axis = helper_axis;
        }

        // Update is called once per frame
        protected void Update()
        {
            UpdateRotation();
            // TODO Create a proper test case out of this snippet.
            //else
            //{
            //    Vector2 dir = new Vector2();
            //    if (Random.Range(0, 2) == 1)
            //    {
            //        dir.x = (Random.Range(0, 2) == 1) ? 1 : -1;
            //    }
            //    else
            //    {
            //        dir.y = (Random.Range(0, 2) == 1) ? 1 : -1;
            //    }
            //    OnUIButtonRotate(dir);
            //}
        }

        protected void UpdateRotation()
        {
            if (r_active != true)
            {
                return;
            }

            r_time += Time.deltaTime / r_speed;

            transform.localRotation = Quaternion.Lerp(r_start, r_end, r_time);

            if (r_time >= 1.0f)
            {
                // Final lerp step.
                transform.localRotation = r_end;
                r_active = false;
                r_time = 0.0f;

                r_debug_timer = Time.time - r_debug_timer;
                // Debug.Log("Rotation animation time: " + TimeSpan.FromSeconds(r_debug_timer).ToString(@"mm\:ss\.ffff"));
            }

            // TODO Only in debug mode?
            UpdateDebugAxis(transform.localRotation);
        }

        /**
         * Get the current parallel axis to the cube's original orientation.
         * 
         * The argument extracts the orientation around the given axis and compares
         * to the cube's current orientation. Note that the relative vector must be
         * a unit vector having exactly one dimension set.
         * 
         * @param relative The unit vector to compare against. Exactly one dimension
         *                 should be the unit (e.g. Vector3.up/right/down).
         * 
         * @return Returns the unit vector (Vector3.up, Vector3.right or Vector3.forward)
         *         parallel to the original orientation's relative vector. Otherwise
         *         returns the zero vector.
         */
        protected Vector3 GetParallelAxis(Vector3 relative)
        {
            //Debug.Log("Cube.up angle Cam.relative: " + Vector3.Angle(transform.rotation * Vector3.up, r_original * relative));
            //Debug.Log("Cube.right angle Cam.relative: " + Vector3.Angle(transform.rotation * Vector3.right, r_original * relative));
            //Debug.Log("Cube.forward angle Cam.relative: " + Vector3.Angle(transform.rotation * Vector3.forward, r_original * relative));

            if (90.0f != Mathf.Round(Vector3.Angle(transform.rotation * Vector3.up, r_original * relative)))
            {
                return Vector3.up;
            }
            else if (90.0f != Mathf.Round(Vector3.Angle(transform.rotation * Vector3.right, r_original * relative)))
            {
                return Vector3.right;
            }
            else if (90.0f != Mathf.Round(Vector3.Angle(transform.rotation * Vector3.forward, r_original * relative)))
            {
                return Vector3.forward;
            }

            return Vector3.zero;
        }

        protected void StartRotation(Vector2 direction)
        {
            if (r_active)
            {
                return;
            }

            r_debug_timer = Time.time;

            // Normalize, just in case.
            direction.Normalize();
            // TODO Direction determination can probably be done better.
            int angle_direction = 1;
            if (direction.x < 0 || direction.y < 0)
            {
                angle_direction = -1;
            }

            Vector3 axis = GetParallelAxis(
                // We have to pick the opposite axis of rotation, because direction is the
                // vector of the desired rotation. In other words, the user wants to rotate
                // vertically so direction will have a non-zero +/- y vector component,
                // but the below algorithm should rotate around the x vector.
                Quaternion.AngleAxis(90.0f, Vector3.forward) * direction
                );

            // Could not find an axis to rotate around.
            if (axis == Vector3.zero)
            {
                Debug.LogError("Cube: Failed to find parallel axis to rotate around.");
                return;
            }

            r_active = true;
            r_time = 0.0f;

            r_start = transform.localRotation;
            r_end = transform.localRotation * Quaternion.AngleAxis(angle_direction * r_angle, axis);
        }

        protected void GenerateNewSequence()
        {
            sequence = CubeFaceSequence.CreateFaceSequence(face_set, sequence_length);

            OnSequenceChanged.Invoke(sequence.Get());

            string debug_sequence = "";
            foreach (GameObject face in sequence.Get())
            {
                debug_sequence += face.name + ", ";
            }
            Debug.Log("Cube: New sequence generated: " + debug_sequence);

        }

        // Player action event handlers.

        public void OnPlayerAction_Activate()
        {
            Debug.Log("OnButtonAction: action activated");

            GameObject face = face_set.GetForwardFacing(r_original * Vector3.forward);
            if (face)
            {
                if (sequence.Activate(face))
                {
                    Debug.Log("Cube: Successfully activated: " + face.name);
                    face.GetComponent<CubeFace>().DummyActivateFace();

                    OnSequenceChanged.Invoke(sequence.Get());
                }

                if (sequence.IsComplete())
                {
                    Debug.Log("Cube: Sequence successfully completed.");
                    GenerateNewSequence();
                }
            }
            else
            {
                Debug.Log("Cube: NO active face");
            }
        }

        public void OnPlayerAction_Reset()
        {
            Debug.Log("OnPlayerAction_Reset: reset all states");

            // TODO more to come.

            transform.localRotation = r_start = r_end = r_original;
            r_active = false;
            r_time = 0f;

            // TODO Only in debug mode?
            UpdateDebugAxis(transform.localRotation);
        }

        public void OnPlayerAction_Rotate(Vector2 direction)
        {
            StartRotation(direction);
        }

        // Input System event handlers have the following naming scheme
        // OnInputSystem_[action map]_[action]
        // Generally, they should delegate the event to the OnPlayerAction_[action]
        // handlers above.

        public void OnInputSystem_Player_Rotate(InputAction.CallbackContext context)
        {
            if (!context.performed)
            {
                return;
            }

            Vector2 direction = context.ReadValue<Vector2>();

            //Debug.Log("InputSystem: Rotate action fired, value: " + direction);

            OnPlayerAction_Rotate(direction);
        }

        public void OnInputSystem_Player_Activate(InputAction.CallbackContext context)
        {
            if (!context.performed)
            {
                return;
            }

            OnPlayerAction_Activate();
        }

        public void OnInputSystem_Player_Reset(InputAction.CallbackContext context)
        {
            if (!context.performed)
            {
                return;
            }

            OnPlayerAction_Reset();
        }

        protected void UpdateDebugAxis(Quaternion new_rotation)
        {
            debug_axis.transform.localRotation = new_rotation;
        }
    }
}