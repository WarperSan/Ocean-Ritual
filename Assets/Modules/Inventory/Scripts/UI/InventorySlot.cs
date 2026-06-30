using ExtensionsModule;
using TMPro;
using UIModule;
using UIModule.Interfaces;
using UnityEngine;
using System.Threading.Tasks;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySlot : UIComponent, IHoverable, IDraggable, IDragReceivable<InventorySlot>
{
    [SerializeField]
    private Image itemImage;

    [SerializeField]
    private TextMeshProUGUI quantity;

    [SerializeField]
    private Graphic background;

    public int slotIndex;

    private void Start() => canvasParent = GetComponentInParent<Canvas>().transform;

    /// <summary>
    ///  Sets the inventory slot with the sprite and quantity of an item
    /// </summary>
    public void SetSlot(
        int    slotIndex,
        Sprite sprite,
        uint   qty         = 1,
        bool   isStackable = true
    )
    {
        this.slotIndex = slotIndex;

        if (sprite == null)
        {
            ClearSlot();
            return;
        }

        itemImage.sprite = sprite;

        quantity.text = isStackable ? "x" + qty.ToString() : "";
        itemImage.SetAlpha(1);

        enabled = true;
    }

    /// <summary>
    /// Clears an inventory slot
    /// </summary>
    public void ClearSlot()
    {
        itemImage.SetAlpha(0);
        quantity.text = "";

        enabled = false;
    }

    public void ClearSlotCancel() => CancelDragTemporarily();

    public void SimulateMouseRelease()
    {
        // Cr�e un PointerEventData pour simuler le rel�chement
        var pointerEventData = new PointerEventData(EventSystem.current)
        {
            pointerId = -1,                 // Id de la souris gauche
            position = Input.mousePosition, // Position actuelle de la souris
        };

        // Envoie l'�v�nement "Pointer Up" pour simuler le rel�chement
        ExecuteEvents.Execute(gameObject, pointerEventData, ExecuteEvents.pointerUpHandler);
    }

    private bool dragCancelled;

    public async void CancelDragTemporarily()
    {
        // Annuler l�action de drag actuelle
        dragCancelled = true;
        enabled = false;
        canvasGroup.blocksRaycasts = false;

        // Forcer la fin du drag et simuler le rel�chement
        DragEnd();
        SimulateMouseRelease();

        await Task.Delay(100);

        while (Input.GetMouseButton(0))
            await Task.Yield();
        enabled = true;
        canvasGroup.blocksRaycasts = true;

        dragCancelled = false;
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

    #region IDraggable

    [Header("Drag")]
    [SerializeField]
    private CanvasGroup canvasGroup;

    private Transform originalParent;
    private GameObject fillingChild;
    private Transform canvasParent;
    private int childIndex;

    /// <inheritdoc/>
    public void OnDragStart()
    {
        if (dragCancelled)
            return;

        if (WheelActif())
        {
            ZoneUIHandler.Instance.GiveRefInventorySlot(this);
            ZoneUIHandler.Instance.GiveIndex(slotIndex);
        }

        // Set up slot for drag
        originalParent = transform.parent;
        canvasGroup.blocksRaycasts = false;

        childIndex = transform.GetSiblingIndex();

        // Adds temporary ghost slot
        fillingChild = Instantiate(gameObject, transform.parent);
        fillingChild.GetComponent<CanvasGroup>().alpha = 0.3f;
        fillingChild.transform.SetSiblingIndex(childIndex);

        transform.SetParent(canvasParent);

        SetBackgroundAlpha(0f);
    }

    public bool WheelActif()
    {
        var WheelObject = GameObject.FindGameObjectWithTag("Roue");

        if (WheelObject == null || !WheelObject.activeInHierarchy)
            return false;
        else
            return true;
    }

    public void TransformIntoGem()
    {
        Debug.Log("transform gemme");
        Inventory.Instance.DropItem(slotIndex);
        ClearSlot();
        ReturnToPosition();
        DragEnd();
    }

    public void Cancel()
    {
        Debug.Log("allo");
        ClearSlotCancel();
        ReturnToPosition();
        DragEnd();

        //Debug.Log("allo");
        //    this.ReturnToPosition();
        //   OnDragEnd(null, null);
        // this.canvasGroup.blocksRaycasts = true;
        // Supprime la s�lection actuelle de l'EventSystem
        // EventSystem.current.SetSelectedGameObject(null);
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
                Debug.Log("ALLO");
                Inventory.Instance.DropItem(slotIndex);
                ClearSlot();
            }

            ReturnToPosition();
        }

        DragEnd();
    }

    public void DragEnd()
    {
        // Destroy filling child
        if (fillingChild != null)
            Destroy(fillingChild);

        canvasGroup.blocksRaycasts = true;
        SetBackgroundAlpha(1f);

        if (WheelActif())
            ZoneUIHandler.Instance.NeedReset();
    }

    public void ReturnToPosition()
    {
        transform.SetParent(originalParent);
        transform.SetSiblingIndex(slotIndex);
    }

    #endregion

    #region IDragReceivable

    /// <inheritdoc/>
    public void OnDragReceive(InventorySlot slot)
    {
        Inventory.Instance.SwapPlace(slotIndex, slot.slotIndex);

        Transform parent = transform.parent;
        int index = transform.GetSiblingIndex();

        transform.SetParent(slot.originalParent);
        transform.SetSiblingIndex(slot.childIndex);

        Transform otherParent = slot.originalParent;

        // If same container but higher
        if (otherParent == parent && slotIndex > slot.slotIndex)
            index++;

        slot.transform.SetParent(parent);
        slot.transform.SetSiblingIndex(index);

        (slotIndex, slot.slotIndex) = (slot.slotIndex, slotIndex);
    }

    /// <inheritdoc/>
    public void OnDragLeave(InventorySlot slot) { }

    #endregion

    #region IHoverable

    /// <inheritdoc/>
    public ItemData GetData() => Inventory.Instance.GetItem(slotIndex);

    #endregion
}