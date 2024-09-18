using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class QuestUI : MonoBehaviour
{
    [SerializeField] List<string> listQuest = new();

    [SerializeField] GameObject slot;

    [SerializeField] Transform parent;

    public void SetQuestUI()
    {
        if(listQuest.Count > 0)
        {
            for (int i = 0; i < listQuest.Count; i++)
            {
                string quest = listQuest[i];

                this.CreateSlot(quest, i);
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
