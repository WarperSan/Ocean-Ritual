using FishingModule;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]

public class PoissonData : ItemData
{
   public string nom;

   public PoissonData()
   {

   }

   public PoissonData(FishSO fish, uint quantity)
   {
      this.nom = fish.name;
      this.quantity = (int) quantity;
      this.quantityMax = 10;
      this.sprite = fish.Icon;
   }
}
