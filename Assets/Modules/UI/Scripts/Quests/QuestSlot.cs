using TMPro;
using UnityEngine;

public class QuestSlot : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI textQuest;

    public void SetQuestSlot(string quest)
    {
        if (quest != "")
            textQuest.SetText(quest);
    }
}