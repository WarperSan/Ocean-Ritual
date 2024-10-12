using System.Collections.Generic;
using TMPro;
using UIModule;
using UIModule.Interfaces;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : UIComponent, IHoverable, IDraggable, IDragReceivable
{
    [SerializeField] Image itemImage;
    [SerializeField] TextMeshProUGUI quantity;
    [SerializeField] Graphic background;
    public int slotIndex;

    private void Start()
    {
        this.canvasParent = this.GetComponentInParent<Canvas>().transform;
    }

    /// <summary>
    ///  Sets the inventory slot with the sprite and quantity of an item
    /// </summary>
    public void SetSlot(int slotIndex, Sprite sprite, uint qty = 1, bool isStackable = true)
    {
        this.slotIndex = slotIndex;
        if (sprite == null)
        {
            this.ClearSlot();
            return;
        }

        this.itemImage.sprite = sprite;

        this.quantity.text = isStackable ? "x" + qty.ToString() : "";
        Color itemColor = this.itemImage.color;
        itemColor.a = 1f;
        this.itemImage.color = itemColor;

        this.enabled = true;
        //this.dragAndDropHandler.enabled = true;
    }

    /// <summary>
    /// Clears an inventory slot
    /// </summary>
    public void ClearSlot()
    {
        Color itemColor = this.itemImage.color;
        itemColor.a = 0f;
        this.itemImage.color = itemColor;
        this.quantity.text = "";

        this.enabled = false;
        //this.dragAndDropHandler.enabled = false; // If the slot is cleared, cannot be dragged
    }

    /// <summary>
    /// Sets the alpha of the background for this slot
    /// </summary>
    /// <param name="alpha">Value between 0 and 1</param>
    public void SetBackgroundAlpha(float alpha)
    {
        Color bgColor = this.background.color;
        bgColor.a = Mathf.Clamp01(alpha);
        this.background.color = bgColor;
    }

    #region Drag

    [Header("Drag")]
    [SerializeField] CanvasGroup canvasGroup;

    Transform originalParent;
    GameObject fillingChild;
    Transform canvasParent;

    /// <inheritdoc/>
    public void OnDragStart()
    {
        // Set up slot for drag
        this.originalParent = this.transform.parent;
        this.canvasGroup.blocksRaycasts = false;

        // Adds temporary ghost slot
        this.fillingChild = Instantiate(this.gameObject, this.transform.parent);
        this.fillingChild.GetComponent<CanvasGroup>().alpha = 0.3f;
        this.fillingChild.transform.SetSiblingIndex(this.transform.GetSiblingIndex());

        this.transform.SetParent(this.canvasParent);

        this.SetBackgroundAlpha(0f);
    }

    public void OnDragEnd(IDragReceivable receivable)
    {
        if (receivable != null)
        {
            // Check if hovering another slot
            if (receivable.Rect.TryGetComponent(out InventorySlot targetSlot))
            {
                Inventory.Instance.SwapPlace(this.slotIndex, targetSlot.slotIndex);

                this.transform.SetParent(this.originalParent);
                this.transform.SetSiblingIndex(targetSlot.slotIndex);

                targetSlot.transform.SetSiblingIndex(this.slotIndex);

                (this.slotIndex, targetSlot.slotIndex) = (targetSlot.slotIndex, this.slotIndex);
            }
        }
        else
        {
            Inventory.Instance.DropItem(this.slotIndex);
            this.ClearSlot();

            this.transform.SetParent(this.originalParent);
            this.transform.SetSiblingIndex(this.slotIndex);
        }

        if (this.fillingChild != null)
            Destroy(this.fillingChild);

        this.canvasGroup.blocksRaycasts = true;

        this.SetBackgroundAlpha(1f);
    }

    #endregion Drag

    #region IHoverable

    /// <inheritdoc/>
    public ItemData GetData() => Inventory.Instance.GetItem(this.slotIndex);

    #endregion
}