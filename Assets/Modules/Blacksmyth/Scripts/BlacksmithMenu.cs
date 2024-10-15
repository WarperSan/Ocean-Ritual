using BlacksmithModule;
using System.Collections;
using UnityEngine;

namespace UIModule.Menus
{
    public class BlacksmithMenu : AnimatedMenu
    {
        public InventoryUI inventoryUI;
        public BlacksmithGemSlot gemSlot;

        /// <inheritdoc/>
        public override IEnumerator Open()
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            inventoryUI.UpdateSelf();

            yield return base.Open();
        }

        /// <inheritdoc/>
        public override IEnumerator Close()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            yield return base.Close();
            gemSlot.ClearGem(true);
        }

        public void CloseButton() => UIManager.Close<BlacksmithMenu>();
    }
}
