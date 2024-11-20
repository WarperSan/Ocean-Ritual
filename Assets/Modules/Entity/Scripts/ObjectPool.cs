using System;
using System.Collections.Generic;
using UnityEngine;

namespace EntityModule
{
    /// <summary>
    /// Class that manages the recycling of objects
    /// </summary>
    public class ObjectPool : MonoBehaviour
    {
        [SerializeField]
        private Setting[] Settings;

        /// <inheritdoc/>
        private void Start() => this.Apply(this.Settings);

        public void Apply(Setting[] settings)
        {
            this.Settings = settings;

            // Create all settings
            foreach (Setting setting in settings)
                this.CreateSetting(setting);
        }

        #region Fetching

        /// <summary>
        /// Tries to get an instance of the prefab with the given name
        /// </summary>
        /// <returns>Instance found or null if an error occurred</returns>
        public GameObject Get(string prefabName)
        {
            // If not cached, skip
            if (!cachedSettings.TryGetValue(prefabName, out Setting setting))
            {
                Debug.LogWarning($"The prefab '{prefabName}' was not pooled.");
                return null;
            }

            GameObject obj = null;

            // Find the first instance of a disabled object
            foreach (Transform child in setting.parent)
            {
                if (child.gameObject.activeInHierarchy)
                    continue;

                obj = child.gameObject;
                break;
            }

            // If no object found, create one
            if (obj == null)
            {
                Debug.LogWarning($"Consider increasing the initial amount of '{prefabName}'.");
                obj = this.CreateObject(setting);
            }

            return obj;
        }

        public void DisableAll(string prefabName)
        {
            // If not cached, skip
            if (!cachedSettings.TryGetValue(prefabName, out Setting setting))
            {
                Debug.LogWarning($"The prefab '{prefabName}' was not pooled.");
                return;
            }

            // Disable all instances
            foreach (Transform child in setting.parent)
                child.gameObject.SetActive(false);
        }

        #endregion

        #region Creation

        private readonly Dictionary<string, Setting> cachedSettings = new();

        /// <summary>
        /// Creates the given setting
        /// </summary>
        private void CreateSetting(Setting setting)
        {
            // Create the prefab parent
            var newSetting = new GameObject()
            {
                name = "[ " + setting.Prefab.name + " ]",
            };

            newSetting.transform.parent = this.transform;
            setting.parent = newSetting.transform;
            this.cachedSettings[setting.Prefab.name] = setting;

            // Create every prefab
            for (int i = 0; i < setting.amount; i++)
                this.CreateObject(setting);
        }

        /// <summary>
        /// Creates a new instance of the given <see cref="GameObject"/>
        /// </summary>
        /// <returns><see cref="GameObject"/> created</returns>
        private GameObject CreateObject(Setting setting)
        {
            // If prefab invalid, skip
            if (setting.Prefab == null)
                return null;

            // Create object
            GameObject newObj = Instantiate(setting.Prefab, setting.parent);
            newObj.SetActive(false);

#if UNITY_EDITOR
            this.IncreaseEvaluate(setting.Prefab.name);
#endif

            return newObj;
        }

        #endregion

        #region Typedef

        /// <summary>
        /// Defines how this object pool should initializes
        /// </summary>
        [Serializable]
        public class Setting
        {
            [Tooltip("GameObject to spawn")]
            public GameObject Prefab;

            [Min(0), Tooltip("Amount of objects to create at the start")]
            public uint amount;

#if UNITY_EDITOR
            [Tooltip("Amount of objects spawned since the start of the pool")]
            public uint spawned;
#endif

            [HideInInspector]
            public Transform parent;
        }

        #endregion

        #region Evaluation
#if UNITY_EDITOR
        /// <summary>
        /// Increases the counter for <paramref name="prefabName"/>
        /// </summary>
        /// <param name="prefabName">Name of the prefab</param>
        private void IncreaseEvaluate(string prefabName)
        {
            // Skip if not cached
            if (!this.cachedSettings.ContainsKey(prefabName))
                return;

            Setting setting = this.cachedSettings[prefabName];
            setting.spawned++;
            this.cachedSettings[prefabName] = setting;
        }
#endif
        #endregion
    }
}