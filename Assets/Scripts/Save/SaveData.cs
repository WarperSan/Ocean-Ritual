namespace Save
{
    [System.Serializable]
    public struct SaveData
    {
        public string VERSION;
        //public Inventory.Inventory Inventory;
        public Inventory.ItemData item;
    }
}