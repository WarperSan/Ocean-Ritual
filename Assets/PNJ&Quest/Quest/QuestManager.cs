using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class QuestManager
{
    private static Dictionary<int, Quest> quests = new Dictionary<int, Quest>();

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
            Debug.LogError("Quest ID already exists: " + quest.ID);
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
}