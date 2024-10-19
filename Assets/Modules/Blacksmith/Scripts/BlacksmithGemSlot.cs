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

        private void Start() => this.canvasParent = this.GetComponentInParent<Canvas>().transform;

        #region Fields

        [Header("Fields")]
        [SerializeField]
        private BlacksmithMenu menu;

        [SerializeField]
        private Image Icon;

        [SerializeField]
        private Graphic background;

        [SerializeField]
        private ShowGemShape showGemShape;

        [SerializeField]
        private CanvasGroup canvasGroup;

        #endregion

        #region GemSlot

        public void ReceiveGem(GemData gem)
        {
            this.gem = gem;
            this.Icon.sprite = gem.sprite;
            this.Icon.SetAlpha(1);
            TestBlackSmith.Instance.SetData(gem);
            this.showGemShape.ShowGem(gem);
            this.enabled = true;
        }

        public void ClearGem(bool returnToInventory)
        {
            if (returnToInventory && this.gem != null)
            {
                Inventory.Instance.AddItem(this.gem, this.slotIndex);
            }

            this.ResetGemSlot();
        }

        private void ResetGemSlot()
        {
            this.Icon.SetAlpha(0);
            this.enabled = false;
            this.gem = null;
            this.slotIndex = -1;
        }

        public void SetBackgroundAlpha(float alpha)
        {
            Color bgColor = this.background.color;
            bgColor.a = Mathf.Clamp01(alpha);
            this.background.color = bgColor;
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
            this.canvasGroup.blocksRaycasts = false;

            this.fillingChild = Instantiate(this.gameObject, this.transform.parent);

            if (this.fillingChild.TryGetComponent(out CanvasGroup childCanvasGroup))
                childCanvasGroup.alpha = 0.3f;

            this.fillingChild.transform.SetSiblingIndex(this.transform.GetSiblingIndex());

            this.transform.SetParent(this.canvasParent);
            this.SetBackgroundAlpha(0f);
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
                    this.ClearGem(false);
                }

                this.transform.SetParent(this.originalParent);
                this.transform.SetSiblingIndex(this.slotIndex);
            }

            if (this.fillingChild != null)
                Destroy(this.fillingChild);

            this.canvasGroup.blocksRaycasts = true;
            this.SetBackgroundAlpha(1f);
        }

        #endregion

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
