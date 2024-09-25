using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(RectTransform))]
[RequireComponent(typeof(DragAndDropHandler))]
public class InventorySlot : MonoBehaviour
{
    [SerializeField] Image itemImage;
    [SerializeField] TextMeshProUGUI quantity;
    [SerializeField] Graphic background;

    private void Awake()
    {
        DragAndDropHandler handler = GetComponent<DragAndDropHandler>();

        handler.OnDragStart += this.OnDragStart;
        handler.OnDragEnd += this.OnDragEnd;

        canvasParent = this.GetComponentInParent<Canvas>().transform;
    }

    public void SetSlot(Sprite sprite, uint qty = 1)
    {
        if (sprite == null)
            return;

        itemImage.sprite = sprite;
        quantity.text = "x" + qty.ToString();
        Color itemColor = itemImage.color;
        itemColor.a = 1f;
        itemImage.color = itemColor;
    }

    public void ClearSlot()
    {
        Color itemColor = itemImage.color;
        itemColor.a = 0f;
        itemImage.color = itemColor;
        quantity.text = "";
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
    [SerializeField]
    private CanvasGroup canvasGroup;

    private Transform originalParent;
    private int originalIndex;
    private GameObject fillingChild;
    private Transform canvasParent;

    private void OnDragStart()
    {
        // Set up slot for drag
        originalParent = transform.parent;
        originalIndex = transform.GetSiblingIndex();
        canvasGroup.blocksRaycasts = false;

        // Add ghost slot
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
            //Debug.Log(raycasts.Count);
            //if (!RectTransformUtility.RectangleContainsScreenPoint((RectTransform)originalParent, Input.mousePosition, Camera.main))
            //{
            //    Inventaire.Instance.DropItem(originalIndex);
            //}
            if(raycasts.Count == 0)
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