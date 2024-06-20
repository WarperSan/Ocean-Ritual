using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

[CustomEditor(typeof(QuestGenerator))]
public class QuestModifEditor : UnityEditor.Editor
{
    private QuestGenerator questGenerator;
    private List<Quest> questList;
    private List<int> selectedQuestIDs = new List<int>();
    private string questIDsInput;

    private void OnEnable()
    {
        questGenerator = (QuestGenerator)target;
        questList = new List<Quest>(questGenerator.ModifingExistantQuest);
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Modify Existing Quests", EditorStyles.boldLabel);

        // Champ pour entrer les IDs de quêtes
        questIDsInput = EditorGUILayout.TextField("Quest IDs (separated by virgule)", questIDsInput);

        if (GUILayout.Button("Load Quest Data"))
        {
            LoadQuestData(questIDsInput);
          
        }
        GUILayout.Space(10);
        if (GUILayout.Button("Save Modified Quests"))
        {
            SaveModifiedQuests();
          
        }
        GUILayout.Space(10);
        if (GUILayout.Button("Adding New Quests"))
        {
            AddNewQuest();
            
        }
        GUILayout.Space(10);
        if (GUILayout.Button("NUKE QUEST DATA!"))
        {
            ResetDataQ();
        }
    }

    private void LoadQuestData(string idsInput)
    {
        selectedQuestIDs.Clear();

        // Parse the input string to get the list of IDs
        string[] ids = idsInput.Split(',');
        foreach (string id in ids)
        {
            if (int.TryParse(id.Trim(), out int questID))
            {
                selectedQuestIDs.Add(questID);
            }
            else
            {
                Debug.LogWarning($"Invalid quest ID: {id}");
            }
        }

        QuestData questData = QuestManager.GetDataQuestLoadModif();
        questList.Clear();
        foreach (int questID in selectedQuestIDs)
        {
            Quest questToLoad = questData.quests.Find(q => q.ID == questID);

            if (questToLoad != null)
            {
                Quest existingQuest = questList.Find(q => q.ID == questID);
                if (existingQuest != null)
                {
                    questList.Remove(existingQuest);
                }
                questList.Add(questToLoad);
               // Debug.Log($"Quest with ID {questID} loaded and added to the list.");
            }
            else
            {
                Debug.LogWarning($"Quest with ID {questID} not found.");
            }
        }

        questGenerator.ModifingExistantQuest = questList;
        EditorUtility.SetDirty(questGenerator);
    }
    private void ResetDataQ()
    {
        QuestManager.ResetData();
    }
    private void SaveModifiedQuests()
    {
        AssetDatabase.Refresh();
        questGenerator.ModifiQuest();
        EditorUtility.SetDirty(questGenerator);
        // Force Unity à rafraîchir les assets

       AssetDatabase.Refresh();

        Debug.Log($"Modified quests saved.");
    }
    private void AddNewQuest()
    {
        AssetDatabase.Refresh();
        questGenerator.AddingQuest();
        EditorUtility.SetDirty(questGenerator);
        // Force Unity à rafraîchir les assets

        AssetDatabase.Refresh();

        Debug.Log($"adding quests saved.");
    }
}