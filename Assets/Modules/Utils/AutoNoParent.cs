using UnityEngine;

namespace UtilsModule
{
    /// <summary>
    /// Automatically puts this object at the root of the scene
    /// </summary>
    public class AutoNoParent : MonoBehaviour
    {
        /// <inheritdoc/>
        private void Awake()
        {
            if (transform.parent != null)
            {
                name = transform.parent.name + " - " + name;
                transform.parent = null;
            }

            Destroy(this);
        }
    }
}