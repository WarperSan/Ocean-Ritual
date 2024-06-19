using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Xml;
public static class QuestManager
{
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

            // Ajoute les quêtes à la liste des quêtes
            foreach (Quest quest in questData.quests)
            {
                quests[quest.ID] = quest;
            }
        }

        Debug.Log($"Loaded {quests.Count} quests.");
    }
    public static QuestData GetDataQuestLoadModif()
    {
        // Chemin du dossier Resources/Quest
        string directoryPath = Path.Combine(Application.dataPath, "Resources", DataQuestPath);

        // Charge tous les fichiers JSON dans le dossier Resources/Quest
        TextAsset[] questFiles = Resources.LoadAll<TextAsset>(DataQuestPath);
        Debug.Log(JsonUtility.FromJson<QuestData>(questFiles[0].text).quests[0]);
        Debug.Log($"Quest temporaire chargé.");
        return JsonUtility.FromJson<QuestData>(questFiles[0].text);
    }

    public static void QuestToDataSave()
    {
        string directoryPath = Path.Combine(Application.dataPath, "Resources", DataQuestPath);

        // Crée le dossier s'il n'existe pas
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

        Debug.Log($"Saved {data.quests.Count} quests to {filePath}");
    }

    public static void StartQuest(int questId)
    {
        if (quests.ContainsKey(questId))
        {
            Quest quest = quests[questId];
            // Logique pour démarrer la quête
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
            // Logique pour compléter la quête
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
        if (quests.ContainsKey(questId))
        {
            return quests[questId];
        }
        else
        {
            Debug.LogError("Quest ID not found: " + questId);
            return null;
        }
    }
    public static void ModifiQuest(Quest TheQuest)
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
}
