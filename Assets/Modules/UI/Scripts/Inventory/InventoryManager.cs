using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    [SerializeField] InventoryUI inventoryUI;
    public int maxSlot = 28;

    void Start()
    {
        inventoryUI.SetInventory(maxSlot);
        inventoryUI.SetPlayerGold();
    }

}
