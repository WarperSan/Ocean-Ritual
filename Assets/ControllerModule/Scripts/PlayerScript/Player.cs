using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{


   [SerializeField] List<Quest> questsInProgress= new List<Quest>();
    [SerializeField] List<Quest> QuestComplet = new List<Quest>();
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void ReceiveReward()
    {

        Debug.Log("Reward Obtain");

    }
    public void AddQuest(Quest theAddingQuest)
    {
       
      if(QuestManager.AddQuestToWatch(theAddingQuest))
        {
            questsInProgress.Add(theAddingQuest);
        }

    }
    public void RemoveQuest(Quest removingQuest)
    {
        // Enlever la quête des quêtes en cours
        if (questsInProgress.Contains(removingQuest))
        {
            questsInProgress.Remove(removingQuest);
        }
        else
        {
            Debug.LogWarning("La quête à supprimer n'est pas dans la liste des quêtes en cours.");
        }
    }

    public void MoveQuestToComplete(Quest completedQuest)
    {

        // Ajouter la quête à la liste des quêtes complétées
        QuestComplet.Add(completedQuest);
        // Enlever la quête de la liste des quêtes en cours
        RemoveQuest(completedQuest);

    }
}
