using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class QuestGenerator : MonoBehaviour
{
    [SerializeField] List< Quest> AddingQuests = new();
    [SerializeField] public List<Quest> ModifingExistantQuest;
    [SerializeField] List<ChangeID> ChangeIDQuests;
    // Start is called before the first frame update
    void Start()
    {
        //applyModif();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void applyModif()
    {
        QuestManager.QuestToDataSave();
        AssetDatabase.Refresh();
        QuestManager.GetDataQuestLoad();
       
       
    }
    public void AddingQuest()
    {
        foreach(Quest quest in AddingQuests)
        {
            QuestManager.AddQuest(quest);
        }
        Debug.Log("pass AddingQuest");
        applyModif();
    }

    public void ModifiQuest()
    {

        foreach(Quest Quests in ModifingExistantQuest)
        {
            QuestManager.ModifyQuest(Quests);
        }
        Debug.Log("pass ModifQuest");
        applyModif();
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


