using ControllerModule.Controllers.Interfaces;
using InteractModule;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickableGemme : MonoBehaviour, IInteractable
{
    [SerializeField] GemmeData laGemme;
    [SerializeField] int lvlOfGemme;
    [SerializeField] Sprite sprite;

    public InteractionAsset InteractionAsset => null;

    public void OnClick()
    {
        laGemme= GeneratorGemme.GenerateRandomGemme(lvlOfGemme);
        laGemme.sprite= sprite;
        Inventaire.Instance.AddItem(laGemme);
    }
}
