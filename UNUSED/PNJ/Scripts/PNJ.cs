using System.Collections.Generic;
using UnityEngine;

public class PNJ : MonoBehaviour
{
    [SerializeField] int idQuest = 0;

    [SerializeField] bool donneQuest = false;
    [SerializeField] Quest theQUest;
    [SerializeField] List<Quest> questsToGive = new();
    [SerializeField] List<Quest> questsToConfirm = new();

    // Start is called before the first frame update

    void Start()
    {
        if (donneQuest)
        {

            Quest theQuest = QuestManager.GetQuestById(idQuest);
            if (theQuest != null)
            {
                questsToGive.Add(theQuest);
                theQUest = theQuest;
            }

        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ReceiveQuest(Quest questToReceive)
    {
        questsToGive.Add(questToReceive);
    }
    public void GiveQuest(int questToGive)
    {


    }

    public void OnInteraction()
    {
        if (donneQuest)
        {
            PNJManager.GiveQuestToPlayer(questsToGive[0]);

        }

        PNJManager.GiveNameToQUestManager(name);
    }
}
