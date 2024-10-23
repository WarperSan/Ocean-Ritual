using InteractModule;
using System.Collections;
using System.Collections.Generic;
using UIModule.Menus;
using UIModule;
using UnityEngine;

public class FishSeller : MonoBehaviour, IInteractable
{
    [SerializeField] FishMenu InterfaceSeller;

    public InteractionAsset InteractionAsset => null;

    /// <inheritdoc/>
    public void OnClick() => openMenu();


    public void openMenu()
    {
        InterfaceSeller.ShowMenu();
        UIManager.Open<FishMenu>();
    }

}
