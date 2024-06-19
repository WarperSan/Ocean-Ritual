using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestGenerator : MonoBehaviour
{
    [SerializeField] List< Quest> AddingQuests;
    [SerializeField] public List<Quest> ModifingExistantQuest;
    [SerializeField] List<ChangeID> ChangeIDQuests;
    // Start is called before the first frame update
    void Start()
    {
        applyModif();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void applyModif()
    {
       QuestManager.GetDataQuestLoad();
        AddingQuest();
        ModifiQuest();
            ChangeIDQuest();
        QuestManager.QuestToDataSave();
    }
    public void AddingQuest()
    {
        foreach(Quest quest in AddingQuests)
        {
            QuestManager.AddQuest(quest);
        }
    }

    public void ModifiQuest()
    {

        foreach(Quest Quests in ModifingExistantQuest)
        {
            QuestManager.ModifiQuest(Quests);
        }
    }

    public void ChangeIDQuest()
    {

    }


    

}

[System.Serializable]
public class ChangeID {
    public int CurrentIDQuest;
    public int NewIDQuest;

}


