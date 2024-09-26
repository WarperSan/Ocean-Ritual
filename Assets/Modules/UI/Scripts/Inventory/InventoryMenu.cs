using System.Collections;
using UnityEngine;

namespace UIModule.Menus
{
    public class InventoryMenu : AnimatedMenu
    {
        public override IEnumerator Open()
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            yield return base.Open();
        }

        public override IEnumerator Close()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            yield return base.Close();
        }
    }
}