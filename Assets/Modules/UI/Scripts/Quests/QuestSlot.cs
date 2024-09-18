using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static UnityEditor.Progress;

public class QuestSlot : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI textQuest;

    public void SetQuestSlot(string quest)
    {
        if (quest != "")
        {
            textQuest.SetText(quest);
        }
    }
}
