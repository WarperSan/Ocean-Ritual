using System.Collections.Generic;

namespace Inventory
{
    [System.Serializable]
    public class Inventory
    {
        public List<Slot> Slots = new();

        public void Clear() => this.Slots.Clear();
       
        public void Add(Inventory inventory)
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
}