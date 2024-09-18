using UnityEngine;

namespace SaveModule
{
    /// <summary>
    /// MonoBehaviour that can be saved and loaded from a save file
    /// </summary>
    public class SaveBehaviour : MonoBehaviour
    {
        #region MonoBehaviour

        /// <summary>
        /// Awake is called when the script instance is being loaded.
        /// </summary>
        private void Awake()
        {
            this.OnPreAwake();

            // Fetch data from cache
            SaveData data = SaveManager.LoadFromCache();
            this.OnLoad(data);

            // Subscribe to events
            SaveManager.OnLoad += this.OnLoad;
            SaveManager.OnSave += this.OnSave;

            this.OnPostAwake();
        }

        /// <summary>
        /// This function is called when the MonoBehaviour will be destroyed.
        /// </summary>
        private void OnDestroy()
        {
            this.OnPreDestroy();

            // Unsubscribe to events
            SaveManager.OnLoad -= this.OnLoad;
            SaveManager.OnSave -= this.OnSave;

            // Send data to cache
            SaveData data = SaveManager.LoadFromCache();
            this.OnSave(ref data);
            SaveManager.SaveToCache(data);

            this.OnPostDestroy();
        }

        #endregion    

        #region Virtual

        /// <summary>
        /// Called before loading from the cache
        /// </summary>
        protected virtual void OnPreAwake() {}

        /// <summary>
        /// Called after loading from the cache
        /// </summary>
        protected virtual void OnPostAwake() {}

        /// <summary>
        /// Called before saving to the cache
        /// </summary>
        protected virtual void OnPreDestroy() {}

        /// <summary>
        /// Called after saving to the cache
        /// </summary>
        protected virtual void OnPostDestroy() {}

        /// <summary>
        /// Called when this object is being loaded
        /// </summary>
        /// <param name="data">Data loaded</param>
        protected virtual void OnLoad(SaveData data) {}

        /// <summary>
        /// Called when this object is being saved
        /// </summary>
        /// <param name="data">Data currently saved</param>
        protected virtual void OnSave(ref SaveData data) {}

        #endregion   
    }
}