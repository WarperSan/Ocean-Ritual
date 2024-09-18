using UnityEngine;

namespace UtilsModule
{
    public class Billboard : MonoBehaviour
    {
        void LateUpdate()
        {
            Camera main = Camera.main;

            if (main == null)
                return;

            Vector3 target = main.transform.position;
            target.y = this.transform.position.y;
            this.transform.LookAt(target);
        }
    }
}

