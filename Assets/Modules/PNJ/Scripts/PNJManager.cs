using UnityEngine;

public static class PNJManager
{
    private static Player playerScript;

    // M�thode statique pour initialiser le script Player
    static PNJManager()
    {
        GetPlayerScript();
    }

    // M�thode priv�e pour obtenir le script Player de l'objet avec le tag "Player"
    private static void GetPlayerScript()
    {
        GameObject playerObject = GameObject.FindWithTag("Player");

        if (playerObject != null)
        {
            playerScript = playerObject.GetComponent<Player>();

            if (playerScript == null)
            {
                Debug.LogError("Le script 'Player' n'a pas �t� trouv� sur l'objet.");
            }
        }
        else
        {
            Debug.LogError("Aucun objet avec le tag 'Player' n'a �t� trouv�.");
        }
    }

    // M�thode pour donner une qu�te au joueur
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
                Debug.Log($"Quest {quest.Name} (ID: {quest.ID}) donn�e au joueur.");
            }
            else
            {
                Debug.LogError($"La qu�te avec l'ID {idQuest} n'a pas �t� trouv�e.");
            }
        }
        else
        {
            Debug.LogError("Le joueur n'a pas �t� trouv� pour donner la qu�te.");
        }
    }
    // M�thode pour donner une qu�te au joueur
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
            Debug.LogError("Le joueur n'a pas �t� trouv� pour donner la qu�te.");
        }
    }
    public static void GiveNameToQUestManager(string name)
    {
   
        QuestManager.SomeoneTalking(name);
    }
}