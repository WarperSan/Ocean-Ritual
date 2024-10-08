using FishingModule;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]

public class FishData : ItemData
{
   public string name;
    public FishRarity rarety;
    public string description;
    public FishData() { }
    public FishData(FishSO fish, uint quantity) {
        sprite = fish.Icon;
        name = fish.DisplayName;
        rarety = fish.Rarity;
        this.quantity = (int)quantity;
    }
}
