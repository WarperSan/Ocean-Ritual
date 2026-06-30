using UnityEngine;

namespace EntityModule
{
    /// <summary>
    /// Equivalent of <see cref="ObjectPool"/>, but globally
    /// </summary>
    public class ObjectPoolGlobal : UtilsModule.Singleton<ObjectPool>
    {
        #region Object Pool

        [SerializeField]
        private ObjectPool.Setting[] Settings;

        /// <inheritdoc cref="ObjectPool.Get(string)"/>
        public static GameObject Get(string prefabName)
        {
            // If no object pool, skip
            if (Instance == null)
            {
                Debug.LogWarning($"Tried to get an object '{prefabName}' when no object pool was created.");
                return null;
            }

            return Instance.Get(prefabName);
        }

        /// <inheritdoc cref="ObjectPool.Apply(ObjectPool.Setting[])"/>
        public static void Apply(ObjectPool.Setting[] settings) => Instance.Apply(settings);

        #endregion

        #region Singleton

        /// <inheritdoc/>
        protected override bool DestroyOnLoad => true;

        /// <inheritdoc/>
        protected override void OnAwake() => Apply(Settings);

        #endregion
    }
}