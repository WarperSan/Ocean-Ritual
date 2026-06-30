using UIModule.Interfaces;
using UIModule;
using UnityEngine;
using UIModule.Menus;
using UnityEngine.UI;
using ExtensionsModule;

public enum CaseRole
{
    INPUT,
    OUTPUT,
}

public class FusionCase : UIComponent, IDragReceivable<InventorySlot>, IDraggable
{
    private int slotIndex = -1;
    public GemData gem;
    public CaseRole role = CaseRole.INPUT;

    #region Fields

    [Header("Fields")]
    [SerializeField]
    private BlacksmithMenu menu;

    [SerializeField]
    private Image Icon;

    #endregion

    private void Awake() => enabled = role == CaseRole.INPUT;

    public void ReceiveGem(GemData gem)
    {
        this.gem = gem;
        Icon.sprite = gem.sprite;
        Icon.SetAlpha(1f);

        enabled = true;
    }

    public void ClearGem()
    {
        slotIndex = -1;
        gem = null;
        Icon.SetAlpha(0f);
        enabled = false;
    }

    #region IDragReceivable

    /// <inheritdoc/>
    public void OnDragReceive(InventorySlot slot)
    {
        if (slot.GetData() is not GemData gem)
            return;

        Inventory.Instance.DropItem(slot.slotIndex);

        if (slotIndex != -1)
            Inventory.Instance.AddItem(this.gem, slot.slotIndex);

        ReceiveGem(gem);
        slotIndex = slot.slotIndex;

        menu.inventoryUI.UpdateSelf();
        slot.DragEnd();
        Destroy(slot.gameObject);
    }

    /// <inheritdoc/>
    public void OnDragLeave(InventorySlot slot) { }

    /// <inheritdoc/>
    public bool CanReceiveDraggable(InventorySlot slot)
    {
        if (role != CaseRole.INPUT)
            return false;

        return slot.GetData() is GemData;
    }

    #endregion

    #region IDraggable

    private Transform originalParent;
    private GameObject fillingChild;
    private Transform canvasParent;

    /// <inheritdoc/>
    public void OnDragStart()
    {
        originalParent = transform.parent;

        fillingChild = Instantiate(gameObject, transform.parent);

        if (fillingChild.TryGetComponent(out CanvasGroup childCanvasGroup))
            childCanvasGroup.alpha = 0.3f;

        fillingChild.transform.SetSiblingIndex(transform.GetSiblingIndex());

        transform.SetParent(canvasParent);
    }

    /// <inheritdoc/>
    public void OnDragEnd(IDragReceivable receivable, RectTransform target)
    {
        if (receivable == null && target != null)
        {
            if (target.TryGetComponent(out InventorySlot slot))
            {
                Inventory.Instance.AddItem(gem, slot.slotIndex);
                menu.inventoryUI.UpdateSelf();
                ClearGem();
            }

            transform.SetParent(originalParent);
            transform.localPosition = fillingChild.transform.localPosition;
        }

        if (fillingChild != null)
            Destroy(fillingChild);
    }

    #endregion
}