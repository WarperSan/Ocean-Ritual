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
            if (this.transform.parent != null)
            {
                this.name = this.transform.parent.name + " - " + this.name;
                this.transform.parent = null;
            }

            Destroy(this);
        }
    }
}