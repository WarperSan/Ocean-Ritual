using ExtensionsModule;
using UIModule;
using UIModule.Interfaces;
using UIModule.Menus;
using UnityEngine;
using UnityEngine.UI;

namespace BlacksmithModule
{
    public class BlacksmithGemSlot : UIComponent, IDragReceivable<InventorySlot>, IDraggable
    {
        private GemData gem = null;

        [SerializeField]
        private BlacksmithMenu menu;

        private void Start()
        {
            this.canvasParent = this.GetComponentInParent<Canvas>().transform;
        }

        #region Fields

        [Header("Fields")]
        [SerializeField] Image Icon;
        [SerializeField] Graphic background;
        public int slotIndex;

        [SerializeField]
        private CreationCase creationCase;

        #endregion

        public void ReceiveGem(GemData gem)
        {
            this.enabled = true;
            this.gem = gem;
            this.Icon.sprite = gem.sprite;
            this.Icon.SetAlpha(1);
            TestBlackSmith.Instance.SetData(gem);
            this.creationCase.CreateUi(this.gem.Shape.GetForme());
            
        }

        public void ClearGem()
        {
            this.Icon.SetAlpha(0);
            this.enabled = false;
            this.gem = null;
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

        #region IDragReceivable

        /// <inheritdoc/>
        public void OnDragReceive(InventorySlot slot)
        {
            if (this.gem != null)
            {
                Inventory.Instance.SwapPlace(this.slotIndex, slot.slotIndex);
                menu.inventoryUI.UpdateSelf();

            }

            this.ReceiveGem(slot.GetData() as GemData);
            //Inventory.Instance.DropItem(slot.slotIndex);
            this.slotIndex = slot.slotIndex;
            slot.ClearSlot();
            slot.ReturnToPosition();
        }

        /// <inheritdoc/>
        public void OnDragLeave(InventorySlot slot)
        {
            Debug.Log("LEAVE");
        }

        /// <inheritdoc/>
        bool IDragReceivable<InventorySlot>.CanReceiveDraggable(InventorySlot slot) => slot.GetData() is GemData;

        #endregion

        #region IDraggable
        [Header("Drag")]
        [SerializeField] CanvasGroup canvasGroup;

        Transform originalParent;
        GameObject fillingChild;
        Transform canvasParent;
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
            Debug.Log(slotIndex);
        }
        public void OnDragEnd(IDragReceivable receivable, RectTransform target)
        {
            if (receivable == null)
            {
                InventorySlot slot = null;

                if (target != null)
                    slot = target.GetComponent<InventorySlot>();

                if (slot != null)
                {
                    Inventory.Instance.SwapPlace(this.slotIndex, slot.slotIndex);
                    menu.inventoryUI.UpdateSelf();
                    this.ClearGem();
                }

                this.ReturnToPosition();
            }

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
    }
}