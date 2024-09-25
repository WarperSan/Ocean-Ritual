using ControllerModule.Controllers.Interfaces;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InteractModule;
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
