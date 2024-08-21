using System.Collections.Generic;

[System.Serializable]
public class Quest
{
    public int ID;
    public ConditionQuest ConditionQuest;
    public bool QuestComplet;
    public Reward reward;
    public string Name;
    public string questGiver;
    public string QuestConfirmer;
    public string Description;
    public List<Hint>? HintList;

    // Constructeur pour initialiser les membres de la classe Quest
    public Quest(int id, string name, string questGiver, string description, ConditionQuest ConditionQuest ,Reward reward,
        List<Hint>? HintList = null, string questConfirmer = null, bool QuestComplett = false)
    {
        ID = id;
        QuestComplet = QuestComplett;
        this.ConditionQuest = ConditionQuest;
        Name = name;
        this.questGiver = questGiver;
        QuestConfirmer = questConfirmer ?? questGiver;
        Description = description;
        this.reward = reward;
        this.HintList = HintList;
    }
}
