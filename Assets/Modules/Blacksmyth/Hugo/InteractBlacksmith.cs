using InteractModule;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UIModule;
using UIModule.Menus;
using UnityEngine;

public class InteractBlacksmith : MonoBehaviour, InteractModule.IInteractable
{
    [SerializeField] 
    public InteractionAsset InteractionAsset => null;

    public void OnClick() => OpenBlacksmith();

    void OpenBlacksmith()
    {
        UIManager.Toggle<BlacksmithMenu>();
    }
}
