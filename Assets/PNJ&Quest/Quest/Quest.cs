using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Quest
{
    public int ID;
    public Reward reward;
    public string Name;
    public string questGiver;
    public string QuestConfirmer;
    public string Description;
    public List<Hint>? HintList;

    // Constructeur pour initialiser les membres de la classe Quest
    public Quest(int id, string name, string questGiver, string description, Reward reward, List<Hint>? HintList = null, string questConfirmer = null)
    {
        ID = id;
        Name = name;
        this.questGiver = questGiver;
        QuestConfirmer = questConfirmer ?? questGiver;
        Description = description;
        this.reward = reward;
        this.HintList = HintList;
    }
}
