using InteractModule;
using UIModule;
using UIModule.Menus;
using UnityEngine;

public class wheelTrigger : MonoBehaviour, IInteractable
{
    [SerializeField]
    private WheelMenu wheelMenu;

    [SerializeField]
    private LinkWheelEquipment Wheel;

    public InteractionAsset InteractionAsset => null;

    /// <inheritdoc/>
    public void OnClick() => openMenu();

    public void openMenu()
    {
        TransitionCam.Instance.SwitchToCamB();

        UIManager.Open<WheelMenu>();
        Wheel.ResetStandFromUpgrade();
    }
}