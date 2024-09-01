using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{


   [SerializeField] List<Quest> questsInProgress= new List<Quest>();
    [SerializeField] List<Quest> QuestComplet = new List<Quest>();
    [SerializeField] GameObject contentTransform;
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
            QuestInterface.AddQuestToUI(theAddingQuest, contentTransform.transform);
        }

    }
    public void RemoveQuest(Quest removingQuest)
    {
        // Enlever la qu�te des qu�tes en cours
        if (questsInProgress.Contains(removingQuest))
        {
            questsInProgress.Remove(removingQuest);
        }
        else
        {
            Debug.LogWarning("La qu�te � supprimer n'est pas dans la liste des qu�tes en cours.");
        }
    }

    public void MoveQuestToComplete(Quest completedQuest)
    {

        // Ajouter la qu�te � la liste des qu�tes compl�t�es
        QuestComplet.Add(completedQuest);
        // Enlever la qu�te de la liste des qu�tes en cours
        RemoveQuest(completedQuest);
       QuestInterface.QuestToDelete(completedQuest);

    }
}
