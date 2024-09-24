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
    List<ItemData> listeItems = new List<ItemData>();
    private void OnEnable()
    {
        listeItems = Inventaire.Instance.GetInventaire();
        UpdateUI(listeItems);
    }

    private void OnDisable()
    {
        Inventaire.Instance.OnInventoryChanged -= UpdateUI;
    }

    public void UpdateUI(List<ItemData> itemList)
    {
        foreach (Transform child in parent)
        {
            Destroy(child.gameObject);
        }

        foreach (var item in listeItems)
        {
            
            Sprite sprite = GetSpriteFromItem(item);
            uint quantity = (uint)item.quantiter;
            CreateSlot(sprite, quantity);
        }
    }

    private void CreateSlot(Sprite sprite, uint quantity)
    {
        GameObject newSlot = Instantiate(slot, parent);
        newSlot.GetComponent<InventorySlot>().SetSlot(sprite, quantity);
    }

    private Sprite GetSpriteFromItem(ItemData item)
    {
        if (item == null || item.sprite == null)
            return null;

        return item.sprite;
    }

    public void SetPlayerGold(int gold = 9999)
    {
        playerGold.text = "$" + gold.ToString();
    }
}
