using UnityEngine;

namespace UtilsModule
{
    public class FollowTarget : MonoBehaviour
    {
        public Transform target;

        [SerializeField]
        private Vector3 axis;

        [SerializeField]
        private Vector3 offset;

        // Update is called once per frame
        private void Update()
        {
            if (target == null)
                return;

            Vector3 pos = target.position;

            pos = Vector3.Scale(pos, axis);
            pos += offset;

            transform.position = pos;
        }
    }
}