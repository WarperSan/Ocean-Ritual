using System.Collections.Generic;
using UnityEngine;

public class QuestUI : MonoBehaviour
{
    [SerializeField]
    private List<string> listQuest = new();

    [SerializeField]
    private GameObject slot;

    [SerializeField]
    private Transform parent;

    public void SetQuestUI()
    {
        if (listQuest.Count > 0)
        {
            for (int i = 0; i < listQuest.Count; i++)
            {
                string quest = listQuest[i];

                CreateSlot(quest, i);
            }
        }
    }

    private void CreateSlot(string quest, int index)
    {
        GameObject newSlot = Instantiate(slot, parent);
        newSlot.name = $"Slot{index + 1}";
        newSlot.GetComponent<QuestSlot>().SetQuestSlot(quest);
    }
}