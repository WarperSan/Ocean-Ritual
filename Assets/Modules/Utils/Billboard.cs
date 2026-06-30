using UnityEngine;

namespace UtilsModule
{
    public class Billboard : MonoBehaviour
    {
        public Vector3 modifier = Vector3.one;

        private void LateUpdate()
        {
            Camera main = Camera.main;

            if (main == null)
                return;

            Vector3 target = main.transform.position;
            target.y = transform.position.y;

            transform.forward = Vector3.Scale(target - transform.position, modifier);
        }
    }
}