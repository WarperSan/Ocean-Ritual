using ControllerModule.Controllers.Interfaces;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickableGemme : MonoBehaviour, IInteractable
{
    [SerializeField] GemmeData laGemme;
    [SerializeField] int lvlOfGemme;
    public void OnClick()
    {
        laGemme= GeneratorGemme.GenerateRandomGemme(lvlOfGemme);
        Inventaire.Instance.AddItem(laGemme);
    }
}
