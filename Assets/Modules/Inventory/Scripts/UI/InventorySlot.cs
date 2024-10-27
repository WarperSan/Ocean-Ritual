using ExtensionsModule;
using TMPro;
using UIModule;
using UIModule.Interfaces;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : UIComponent, IHoverable, IDraggable, IDragReceivable<InventorySlot>
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
        this.itemImage.SetAlpha(1);

        this.enabled = true;
        //this.dragAndDropHandler.enabled = true;
    }

    /// <summary>
    /// Clears an inventory slot
    /// </summary>
    public void ClearSlot()
    {
        this.itemImage.SetAlpha(0);
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

    #region IDraggable

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

    /// <inheritdoc/>
    public void OnDragEnd(IDragReceivable receivable, RectTransform target)
    {
        // If ended on no receiver, return to original position
        if (receivable == null)
        {
            // If ended on nothing, drop
            if (target == null)
            {
                Inventory.Instance.DropItem(this.slotIndex);
                this.ClearSlot();
            }

            this.ReturnToPosition();
        }

        this.DragEnd();
    }

    public void DragEnd()
    {
        // Destroy filling child
        if (this.fillingChild != null)
            Destroy(this.fillingChild);

        this.canvasGroup.blocksRaycasts = true;
        this.SetBackgroundAlpha(1f);
    }

    public void ReturnToPosition()
    {
        this.transform.SetParent(this.originalParent);
        this.transform.SetSiblingIndex(this.slotIndex);
    }

    #endregion

    #region IDragReceivable

    /// <inheritdoc/>
    public void OnDragReceive(InventorySlot slot)
    {
        Inventory.Instance.SwapPlace(this.slotIndex, slot.slotIndex);

        slot.transform.SetParent(slot.originalParent);
        slot.transform.SetSiblingIndex(this.slotIndex);

        this.transform.SetSiblingIndex(slot.slotIndex);

        (this.slotIndex, slot.slotIndex) = (slot.slotIndex, this.slotIndex);
    }

    /// <inheritdoc/>
    public void OnDragLeave(InventorySlot slot) { }

    #endregion

    #region IHoverable

    /// <inheritdoc/>
    public ItemData GetData() => Inventory.Instance.GetItem(this.slotIndex);

    #endregion
}