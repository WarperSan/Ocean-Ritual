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
        private GemData gem;
        private int slotIndex = -1;

        [SerializeField] private BlacksmithMenu menu;
        [SerializeField] private Image Icon;
        [SerializeField] private Graphic background;
        [SerializeField] private CreationCase creationCase;
        [SerializeField] private CanvasGroup canvasGroup;

        private Transform originalParent;
        private GameObject fillingChild;
        private Transform canvasParent;

        private void Start()
        {
            canvasParent = GetComponentInParent<Canvas>().transform;
        }

        #region GemSlot

        public void ReceiveGem(GemData gem)
        {
            this.gem = gem;
            Icon.sprite = gem.sprite;
            Icon.SetAlpha(1);
            TestBlackSmith.Instance.SetData(gem);
            creationCase.CreateUi(gem.Shape.GetForme());
            enabled = true;
        }

        public void ClearGem(bool returnToInventory)
        {
            if (returnToInventory && gem != null)
            {
                Inventory.Instance.AddItem(gem, slotIndex);
            }
            ResetGemSlot();
        }

        private void ResetGemSlot()
        {
            Icon.SetAlpha(0);
            enabled = false;
            gem = null;
            slotIndex = -1;
        }

        public void SetBackgroundAlpha(float alpha)
        {
            Color bgColor = background.color;
            bgColor.a = Mathf.Clamp01(alpha);
            background.color = bgColor;
        }

        #endregion

        #region IDragReceivable

        public void OnDragReceive(InventorySlot slot)
        {
            if (slot.GetData() is GemData gem)
            {
                Inventory.Instance.DropItem(slot.slotIndex);
                if (slotIndex != -1)
                {
                    Inventory.Instance.AddItem(this.gem, slot.slotIndex);
                }
                ReceiveGem(gem);
                slotIndex = slot.slotIndex;
                menu.inventoryUI.UpdateSelf();
                TestBlackSmith.Instance.UpdateUI();
                slot.DragEnd();
                Destroy(slot.gameObject);
            }
        }

        public void OnDragLeave(InventorySlot slot)
        {
            Debug.Log("LEAVE");
        }

        bool IDragReceivable<InventorySlot>.CanReceiveDraggable(InventorySlot slot) => slot.GetData() is GemData;

        #endregion

        #region IDraggable

        public void OnDragStart()
        {
            originalParent = transform.parent;
            canvasGroup.blocksRaycasts = false;

            fillingChild = Instantiate(gameObject, transform.parent);
            var childCanvasGroup = fillingChild.GetComponent<CanvasGroup>();
            if (childCanvasGroup != null)
            {
                childCanvasGroup.alpha = 0.3f;
            }
            fillingChild.transform.SetSiblingIndex(transform.GetSiblingIndex());

            transform.SetParent(canvasParent);
            SetBackgroundAlpha(0f);
        }

        public void OnDragEnd(IDragReceivable receivable, RectTransform target)
        {
            if (receivable == null)
            {
                HandleDragEndWithoutReceivable(target);
            }

            DestroyFillingChild();
            canvasGroup.blocksRaycasts = true;
            SetBackgroundAlpha(1f);
        }

        private void HandleDragEndWithoutReceivable(RectTransform target)
        {
            var slot = target?.GetComponent<InventorySlot>();
            if (slot != null)
            {
                Inventory.Instance.AddItem(gem, slot.slotIndex);
                menu.inventoryUI.UpdateSelf();
                ClearGem(false);
            }
            ReturnToPosition();
        }

        private void DestroyFillingChild()
        {
            if (fillingChild != null)
            {
                Destroy(fillingChild);
            }
        }

        public void ReturnToPosition()
        {
            transform.SetParent(originalParent);
            transform.SetSiblingIndex(slotIndex);
        }

        #endregion
    }
}
