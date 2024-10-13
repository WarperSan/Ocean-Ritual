using ExtensionsModule;
using UIModule;
using UIModule.Interfaces;
using UnityEngine;
using UnityEngine.UI;

namespace BlacksmithModule
{
    public class BlacksmithGemSlot : UIComponent, IDragReceivable<InventorySlot>
    {
        [SerializeField]
        private GemData gem;

        #region Fields

        [Header("Fields")]
        [SerializeField]
        private Image Icon;

        [SerializeField]
        private CreationCase creationCase;

        #endregion

        public void ReceiveGem(GemData gem)
        {
            this.gem = gem;
            this.Icon.sprite = gem.sprite;
            this.Icon.SetAlpha(1);
            TestBlackSmith.Instance.SetData(gem );
            this.creationCase.CreateUi(this.gem.Shape.GetForme());
        }

        public void ClearGem()
        {
            this.Icon.SetAlpha(0);
        }

        #region IDragReceivable

        /// <inheritdoc/>
        public void OnDragReceive(InventorySlot slot)
        {
            this.ReceiveGem(slot.GetData() as GemData);

            slot.ClearSlot();
            slot.ReturnToPosition();
        }

        /// <inheritdoc/>
        public void OnDragLeave(InventorySlot slot)
        {
            Debug.Log("LEAVE");
            this.ClearGem();
            this.gem = null;
        }

        /// <inheritdoc/>
        bool IDragReceivable<InventorySlot>.CanReceiveDraggable(InventorySlot slot) => slot.GetData() is GemData;

        #endregion
    }
}