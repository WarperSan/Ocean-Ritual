using System.Collections.Generic;
using UnityEngine;

public static class PNJManager
{
    private static Player playerScript;

    // Méthode statique pour initialiser le script Player
    static PNJManager()
    {
        GetPlayerScript();
    }

    // Méthode privée pour obtenir le script Player de l'objet avec le tag "Player"
    private static void GetPlayerScript()
    {
        GameObject playerObject = GameObject.FindWithTag("Player");

        if (playerObject != null)
        {
            playerScript = playerObject.GetComponent<Player>();

            if (playerScript == null)
            {
                Debug.LogError("Le script 'Player' n'a pas été trouvé sur l'objet.");
            }
        }
        else
        {
            Debug.LogError("Aucun objet avec le tag 'Player' n'a été trouvé.");
        }
    }

    // Méthode pour donner une quête au joueur
    public static void GiveQuestToPlayer(int idQuest)
    {
        if (playerScript == null)
        {
            GetPlayerScript();
        }

        if (playerScript != null)
        {
            Quest quest = QuestManager.GetQuestById(idQuest);

            if (quest != null)
            {
                playerScript.AddQuest(quest);
                Debug.Log($"Quest {quest.Name} (ID: {quest.ID}) donnée au joueur.");
            }
            else
            {
                Debug.LogError($"La quête avec l'ID {idQuest} n'a pas été trouvée.");
            }
        }
        else
        {
            Debug.LogError("Le joueur n'a pas été trouvé pour donner la quête.");
        }
    }
    // Méthode pour donner une quête au joueur
    public static void GiveQuestToPlayer(Quest quest)
    {
        if (playerScript == null)
        {
            GetPlayerScript();
        }

        if (playerScript != null)
        {
           

            if (quest != null)
            {
                playerScript.AddQuest(quest);
               
            }
            else
            {
                
            }
        }
        else
        {
            Debug.LogError("Le joueur n'a pas été trouvé pour donner la quête.");
        }
    }
    public static void GiveNameToQUestManager(string name)
    {
        QuestManager.SomeoneTalking(name);
    }
}