using ControllerModule;
using ControllerModule.Controllers.Interfaces;
using UnityEngine;

public class EnnemieDeath : MonoBehaviour, IInteractable
{
    public InteractionAsset InteractionAsset => null;

    // Start is called before the first frame update

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnClick()
    {
        QuestManager.SomeoneDeath(name);
    }
}
