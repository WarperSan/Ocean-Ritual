using UnityEngine;

namespace UtilsModule
{
    public class Billboard : MonoBehaviour
    {
        public Vector3 modifier = Vector3.one;

        void LateUpdate()
        {
            Camera main = Camera.main;

            if (main == null)
                return;

            Vector3 target = main.transform.position;
            target.y = this.transform.position.y;

            this.transform.forward = Vector3.Scale(target - this.transform.position, this.modifier);
        }
    }
}

