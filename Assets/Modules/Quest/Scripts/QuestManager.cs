using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;
public static class QuestManager
{
    #region gestionDonne
    private static string DataQuestPath = "Quest";
    private static Dictionary<int, Quest> quests = new Dictionary<int, Quest>();
    public static void GetDataQuestLoad()
    {
        // Chemin du dossier Resources/Quest
        string directoryPath = Path.Combine(Application.dataPath, "Resources", DataQuestPath);

        // Charge tous les fichiers JSON dans le dossier Resources/Quest
        TextAsset[] questFiles = Resources.LoadAll<TextAsset>(DataQuestPath);

        quests.Clear();
        foreach (TextAsset questFile in questFiles)
        {

            // Convertit le fichier JSON en QuestData
            QuestData questData = JsonUtility.FromJson<QuestData>(questFile.text);

            // Ajoute les qu�tes � la liste des qu�tes
            foreach (Quest quest in questData.quests)
            {
                quests[quest.ID] = quest;
            }
        }

     //   Debug.Log($"Loaded {quests.Count} quests.");
    }
    public static QuestData GetDataQuestLoadModif()
    {
        // Chemin du dossier Resources/Quest
        string directoryPath = Path.Combine(Application.dataPath, "Resources", DataQuestPath);

        // Charge tous les fichiers JSON dans le dossier Resources/Quest
        TextAsset[] questFiles = Resources.LoadAll<TextAsset>(DataQuestPath);

        // V�rifie si des fichiers ont �t� trouv�s
        if (questFiles == null || questFiles.Length == 0)
        {
            Debug.LogWarning("No quest files found in the specified path.");
            return null;
        }

        // Essaie de charger et de convertir le premier fichier JSON en QuestData
        try
        {
            QuestData questData = JsonUtility.FromJson<QuestData>(questFiles[0].text);

            // V�rifie si des qu�tes ont �t� charg�es
            if (questData == null || questData.quests == null || questData.quests.Count == 0)
            {
                Debug.LogWarning("No quests found in the loaded quest data.");
                return null;
            }

            Debug.Log($"Quest data loaded successfully. Number of quests: {questData.quests.Count}");
            return questData;
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Error loading quest data: {e.Message}");
            return null;
        }
    }
    public static void ResetData()
    {
        quests.Clear();
        QuestToDataSave();
    }
    public static void QuestToDataSave()
    {
        string directoryPath = Path.Combine(Application.dataPath, "Resources", DataQuestPath);

        // Cr�e le dossier s'il n'existe pas
        if (!Directory.Exists(directoryPath))
        {
            Directory.CreateDirectory(directoryPath);
        }
        //
        QuestData data = new(new List<Quest>(quests.Values));
        
 
        string json = JsonUtility.ToJson(data, true);
        Debug.Log(json);
        string filePath = Path.Combine(directoryPath, "QuestData.json");

        File.WriteAllText(filePath, json);

       // Debug.Log($"Saved {data.quests.Count} quests to {filePath}");
    }

    public static void StartQuest(int questId)
    {
        if (quests.ContainsKey(questId))
        {
            Quest quest = quests[questId];
            // Logique pour d�marrer la qu�te
            Debug.Log($"Quest started: {quest.Name}");
        }
        else
        {
            Debug.LogError("Quest ID not found: " + questId);
        }
    }

    public static void CompleteQuest(int questId)
    {
        if (quests.ContainsKey(questId))
        {
            Quest quest = quests[questId];
            // Logique pour compl�ter la qu�te
            Debug.Log($"Quest completed: {quest.Name}");
        }
        else
        {
            Debug.LogError("Quest ID not found: " + questId);
        }
    }

    public static void AddQuest(Quest quest)
    {
        if (!quests.ContainsKey(quest.ID))
        {
            quests.Add(quest.ID, quest);
            Debug.Log($"Quest added: {quest.Name}");
        }
        else
        {
            quest.ID = GetNextID();
            Debug.LogError("Quest ID already exists:   we generate a new ID for you new ID:" + quest.ID);
        
            if (!quests.ContainsKey(quest.ID))
            {
                quests.Add(quest.ID, quest);
                Debug.Log($"Quest added: {quest.Name}");
            }
        }
      
    }

    public static Quest GetQuestById(int questId)
    {
        GetDataQuestLoad();
        if (quests.ContainsKey(questId))
        {
            return quests[questId];
        }
        else
        {
            Debug.Log(quests.Count);
            Debug.LogError("Quest ID not found: " + questId);
            return null;
        }
    }
    public static void ModifyQuest(Quest TheQuest)
    {
        if (quests.ContainsKey(TheQuest.ID))
        {
            if (ValidQuest(TheQuest))
            {
                quests[TheQuest.ID] = TheQuest;
            }
            else
            {
                Debug.Log($"The Quest is not good  ID Quest :"+TheQuest.ID);
            }

            Debug.Log($"Quest Change: {TheQuest.Name}");
        }
    }
    public static int GetNextID()
    {
        // Retrieve all existing IDs from the dictionary
        HashSet<int> existingIds = new HashSet<int>(quests.Keys);

        // Find the minimum available ID
        int nextId = 1; 
        while (existingIds.Contains(nextId))
        {
            nextId++;
        }

        return nextId;
    }
    public static bool ValidQuest(Quest TheQuest)
    {


        return true;
    }
    #endregion
    #region SurveillanceQuete
    private static Player playerScript;
    private static List<Quest> ListQuestToChek = new();
    static public bool AddQuestToWatch(Quest questToWatch)
    {
        // V�rifie si la qu�te existe d�j� dans la liste
        if (!ListQuestToChek.Any(q => q.ID == questToWatch.ID))
        {
            ListQuestToChek.Add(questToWatch);
           // Debug.Log($"Qu�te {questToWatch.Name} (ID: {questToWatch.ID}) ajout�e � la liste de v�rification.");
            return true;
        }
        else
        {
            Debug.Log($"La qu�te {questToWatch.Name} est d�j� dans la liste de v�rification.");
            return false;
        }
    }


    static public void SomeoneDeath(string mobName)
    {
        // Filtrer les qu�tes de type 'Extermination'
        var questsToUpdate = ListQuestToChek
            .Where(quest => quest.ConditionQuest.typeOfTheQuest == typeOfQuest.Extermination)
            .ToList();

        // Parcourir les qu�tes filtr�es
        foreach (Quest quest in questsToUpdate)
        {
            if (!quest.QuestComplet)
            {
                // Parcourir chaque condition de la qu�te
                foreach (var cond in quest.ConditionQuest.condition)
                {

                    if (cond.name == mobName)
                    {
                        // Incr�menter quantiteInProgress
                        cond.quantiteInProgress++;

                        // V�rifier si la condition est remplie
                        if (cond.quantiteInProgress >= cond.quantite)
                        {
                            quest.QuestComplet = true;
                           
                            //Debug.Log($"La qu�te {quest.Name} (ID: {quest.ID}) est maintenant compl�te tout les mob sont mort.");
                        }
                    }
                }
                if (!quest.QuestComplet)
                {
                    QuestInterface.QuestKillingUpdate(quest);
                }
                else
                {
                    QuestInterface.QuestKillingUpdate(quest);
                    QuestInterface.QuestToEarnReward(quest);

                }
                
            }
        }
    }
    static public void SomeoneTalking(string PNJName)
    {
      
        // Debug.Log("Je parle � " + PNJName);

        // Filtrer les qu�tes de type 'Discution'
        var questsToUpdate = ListQuestToChek
            .Where(quest => quest.ConditionQuest.typeOfTheQuest == typeOfQuest.Discution)
            .ToList();
       // Debug.Log(questsToUpdate.Count);
        // Parcourir les qu�tes filtr�es
        foreach (Quest quest in questsToUpdate)
        {
            
            // V�rifier chaque condition de la qu�te
            if (quest.ConditionQuest.condition
                .Any(cond => cond.name == PNJName))
            {
                // Si le nom correspond, marquer la qu�te comme compl�te
                quest.QuestComplet = true;
                QuestInterface.QuestToEarnReward(quest);
                Debug.Log($"La qu�te {quest.Name} (ID: {quest.ID}) est maintenant compl�te.");
            }
         
        }
        GiveReward(PNJName);


    }
    static public void GiveReward( string name)
    {
       
        List<Quest>completQuestToRemove = new ();
        foreach (Quest quest in ListQuestToChek)
        {
            if (quest.QuestConfirmer == name && quest.QuestComplet)
            {
                GetPlayerScript();
                if (playerScript != null)
                {
                    playerScript.ReceiveReward();
                    playerScript.MoveQuestToComplete(quest);
                    completQuestToRemove.Add(quest);
                 
                }
                else
                {
                    Debug.Log("pas de joueur trouver");
                }

           
            }
        }
        foreach (Quest quests in completQuestToRemove)
        {
            RemoveQuest(quests);
          
        }
    }
    static public void RessourceHarvrest(string RessourceName, int quantite = 1)
    {

    }

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
    private static void RemoveQuest(Quest removingQuest)
    {
        // Enlever la qu�te des qu�tes en cours
        if (ListQuestToChek.Contains(removingQuest))
        {
            ListQuestToChek.Remove(removingQuest);
        }
        else
        {
            Debug.LogWarning("La qu�te � supprimer n'est pas dans la liste des qu�tes en cours.");
        }
    }

    #endregion
}
