using UIModule.Interfaces;
using UIModule;
using UnityEngine;
using UIModule.Menus;
using UnityEngine.UI;
using ExtensionsModule;

public enum CaseRole
{
    INPUT,
    OUTPUT
}

public class FusionCase : UIComponent, IDragReceivable<InventorySlot>, IDraggable
{
    private int slotIndex = -1;
    public GemData gem = null;
    public CaseRole role = CaseRole.INPUT;

    #region Fields

    [Header("Fields")]
    [SerializeField]
    private BlacksmithMenu menu;

    [SerializeField]
    private Image Icon;

    #endregion

    private void Awake()
    {
        this.enabled = this.role == CaseRole.INPUT;
    }

    public void ReceiveGem(GemData gem)
    {
        this.gem = gem;
        this.Icon.sprite = gem.sprite;
        this.Icon.SetAlpha(1f);

        this.enabled = true;
    }

    public void ClearGem()
    {
        slotIndex = -1;
        gem = null;
        this.Icon.SetAlpha(0f);
        this.enabled = false;
    }

    #region IDragReceivable

    /// <inheritdoc/>
    public void OnDragReceive(InventorySlot slot)
    {
        if (slot.GetData() is not GemData gem)
            return;

        Inventory.Instance.DropItem(slot.slotIndex);

        if (this.slotIndex != -1)
            Inventory.Instance.AddItem(this.gem, slot.slotIndex);

        this.ReceiveGem(gem);
        this.slotIndex = slot.slotIndex;

        this.menu.inventoryUI.UpdateSelf();
        slot.DragEnd();
        Destroy(slot.gameObject);
    }

    /// <inheritdoc/>
    public void OnDragLeave(InventorySlot slot) { }

    /// <inheritdoc/>
    public bool CanReceiveDraggable(InventorySlot slot)
    {
        if (this.role != CaseRole.INPUT)
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
        this.originalParent = this.transform.parent;

        this.fillingChild = Instantiate(this.gameObject, this.transform.parent);

        if (this.fillingChild.TryGetComponent(out CanvasGroup childCanvasGroup))
            childCanvasGroup.alpha = 0.3f;

        this.fillingChild.transform.SetSiblingIndex(this.transform.GetSiblingIndex());

        this.transform.SetParent(this.canvasParent);
    }

    /// <inheritdoc/>
    public void OnDragEnd(IDragReceivable receivable, RectTransform target)
    {
        if (receivable == null && target != null)
        {
            if (target.TryGetComponent(out InventorySlot slot))
            {
                Inventory.Instance.AddItem(this.gem, slot.slotIndex);
                this.menu.inventoryUI.UpdateSelf();
                this.ClearGem();
            }

            this.transform.SetParent(this.originalParent);
            this.transform.localPosition = this.fillingChild.transform.localPosition;
        }

        if (this.fillingChild != null)
            Destroy(this.fillingChild);
    }

    #endregion
}
