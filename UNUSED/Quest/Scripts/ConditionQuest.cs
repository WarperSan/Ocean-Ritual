using System.Collections.Generic;
[System.Serializable]
public class ConditionQuest 
{
    public typeOfQuest typeOfTheQuest;
    public List<condition> condition;

}




[System.Serializable]
public class condition
{
    public string name;
    public int quantiteInProgress;
    public int quantite;

}
