using UnityEngine;

namespace Inventory
{
    /// <summary>
    /// Class that defines an item in the database
    ///</summary>
    public class Item : MonoBehaviour
    {
        [HideInInspector]
        public string Namespace;

        #region Save

        /// <returns>Data to save for this item</returns>
        public ItemData Save()
        {
            this.OnSave();

            return new()
            {
                Namespace = this.Namespace,
                ExtraData = JsonUtility.ToJson(this.GetExtra())
            };
        }

         /// <summary>Called when this item is being saved</summary>
        protected virtual void OnSave() { }

        /// <summary>Get the extra data of this item</summary>
        protected virtual object GetExtra() => default;

        #endregion

        #region Load

        /// <summary>Loads this item with the given data</summary>
        /// <returns>Success of the load</returns>
        public bool Load(ItemData data)
        {
            // If namespaces not matching
            if (this.Namespace != data.Namespace)
                return false;

            // Parse extra data
            return this.ParseJson(data.ExtraData);
        }

        /// <summary>Parse the given extra data</summary>
        /// <returns>Given extra data is valid</returns>
        protected virtual bool ParseJson(string json) => true;

        #endregion
    }

    /// <summary>
    /// Class that converts extra data from an item into a given type
    ///</summary>
    public class Item<T> : Item
    {
        #region Item

        /// <inheritdoc/>
        protected sealed override object GetExtra() => this.GetData();

        /// <inheritdoc/>
        protected sealed override bool ParseJson(string json)
        {
            try
            {
                T data = FromJson(json);

                // If data is corrupted, skip
                if (this.IsCorrupted(data))
                    throw new System.ArgumentException("Given data is corrupted");

                // Set data
                return this.SetData(data);
            }
            catch (System.Exception e)
            {   
                Debug.LogError($"Error while parsing extra data to '{typeof(T).Name}': {e.Message}");
            }

            return false;
        }

        public static T FromJson(string json) => JsonUtility.FromJson<T>(json);

        #endregion

        #region Virtual

        /// <returns>The given data is corrupted</returns>
        protected virtual bool IsCorrupted(T data) => false;

        /// <remarks>
        /// Try to limit the amount of data saved. If a value can be calculated, avoid saving it.
        /// </remarks>
        /// <returns>Data to save</returns>
        protected virtual T GetData() => default;
        
        /// <returns>Succeed to set the data</returns>
        protected virtual bool SetData(T data) => true;

        #endregion
    }
}