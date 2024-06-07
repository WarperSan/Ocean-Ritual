using UnityEngine;

namespace Inventory
{
#nullable enable
    /// <summary>
    /// Class that defines an item in the database
    ///</summary>
    public class Item : MonoBehaviour
    {
        [HideInInspector]
        public string Namespace = "";

        #region Load

        /// <summary>Loads this item with the given data</summary>
        /// <returns>Success of the load</returns>
        public bool Load(ItemData data)
        {
            // If namespaces not matching
            if (this.Namespace != data.Namespace)
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
                return this.SetData(data);
            }
            catch (System.Exception e)
            {
                Debug.LogError($"Error while setting data: {e.Message}");
            }

            return false;
        }

        #endregion

        #region Virtual

        /// <returns>The given data is corrupted</returns>
        public virtual bool IsCorrupted(object? data) => data == null;

        /// <returns>Succeed to set the data</returns>
        protected virtual bool SetData(object? data) => true;

        #endregion
    }

    /// <summary>
    /// Class that converts extra data from an item into a given type
    ///</summary>
    public class Item<T> : Item where T : ItemData
    {
        #region Item

        /// <inheritdoc/>
        public sealed override bool IsCorrupted(object? data) 
            => data is T d ? this.IsCorrupted(d) : base.IsCorrupted(data);

        /// <inheritdoc/>
        protected override bool SetData(object? data) 
            => data is T d && this.SetData(d);

        #endregion

        #region Save

        /// <returns>Data to save for this item</returns>
        public T? Save()
        {
            this.OnSave();
            T? data = this.GetData();

            if (data == null)
                return null;

            data.Namespace = this.Namespace;

            return data;
        }

        /// <summary>Called when this item is being saved</summary>
        protected virtual void OnSave() { }

        #endregion

        #region Virtual

        /// <returns>The given data is corrupted</returns>
        protected virtual bool IsCorrupted(T? data) => false;

        /// <remarks>
        /// Try to limit the amount of data saved. If a value can be calculated, avoid saving it.
        /// </remarks>
        /// <returns>Data to save</returns>
        public virtual T? GetData() => null;

        /// <returns>Succeed to set the data</returns>
        protected virtual bool SetData(T? data) => true;

        #endregion
    }
}