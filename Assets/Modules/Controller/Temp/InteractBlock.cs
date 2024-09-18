using UnityEngine;
using ControllerModule.Controllers.Interfaces;
using ControllerModule;

public class InteractBlock : MonoBehaviour, IInteractable
{
    public InteractionAsset InteractionAsset => null;

    public void OnClick() 
    {
        this.transform.position += new Vector3(0, 0.1f, 0);
    }
}