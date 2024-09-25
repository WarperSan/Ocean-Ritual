using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    [SerializeField] GameObject slot;
    [SerializeField] Transform parent;
    [SerializeField] TextMeshProUGUI playerGold;

    private void OnEnable()
    {
        Inventaire.Instance.UpdateItemListeUI();
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
                quantity = (uint)item.quantiter;
                maxStack = (uint)item.quantiterMax;
            }

            this.CreateSlot(sprite, quantity, maxStack);
        }
    }

    private void CreateSlot(Sprite sprite, uint quantity, uint maxStack)
    {
        GameObject newSlot = Instantiate(slot, parent);
        newSlot.GetComponent<InventorySlot>().SetSlot(sprite, quantity, maxStack > 1);
    }

    private Sprite GetSpriteFromItem(ItemData item)
    {
        if (item == null)
            return null;

        return item.sprite;
    }

    public void SetPlayerGold(int gold = 9999)
    {
        playerGold.text = "$" + gold.ToString();
    }
}
