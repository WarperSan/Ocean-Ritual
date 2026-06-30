using UnityEngine;

public class QuestManage : MonoBehaviour
{
    [SerializeField]
    private QuestUI questUI;

    private void Start() => questUI.SetQuestUI();
}