//using Fishing;
//using Inventory;

namespace Save
{
    [System.Serializable]
    public struct SaveData
    {
        public string VERSION;
      //  public Inventory<FishData> Fishes;
        public Test1Data monsterA;
        public Test1Data monsterB;
        public ListeGBNData gbnData;
        public int test;
    }

    [System.Serializable]
    public struct Test1Data
    {
        public int A;
    }
}