using ExtensionsModule;
using GemModule.UI;
using UIModule;
using UIModule.Interfaces;
using UIModule.Menus;
using UnityEngine;
using UnityEngine.UI;

namespace BlacksmithModule
{
    public class BlacksmithGemSlot : UIComponent, IDragReceivable<InventorySlot>, IDraggable
    {
        private GemData gem;
        private int slotIndex = -1;

        private void Start() => canvasParent = GetComponentInParent<Canvas>().transform;

        #region Fields

        [Header("Fields")]
        [SerializeField]
        private BlacksmithMenu menu;

        [SerializeField]
        private Image Icon;

        [SerializeField]
        private Graphic background;

        [SerializeField]
        private Graphic frame;

        [SerializeField]
        private ShowGemShape showGemShape;

        [SerializeField]
        private CanvasGroup canvasGroup;

        #endregion

        #region GemSlot

        public void ReceiveGem(GemData gem)
        {
            this.gem = gem;
            Icon.sprite = gem.sprite;
            Icon.SetAlpha(1);
            enabled = true;

            TestBlackSmith.Instance.SetData(gem);
        }

        public void Confirm() => TestBlackSmith.Instance.ConfirmChoice();

        public void ClearGem(bool returnToInventory)
        {
            if (returnToInventory && gem != null)
                Inventory.Instance.AddItem(gem, slotIndex);

            gem = null;
            Icon.sprite = null;
            Icon.SetAlpha(0);
            enabled = false;
            slotIndex = -1;

            TestBlackSmith.Instance.SetData(null);
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
            canvasGroup.blocksRaycasts = false;

            fillingChild = Instantiate(gameObject, transform.parent);

            if (fillingChild.TryGetComponent(out CanvasGroup childCanvasGroup))
                childCanvasGroup.alpha = 0.3f;

            fillingChild.transform.SetSiblingIndex(transform.GetSiblingIndex());

            transform.SetParent(canvasParent);
            background.SetAlpha(0f);
            frame.SetAlpha(0f);
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
                    ClearGem(false);
                }

                transform.SetParent(originalParent);
                transform.SetSiblingIndex(slotIndex);
            }

            if (fillingChild != null)
                Destroy(fillingChild);

            canvasGroup.blocksRaycasts = true;
            background.SetAlpha(1f);
            frame.SetAlpha(1f);
        }

        #endregion

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
            TestBlackSmith.Instance.UpdateUI();
            slot.DragEnd();
            Destroy(slot.gameObject);
        }

        /// <inheritdoc/>
        public void OnDragLeave(InventorySlot slot) { }

        /// <inheritdoc/>
        bool IDragReceivable<InventorySlot>.CanReceiveDraggable(InventorySlot slot) => slot.GetData() is GemData;

        #endregion
    }
}