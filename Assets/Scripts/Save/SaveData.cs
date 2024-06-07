using Fishing;
using Inventory;

namespace Save
{
    [System.Serializable]
    public struct SaveData
    {
        public string VERSION;
        public Inventory<Fish, FishSOData> Fishes;
    }
}