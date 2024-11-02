using InteractModule;
using System.Collections;
using System.Collections.Generic;
using UIModule;
using UIModule.Menus;
using UnityEngine;

public class wheelTrigger : MonoBehaviour, IInteractable
{
    [SerializeField] WheelMenu wheelMenu;

    public InteractionAsset InteractionAsset => null;

    /// <inheritdoc/>
    public void OnClick() => openMenu();


    public void openMenu()
    {
        TransitionCam.Instance.SwitchToCamB();
        UIManager.Open<WheelMenu>();
      

    }


}
