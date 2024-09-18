using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    [SerializeField] List<Sprite> listSprite = new();

    [SerializeField] List<uint> listQuantity = new();

    [SerializeField] GameObject slot;

    [SerializeField] Transform parent;

    [SerializeField] TextMeshProUGUI playerGold;
    
    public void SetInventory(int maxSlot)
    {
        // Add missing items
        for (int i = 0; i < maxSlot - listSprite.Count; i++)
            listSprite.Add(null);

        for (int i = 0; i < maxSlot; i++)
        {
            Sprite sprite = null;
            uint quantity = 0;
            
            // If there is an item
            if (listQuantity.Count > i)
            {
                sprite = listSprite[i];
                quantity = listQuantity[i];
            }

            this.CreateSlot(sprite, quantity, i);
        }
    }

    private void CreateSlot(Sprite sprite, uint quantity, int index)
    {
        GameObject newSlot = Instantiate(slot, parent);
        newSlot.name = $"Slot{index + 1}";
        newSlot.GetComponent<InventorySlot>().SetSlot(sprite, quantity);
    }

    public void SetPlayerGold(int gold = 9999)
    {
        playerGold.text = "$" + gold.ToString();
    }
}
