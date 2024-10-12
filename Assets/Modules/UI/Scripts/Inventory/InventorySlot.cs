using System.Collections.Generic;
using TMPro;
using UIModule.Interfaces;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(RectTransform))]
public class InventorySlot : MonoBehaviour, IHoverable
{
    [SerializeField] Image itemImage;
    [SerializeField] TextMeshProUGUI quantity;
    [SerializeField] Graphic background;
    public int slotIndex;

    private void Awake()
    {
        dragAndDropHandler.OnDragStart += this.OnDragStart;
        dragAndDropHandler.OnDragEnd += this.OnDragEnd;

        canvasParent = this.GetComponentInParent<Canvas>().transform;
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

        itemImage.sprite = sprite;

        quantity.text = isStackable ? "x" + qty.ToString() : "";
        Color itemColor = itemImage.color;
        itemColor.a = 1f;
        itemImage.color = itemColor;

        dragAndDropHandler.enabled = true;
    }

    /// <summary>
    /// Clears an inventory slot
    /// </summary>
    public void ClearSlot()
    {
        Color itemColor = itemImage.color;
        itemColor.a = 0f;
        itemImage.color = itemColor;
        quantity.text = "";

        dragAndDropHandler.enabled = false; // If the slot is cleared, cannot be dragged
    }

    /// <summary>
    /// Sets the alpha of the background for this slot
    /// </summary>
    /// <param name="alpha">Value between 0 and 1</param>
    public void SetBackgroundAlpha(float alpha)
    {
        Color bgColor = background.color;
        bgColor.a = Mathf.Clamp01(alpha);
        background.color = bgColor;
    }

    #region Drag

    [Header("Drag")]
    [SerializeField] CanvasGroup canvasGroup;

    [SerializeField] DragAndDropHandler dragAndDropHandler;
    Transform originalParent;
    GameObject fillingChild;
    Transform canvasParent;

    private void OnDragStart()
    {
        // Set up slot for drag
        originalParent = transform.parent;
        canvasGroup.blocksRaycasts = false;

        // Adds temporary ghost slot
        fillingChild = Instantiate(gameObject, transform.parent);
        fillingChild.GetComponent<CanvasGroup>().alpha = 0.3f;
        fillingChild.transform.SetSiblingIndex(transform.GetSiblingIndex());

        transform.SetParent(canvasParent);

        this.SetBackgroundAlpha(0f);
    }

    private void OnDragEnd(List<RaycastResult> raycasts)
    {
        GameObject firstTarget = null;

        if (raycasts.Count > 0)
            firstTarget = raycasts[0].gameObject;

        // if (firstTarget != null && firstTarget.TryGetComponent(out BlacksmithGemSlot targetGemSlot))
        // {
        //     targetGemSlot.ReceiveGem((GemData)GetItem());
        // }

        // Check if hovering another slot
        if (firstTarget != null && firstTarget.TryGetComponent(out InventorySlot targetSlot))
        {
            Inventory.Instance.SwapPlace(slotIndex, targetSlot.slotIndex);

            transform.SetParent(originalParent);
            transform.SetSiblingIndex(targetSlot.slotIndex);

            targetSlot.transform.SetSiblingIndex(slotIndex);

            (slotIndex, targetSlot.slotIndex) = (targetSlot.slotIndex, slotIndex);
        }
        else
        {
            if (raycasts.Count == 0)
            {
                Inventory.Instance.DropItem(slotIndex);
                this.ClearSlot();
            }

            transform.SetParent(originalParent);
            transform.SetSiblingIndex(slotIndex);
        }

        if (fillingChild != null)
            Destroy(fillingChild);

        canvasGroup.blocksRaycasts = true;

        this.SetBackgroundAlpha(1f);
    }

    #endregion Drag

    #region IHoverable

    /// <inheritdoc/>
    public ItemData GetData() => Inventory.Instance.GetItem(slotIndex);

    #endregion
}