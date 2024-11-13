using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

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
