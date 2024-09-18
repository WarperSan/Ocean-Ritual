namespace SaveModule
{
    [System.Serializable]
    public struct SaveData
    {
        public string VERSION;
        //public InventoryModule.Inventory<FishingModule.FishData> Fishes;
        public ListeGBNData gbnData;
        public int test;
    }
}