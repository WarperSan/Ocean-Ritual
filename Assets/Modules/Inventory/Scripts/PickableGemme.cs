using ControllerModule.Controllers.Interfaces;
using InteractModule;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickableGemme : MonoBehaviour, IInteractable
{
    [SerializeField] GemmeData TheGemme;
    [SerializeField] int lvlOfGemme;
    [SerializeField] Sprite sprite;

    public InteractionAsset InteractionAsset => null;

    public void OnClick()
    {
        TheGemme= GeneratorGemme.GenerateRandomGemme(lvlOfGemme);
        TheGemme.sprite= sprite;
        Inventory.Instance.AddItem(TheGemme);
    }
}
