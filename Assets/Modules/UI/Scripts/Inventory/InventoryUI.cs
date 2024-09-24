using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static UnityEditor.Progress;

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
            if (itemList[i] != null)
            {
                ItemData item = itemList[i];
                Sprite sprite = GetSpriteFromItem(item);
                uint quantity = (uint)item.quantiter;
                CreateSlot(sprite, quantity);
            }
            else
            {
                CreateSlot(null, 0);
            }
        }
    }

    private void CreateSlot(Sprite sprite, uint quantity)
    {
        GameObject newSlot = Instantiate(slot, parent);
        newSlot.GetComponent<InventorySlot>().SetSlot(sprite, quantity);
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
