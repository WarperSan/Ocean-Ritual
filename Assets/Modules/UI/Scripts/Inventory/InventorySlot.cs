using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    [SerializeField] Image item;
    [SerializeField] TextMeshProUGUI quantity;

    public void SetSlot(Sprite sprite, uint qty = 1)
    {
        if(sprite != null)
        {
            item.sprite = sprite;
            quantity.text = "x" + qty.ToString();
            Color itemColor = item.color;
            itemColor.a = 255f;
            item.color = itemColor;
        }
        
    }
    public void ClearSlot()
    {
        item.gameObject.SetActive(false);
    }
}
