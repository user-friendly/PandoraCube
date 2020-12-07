using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace PandoraCube
{
    using CubeFaceTuple = Tuple<Vector3, string, GameObject>;

    [CreateAssetMenu(fileName = "Cube Face Set", menuName = "Pandora Cube/Cube Face Set")]
    public class CubeFaceSet : ScriptableObject
    {
        public const uint FACE_COUNT = 6;

        public GameObject[] face_prototypes = new GameObject[FACE_COUNT];

        protected GameObject[] faces = new GameObject[FACE_COUNT];

        protected CubeFaceTuple[] face_layout = new CubeFaceTuple[] {
            new CubeFaceTuple(Vector3.forward, "CubeFace_Forward", null),
            new CubeFaceTuple(Vector3.back, "CubeFace_Back", null),
            new CubeFaceTuple(Vector3.up, "CubeFace_Up", null),
            new CubeFaceTuple(Vector3.right, "CubeFace_Right", null),
            new CubeFaceTuple(Vector3.down, "CubeFace_Down", null),
            new CubeFaceTuple(Vector3.left, "CubeFace_Left", null)
        };

        // TODO This is not ideal.
        // Offset faces, relative to cube's center.
        protected float offset = 1.05f;

        protected GameObject parent;

        protected void Awake()
        {
            Assert.AreEqual(FACE_COUNT, face_prototypes.Length, "CubeFaceSet: There must be 6 face prototpyes");

            for (int i = 0; i < face_layout.Length; i++)
            {
                var new_face = (
                    direction: face_layout[i].Item1,
                    name: face_layout[i].Item2,
                    face: Instantiate(face_prototypes[i])
                    );



                face_layout[i] = new CubeFaceTuple(new_face.direction, new_face.name, new_face.face);
            }
        }

        public void AttachTo(GameObject parent)
        {
            this.parent = parent;

            foreach (GameObject face in faces)
            {
                face.transform.parent = parent.transform;
            }
        }

        protected void SetFaceDirection(Vector3 direction, GameObject face)
        {
            direction.Normalize();

            Debug.Log("CubeFace: set variant to: " + direction + ", dot: " + (int)Vector3.Dot(Vector3.forward, direction));

            if (1 != Mathf.Abs((int) Vector3.Dot(Vector3.forward, direction)))
            {
                face.transform.localRotation = Quaternion.LookRotation(Vector3.forward, direction);
            }
            else
            {
                face.transform.localRotation = Quaternion.LookRotation(Vector3.up, direction);
            }

            face.transform.localPosition = (direction * offset);
        }
    }
}
