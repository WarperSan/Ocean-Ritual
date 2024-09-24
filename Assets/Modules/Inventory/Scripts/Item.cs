using UnityEngine;

namespace InventoryModule
{
    /// <summary>
    /// Class that defines an item in the database
    ///</summary>
    public abstract class Item : MonoBehaviour
    {
        /// <summary>
        /// Name used by the item to register
        /// </summary>
        [HideInInspector]
        public string Namespace = "";

        #region Load

        /// <summary>Loads this item with the given data</summary>
        /// <returns>Succeed to load</returns>
        public bool Load(ItemData data)
        {
            // If namespaces not matching
            if (this.Namespace.Equals(data.Namespace))
                return false;

            // If data is corrupted, skip
            if (this.IsCorrupted(data))
            {
                Debug.LogError("Given data is corrupted");
                return false;
            }

            try
            {
                // Set data
                this.SetData(data);
            }
            catch (System.Exception e)
            {
                Debug.LogError($"Error while setting data: {e.Message}");
                return false;
            }

            return true;
        }

        /// <summary>
        /// Checks if the given data is corrupted
        /// </summary>
        /// <returns>The given data is corrupted</returns>
        public virtual bool IsCorrupted(object data) => data == null;

        /// <summary>
        /// Sets the given data to this item
        /// </summary>
        protected virtual void SetData(object data) { }

        #endregion
    }

    /// <summary>
    /// Class that defines items with extra data attached to it
    ///</summary>
    public class Item<T> : Item where T : ItemData
    {
        #region Save

        /// <summary>
        /// Gathers the data to save for this item
        /// </summary>
        /// <returns>Data to save</returns>
        public T Save()
        {
            this.OnSave();
            T data = this.GetData();

            if (data == null)
                return null;

            // Overwrite namespace
            data.Namespace = this.Namespace;

            return data;
        }

        /// <summary>
        /// Fetches the data to save for this item
        /// </summary>
        /// <remarks>
        /// Try to limit the amount of data saved. If a value can be calculated, avoid saving it
        /// </remarks>
        /// <returns>Data to save</returns>
        protected virtual T GetData() => null;

        /// <summary>Called before this item is saved</summary>
        protected virtual void OnSave() { }

        #endregion

        #region Load

        /// <inheritdoc/>
        public sealed override bool IsCorrupted(object data)
        {
            // If wrong type, default
            if (data is not T d)
                return base.IsCorrupted(data);

            // Use own check
            return this.IsCorrupted(d);
        }

        /// <inheritdoc/>
        protected sealed override void SetData(object data)
        {
            // If wrong type, skip
            if (data is not T d)
                return;

            this.SetData(d);
        }

        /// <inheritdoc cref="Item.IsCorrupted(object)"/>
        protected virtual bool IsCorrupted(T data) => false;

        /// <inheritdoc cref="Item.SetData(object)"/>
        protected virtual void SetData(T data) {}

        #endregion
    }
}