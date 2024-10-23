using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    [SerializeField] GameObject slot;
    [SerializeField] Transform parent;
    [SerializeField] TextMeshProUGUI playerGold;

    public void UpdateSelf()
    {
        Inventory.Instance.UpdateItemListeUI(this);
    }

    public void UpdateUI(List<ItemData> itemList)
    {
        foreach (Transform child in parent)
        {
            Destroy(child.gameObject);
        }

        for (int i = 0; i < itemList.Count; i++)
        {
            Sprite sprite = null;
            uint quantity = 0;
            uint maxStack = 0;

            if (itemList[i] != null)
            {
                ItemData item = itemList[i];
                sprite = this.GetSpriteFromItem(item);
                quantity = (uint)item.quantity;
                maxStack = (uint)item.quantityMax;
            }

            this.CreateSlot(i, sprite, quantity, maxStack);
        }
    }

    private void CreateSlot(int slotIndex, Sprite sprite, uint quantity, uint maxStack)
    {
        GameObject newSlot = Instantiate(slot, parent);
        newSlot.GetComponent<InventorySlot>().SetSlot(slotIndex, sprite, quantity, maxStack > 1);
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
