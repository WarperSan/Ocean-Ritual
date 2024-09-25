using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(RectTransform))]
public class InventorySlot : MonoBehaviour
{
    [SerializeField] Image itemImage;
    [SerializeField] TextMeshProUGUI quantity;
    [SerializeField] Graphic background;

    private void Awake()
    {
        dragAndDropHandler.OnDragStart += this.OnDragStart;
        dragAndDropHandler.OnDragEnd += this.OnDragEnd;

        canvasParent = this.GetComponentInParent<Canvas>().transform;
    }

    /// <summary>
    ///  Sets the inventory slot with the sprite and quantity of an item
    /// </summary>
    public void SetSlot(Sprite sprite, uint qty = 1, bool isStackable = true)
    {
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
    int originalIndex;
    GameObject fillingChild;
    Transform canvasParent;

    private void OnDragStart()
    {
        // Set up slot for drag
        originalParent = transform.parent;
        originalIndex = transform.GetSiblingIndex();
        canvasGroup.blocksRaycasts = false;

        // Adds temporary ghost slot
        fillingChild = Instantiate(gameObject, transform.parent);
        fillingChild.GetComponent<CanvasGroup>().alpha = 0.3f;
        fillingChild.transform.SetSiblingIndex(originalIndex);

        transform.SetParent(canvasParent);

        this.SetBackgroundAlpha(0f);
    }

    private void OnDragEnd(List<RaycastResult> raycasts)
    {
        GameObject firstTarget = null;

        if (raycasts.Count > 0)
            firstTarget = raycasts[0].gameObject;

        // Check if hovering another slot
        if (firstTarget != null && firstTarget.TryGetComponent(out InventorySlot targetSlot))
        {
            int targetIndex = targetSlot.transform.GetSiblingIndex();

            Inventaire.Instance.SwapPlace(originalIndex, targetIndex);

            transform.SetParent(originalParent);
            transform.SetSiblingIndex(targetIndex);

            targetSlot.transform.SetSiblingIndex(originalIndex);

            originalIndex = targetIndex;
        }
        else
        {
            if (raycasts.Count == 0)
            {
                Inventaire.Instance.DropItem(originalIndex);
                this.ClearSlot();
            }

            transform.SetParent(originalParent);
            transform.SetSiblingIndex(originalIndex);
        }

        if (fillingChild != null)
            Destroy(fillingChild);

        canvasGroup.blocksRaycasts = true;

        this.SetBackgroundAlpha(1f);
    }

    #endregion Drag
}