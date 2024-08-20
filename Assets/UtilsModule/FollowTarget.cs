using UnityEngine;

namespace UtilsModule
{
    public class FollowTarget : MonoBehaviour
    {
        [SerializeField]
        private Transform target;

        [SerializeField]
        private Vector3 axis;

        [SerializeField]
        private Vector3 offset;

        // Update is called once per frame
        void Update()
        {
            if (this.target == null)
                return;

            Vector3 pos = this.target.position;

            pos = Vector3.Scale(pos, this.axis);
            pos += this.offset;

            this.transform.position = pos;
        }
    }
}