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
    }

    /// <summary>
    /// Clears an inventory slot
    /// </summary>
    public void ClearSlot()
    {
        this.itemImage.SetAlpha(0f);
        this.quantity.text = "";

        this.enabled = false; // If the slot is cleared, cannot be dragged
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

        this.background.SetAlpha(0f);
    }

    /// <inheritdoc/>
    public void OnDragEnd(IDragReceivable receivable, RectTransform target)
    {
        if (receivable == null)
        {
            if (target == null)
            {
                Inventory.Instance.DropItem(this.slotIndex);
                this.ClearSlot();
            }

            this.transform.SetParent(this.originalParent);
            this.transform.SetSiblingIndex(this.slotIndex);
        }

        if (this.fillingChild != null)
            Destroy(this.fillingChild);

        this.canvasGroup.blocksRaycasts = true;

        this.background.SetAlpha(1f);
    }

    #endregion

    #region IDragReceivable

    /// <inheritdoc/>
    public void OnDragReceive(InventorySlot draggable)
    {
        Inventory.Instance.SwapPlace(this.slotIndex, draggable.slotIndex);

        draggable.transform.SetParent(draggable.originalParent);
        draggable.transform.SetSiblingIndex(this.slotIndex);

        this.transform.SetSiblingIndex(draggable.slotIndex);

        (draggable.slotIndex, this.slotIndex) = (this.slotIndex, draggable.slotIndex);
    }

    /// <inheritdoc/>
    public void OnDragLeave(InventorySlot draggable) { }

    #endregion

    #region IHoverable

    /// <inheritdoc/>
    public ItemData GetData() => Inventory.Instance.GetItem(this.slotIndex);

    #endregion
}