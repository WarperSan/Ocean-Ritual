using ControllerModule.Controllers.Interfaces;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickableGemme : MonoBehaviour, IInteractable
{

    [SerializeField] int lvlOfGemme;
    public void OnClick()
    {
        GeneratorGemme.GenerateRandomGemme(lvlOfGemme, "Red");
    }
}
