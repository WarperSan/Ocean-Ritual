using InteractModule;
using UnityEngine;

public class PickableObject : MonoBehaviour, IInteractable
{
    public InteractionAsset InteractionAsset => null;

    public void OnClick()
    {
    }
}