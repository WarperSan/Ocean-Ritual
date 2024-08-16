using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ControllerModule.Controllers.Interfaces;

public class PNJ : MonoBehaviour, IInteractable 
{

   
    [SerializeField]   int idQuest=0;
    [SerializeField] Quest theQUest ;
    [SerializeField] List<Quest> questsToGive = new ();
    [SerializeField] List<Quest> questsToConfirm = new ();
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(questsToGive.Count);
        Quest theQuest = QuestManager.GetQuestById(idQuest);
        if (theQuest != null)
        {
            questsToGive.Add(theQuest);
            theQUest = theQuest;
        }
      
        Debug.Log(questsToGive.Count);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ReceiveQuest(Quest questToReceive)
    {
        questsToGive.Add(questToReceive);
    }
    public void GiveQuest(int questToGive) {
    
    
    }
    public void ConfirmReward(int IDquest)
    {
        Quest theQuest = QuestManager.GetQuestById(IDquest);
        if (theQuest.QuestComplet)
        {
            Debug.Log("récompence de quete donné");
        }
    }
    public void OnClick()
    {
        PNJManager.GiveQuestToPlayer(questsToGive[0]);
        PNJManager.GiveNameToQUestManager(name);
    }
}
