using Extensions;
using UnityEditor;
using UnityEngine;

namespace Inventory
{
    /// <summary>
    /// Class that defines an item in the database
    ///</summary>
    public class Item : MonoBehaviour
    {
        [Header("Item")]
        #region Namespace
        public string Namespace;

#if UNITY_EDITOR
        /// <inheritdoc/>
        private void Reset() => this.UpdateNamespace();

        /// <inheritdoc/>
        private void OnValidate() => this.UpdateNamespace();

        /// <summary>
        /// Updates the namespace of this item
        /// </summary>
        private void UpdateNamespace()
        {
            string newNamespace = GenerateNamespace(this);

            if (newNamespace.IndexOf(".") == -1)
                return;

            bool wasDefined = true;

            if (string.IsNullOrEmpty(this.Namespace))
            {
                this.Namespace = "undefined";
                wasDefined = false;
            }

            // Skip if namespaces are the same
            if (this.Namespace.Equals(newNamespace))
                return;

            if (Application.isBatchMode)
                return;

            bool updateName = !wasDefined || EditorUtility.DisplayDialog(
                "Replace namespace?",
                $"{this.name} has been modified and it's namespace changed.\nDo you want to update it?\n\n" +
                this.Namespace + "\nV\n" + newNamespace,
                "Update",
                "Cancel"
            );

            // If don't update
            if (!updateName)
                return;

            this.Namespace = newNamespace;
        }

        /// <summary>
        /// Generates the namespace for the given item
        /// </summary>
        private static string GenerateNamespace(Item item)
        {
            string path = AssetDatabase.GetAssetPath(item);
            path = path.Replace("Resources/", ""); // Remove Resources
            path = path.Replace("Items/", ""); // Remove Items
            path = path.Split(".")[0]; // Remove extension

            string[] folders = path.Split("/");
            folders[0] = Application.productName; // Application name as first

            return folders.Join(".").ToLower().Replace(" ", "_");
        }
#endif
        #endregion

        #region Save/Load

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

        /// <summary>Called when this item is being saved</summary>
        protected virtual void OnSave() { }

        /// <summary>Get the extra data of this item</summary>
        protected virtual object GetExtra() => default;

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
            T data = JsonUtility.FromJson<T>(json);

            // If data not valid, skip
            if (!this.IsValid(data))
                return false;

            // Set data
            return this.SetData(data);
        }

        #endregion

        #region Virtual

        protected virtual bool IsValid(T data) => true;

        protected virtual T GetData() => default;
        protected virtual bool SetData(T data) => true;

        #endregion
    }
}