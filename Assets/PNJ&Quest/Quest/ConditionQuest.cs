using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
    public int quantite;

}
