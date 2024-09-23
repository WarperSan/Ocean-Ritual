using ControllerModule.Controllers.Interfaces;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickableFish : MonoBehaviour, IInteractable
{
    [SerializeField] string FishName;
    [SerializeField] int MaxQUantiter;
    [SerializeField] int Quantiter;
    public void OnClick()
    {
       PoissonData poison =  new PoissonData();
        poison.nom = FishName;
        poison.quantiter = Quantiter;
        poison.quantiterMax = MaxQUantiter;
        Inventaire.Instance.AddItem(poison);
    }
}
