using System.Collections.Generic;
[System.Serializable]
 public class QuestData
{
     public List<Quest> quests;
    public QuestData(List<Quest> quests)
    {
        this.quests = quests;
    }
}
