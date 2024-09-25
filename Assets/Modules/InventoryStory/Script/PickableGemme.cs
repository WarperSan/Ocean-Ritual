using ControllerModule.Controllers.Interfaces;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickableGemme : MonoBehaviour, IInteractable
{
    [SerializeField] GemmeData laGemme;
    [SerializeField] int lvlOfGemme;
    [SerializeField] Sprite sprite;
    public void OnClick()
    {
        laGemme= GeneratorGemme.GenerateRandomGemme(lvlOfGemme);
        laGemme.sprite= sprite;
        Inventaire.Instance.AddItem(laGemme);
    }
}
