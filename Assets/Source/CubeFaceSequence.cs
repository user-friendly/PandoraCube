using System.Collections.Generic;
using UnityEngine;


namespace PandoraCube
{
    public class CubeFaceSequence
    {
        // Initial sequence.
        protected List<GameObject> sequence = new List<GameObject>();

        // Sequence state.
        protected Queue<GameObject> state;

        static public CubeFaceSequence CreateFaceSequence(CubeFaceSet set, uint count)
        {
            CubeFaceSequence s = new CubeFaceSequence();

            List<GameObject> faces = set.GetFaces();

            if (faces.Count <= 0)
            {
                // TODO Throw exception instead?
                return null;
            }

            for (int i = 0; i < count; i++)
            {
                s.sequence.Add(faces[Random.Range(0, faces.Count)]);
            }

            s.state = new Queue<GameObject>(s.sequence);

            return s;
        }

        /**
         * Get a copy of the initial sequence.
         */
        public List<GameObject> GetSequence()
        {
            return new List<GameObject>(sequence);
        }

        /**
         * Attempt to activate the next cube face in the sequence.
         * 
         * Returns true if the current face in the sequence matches
         * and it is removed from the sequence's current state.
         * 
         * Returns false if the current face did not match. The
         * state is not modified. Note that if the sequence is
         * complete, i.e. no more faces to activate, false is
         * returned. Use IsComplete() to determine if the sequence
         * was complete.
         */
        public bool Activate(GameObject face)
        {
            if (state.Peek() == face)
            {
                // TODO Raise and event.
                state.Dequeue();
                return true;
            }

            return false;
        }

        /**
         * Check whether the sequence has been completed or not.
         */
        public bool IsComplete()
        {
            return state.Count == 0 ? true : false;
        }
    }
}