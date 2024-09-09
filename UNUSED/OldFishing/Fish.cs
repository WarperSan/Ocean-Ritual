using UnityEngine;

namespace FishingModule
{
    /// <summary>
    /// Object that represents the informations for a fish
    /// </summary>
    public class Fish : InventoryModule.Item<FishData>
    {
        [Tooltip("Name to display when this fish is caught")]
        public string DisplayName;

        [Tooltip("Rarity of this fish")]
        public Rarity Rarity;

        [Tooltip("Weight of this fish")]
        public uint Weight;

        public uint Amount;

        protected override void SetData(FishData data) 
        {
            data ??= new FishData();

            this.Amount = data.Amount;
        }

        protected override FishData GetData() 
        {
            if (this.Amount == 0)
                return null;

            return new()
            {
                Amount = this.Amount,
            };
        }
    }

    [System.Serializable]
    public class FishData : InventoryModule.ItemStackData<FishData>
    {
        
    }
}
