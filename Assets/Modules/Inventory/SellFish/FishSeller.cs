using InteractModule;
using UIModule;
using UnityEngine;

public class FishSeller : MonoBehaviour, IInteractable
{
    [SerializeField]
    private FishMenu InterfaceSeller;

    public InteractionAsset InteractionAsset => null;

    /// <inheritdoc/>
    public void OnClick() => openMenu();

    public void openMenu()
    {
        UIManager.Open<FishMenu>();
        InterfaceSeller.ShowMenu();
    }
}