using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace UIModule.Menus
{
    public class InventoryMenu : AnimatedMenu
    {
        public InventoryUI inventoryUI;

        public override IEnumerator Open()
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            inventoryUI.UpdateSelf();
            yield return base.Open();
            LayoutRebuilder.ForceRebuildLayoutImmediate(inventoryUI.GetComponent<RectTransform>());
        }

        public override IEnumerator Close()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            yield return base.Close();
        }
    }
}
