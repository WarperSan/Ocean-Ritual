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
        textMeshProUGUI.text = "Vendre votre poisson pour  : " + cash  + " d'or?"; 
    }

    public void Deny()
    {
        UIManager.Close(this);
    }
    public void Accept()
    {
        Inventory.Instance.AddCash(cash);
        UIManager.Close(this);
    }
}

