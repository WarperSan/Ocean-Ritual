using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManage : MonoBehaviour
{
    [SerializeField] QuestUI questUI;

    void Start()
    {
        questUI.SetQuestUI();
    }
}
