using System.Collections.Generic;
using UnityEngine;

namespace Inventory
{
    [System.Serializable]
    public class InventoryA
    {
        public List<Slot> Slots = new();

        public void Clear() => this.Slots.Clear();

        public void Add(InventoryA inventory)
        {
            foreach (Slot item in inventory.Slots)
                this.Add(item.ItemID, item.Quantity);
        }
        public void Add(object item, uint quantity = 1) => this.Add(item.GetHashCode(), quantity);
        private void Add(int itemId, uint quantity)
        {
            bool wasAdded = false;

            // Check every slots
            for (int i = 0; i < this.Slots.Count; i++)
            {
                Slot slot = this.Slots[i];

                // If slot not item, skip
                if (slot.ItemID != itemId)
                    continue;

                // Increase quantity
                slot.Quantity += quantity;
                this.Slots[i] = slot;

                wasAdded = true;
            }

            // If already added, skip
            if (wasAdded)
                return;

            this.Slots.Add(new Slot()
            {
                ItemID = itemId,
                Quantity = quantity
            });
        }
    }

    [System.Serializable]
    public struct Slot
    {
        public int ItemID;
        public uint Quantity;
    }

    [System.Serializable]
    public class Inventory<T, U> : ISerializationCallbackReceiver where T : Item<U> where U : ItemData
    {
        [System.NonSerialized]
        public List<(T asset, U data)> Data = new();

        #region Add

        public void Add(T item) 
        {
            this.Data.Add((item, item.Save()));
        }

        #endregion

        #region ISerializationCallbackReceiver

        [SerializeField, HideInInspector]
        private U[] Items;

        /// <inheritdoc/>
        public void OnBeforeSerialize() 
        {
            this.Items = new U[this.Data.Count];

            for (int i = 0; i < this.Items.Length; i++)
                this.Items[i] = this.Data[i].data;
        }

        /// <inheritdoc/>
        public void OnAfterDeserialize() 
        {
            this.Data.Clear();
            foreach (U data in this.Items)
            {
                // If item invalid, skip
                if (!Registry.GetItem(data, out T item))
                    continue;

                // If data corrupted, skip
                if (item.IsCorrupted(data) || data is not U correctData)
                    continue;

                this.Data.Add((item, correctData));
            }
        }

        #endregion
    }
}