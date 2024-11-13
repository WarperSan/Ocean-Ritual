using FishingModule;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UIModule;
using UIModule.Menus;
using UnityEngine;

public class FishMenu : AnimatedMenu
{
    [SerializeField] TextMeshProUGUI textMeshProUGUI;
    int cash;
    public void ShowMenu()
    {
        this.cash = Inventory.Instance.NumberOfCashFromSellingFish();
        textMeshProUGUI.text = string.Format("Voulez-vous vendre vos poissons pour <sprite name={0}> <color=#070>{1}</color>?", Inventory.CASH_ICON, cash);
    }

    public void Deny()
    {
        UIManager.Close(this);
    }
    public void Accept()
    {
        Inventory.Instance.sellingAllFish();
        
        UIManager.Close(this);
    }
}

