namespace Save
{
    [System.Serializable]
    public struct SaveData
    {
        public string VERSION;
        public Test1Data monsterA;
        public Test1Data monsterB;
        //public GBNData gbnData;
    }

    [System.Serializable]
    public struct Test1Data
    {
        public int A;
    }
}