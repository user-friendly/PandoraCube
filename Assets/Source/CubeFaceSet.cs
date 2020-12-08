using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace PandoraCube
{
    using CubeFacePair = Tuple<Vector3, string>;
    using CubeFaceMap = Dictionary<Vector3, GameObject>;
    using CubeFaceMapPair = KeyValuePair<Vector3, GameObject>;

    [CreateAssetMenu(fileName = "Cube Face Set", menuName = "Pandora Cube/Cube Face Set")]
    public class CubeFaceSet : ScriptableObject
    {
        public const uint FACE_COUNT = 6;

        [HideInInspector]
        public Transform parent;

        public GameObject[] face_prototypes = new GameObject[FACE_COUNT];

        protected CubeFaceMap face_map = new CubeFaceMap();
        public CubeFaceMap faces => face_map;

        protected CubeFacePair[] face_layout = new CubeFacePair[] {
            new CubeFacePair(Vector3.forward, "CubeFace_Forward"),
            new CubeFacePair(Vector3.back, "CubeFace_Back"),
            new CubeFacePair(Vector3.up, "CubeFace_Up"),
            new CubeFacePair(Vector3.right, "CubeFace_Right"),
            new CubeFacePair(Vector3.down, "CubeFace_Down"),
            new CubeFacePair(Vector3.left, "CubeFace_Left")
        };

        protected void Awake()
        {
            Debug.Log("CubeFaceSet: Awake()");
        }

        public void Init(Transform parent)
        {
            this.parent = parent;

            Assert.IsNotNull(parent, "CubeFaceSet: There must be a parent transform");
            Assert.AreEqual(FACE_COUNT, face_prototypes.Length, "CubeFaceSet: There must be 6 face prototypes");

            for (int i = 0; i < face_prototypes.Length; i++)
            {
                GameObject new_face = Instantiate(face_prototypes[i], parent);
                new_face.name = face_layout[i].Item2;

                CubeFace new_cubeface = (CubeFace)new_face.AddComponent(typeof(CubeFace));
                new_cubeface.direction = face_layout[i].Item1;

                face_map[face_layout[i].Item1] = new_face;
            }
        }

        /**
         * Get the face facing the relative axis.
         * 
         * Note that each face's forward axis is up.
         */
        public GameObject GetForwardFacing(Vector3 relative)
        {
            GameObject face = null;
            foreach (CubeFaceMapPair pair in face_map)
            {
                face = pair.Value;
                if (0.0f == Mathf.Round(Vector3.Angle(face.transform.rotation * Vector3.up, relative)))
                {
                    return face;
                }
            }
            return null;
        }
    }
}
