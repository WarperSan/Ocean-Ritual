using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
 public class QuestData
{
     public List<Quest> quests;
    public QuestData(List<Quest> quests)
    {
        this.quests = quests;
    }
}
