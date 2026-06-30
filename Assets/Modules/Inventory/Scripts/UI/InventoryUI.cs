using ExtensionsModule;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    [SerializeField]
    private GameObject slot;

    [SerializeField]
    private TextMeshProUGUI playerGold;

    [SerializeField]
    private Transform[] pages;

    public void UpdateSelf() => Inventory.Instance.UpdateItemListeUI(this);

    public void UpdateUI(List<ItemData> itemList)
    {
        // Clear all pages
        foreach (Transform item in pages)
            item.RemoveAll();

        int amountPerPage = Inventory.Instance.NbSlotInventory / pages.Length;

        for (int i = 0; i < itemList.Count; i++)
        {
            Sprite sprite = null;
            uint quantity = 0;
            uint maxStack = 0;

            if (itemList[i] != null)
            {
                ItemData item = itemList[i];
                sprite = GetSpriteFromItem(item);
                quantity = (uint)item.quantity;
                maxStack = (uint)item.quantityMax;
            }

            // Choisir le parent en fonction de l'index
            Transform slotParent = pages[i / amountPerPage];

            CreateSlot(i,
                sprite,
                quantity,
                maxStack,
                slotParent);
        }
    }

    private void CreateSlot(
        int       slotIndex,
        Sprite    sprite,
        uint      quantity,
        uint      maxStack,
        Transform slotParent
    )
    {
        // Instancie le nouveau slot dans le bon parent
        GameObject newSlot = Instantiate(slot, slotParent);

        newSlot.GetComponent<InventorySlot>()
        .SetSlot(slotIndex,
            sprite,
            quantity,
            maxStack > 1);
    }

    private Sprite GetSpriteFromItem(ItemData item)
    {
        if (item == null)
            return null;

        return item.sprite;
    }

    public void SetPlayerGold(int gold = 9999)
    {
        if (playerGold == null)
            return;

        playerGold.text = "$" + gold.ToString();
    }
}