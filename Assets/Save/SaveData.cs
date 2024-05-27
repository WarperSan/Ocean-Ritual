namespace Save
{
    [System.Serializable]
    public struct SaveData
    {
        public string VERSION;
        public Test1Data monsterA;
        public Test1Data monsterB;
    }

    [System.Serializable]
    public struct Test1Data
    {
        public int A;
    }
}