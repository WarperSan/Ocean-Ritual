using UnityEngine;

namespace UtilsModule
{
    /// <summary>
    /// Class that allows any class to be accessed from anywhere
    /// </summary>
    /// <typeparam name="T">Type of the class</typeparam>
    public abstract class Singleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        /// <summary>
        /// Unique instance of <typeparamref name="T"/>
        /// </summary>
        public static T Instance;

        #region MonoBehaviour

        /// <inheritdoc cref="Awake" />
        private void Awake()
        {
            // Keep only one
            if (Instance != null)
            {
                Debug.LogWarning($"Another instance of {this.GetType().Name} has been found.");
                Destroy(this.gameObject);
                return;
            }

            Instance = this.gameObject.GetComponent<T>();

            if (!this.KeepParent)
                this.transform.SetParent(null);

            if (!this.DestroyOnLoad)
                DontDestroyOnLoad(this.gameObject);

            this.OnAwake();
        }

        #endregion MonoBehaviour

        #region Virtual

        /// <summary>
        /// Defines if the singleton should be destroyed when loading a new scene
        /// </summary>
        protected virtual bool DestroyOnLoad => false;

        /// <summary>
        /// Defines if the singleton should keep it's current parent or not
        /// </summary>
        protected virtual bool KeepParent => false;

        /// <summary>
        /// Called when <see cref="Awake"/> is called
        /// </summary>
        protected virtual void OnAwake() { }

        #endregion Virtual
    }
}