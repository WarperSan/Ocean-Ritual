
using System.Collections;
using UnityEngine;

namespace UIModule.Menus
{
    public class InventoryMenu : UIMenu
    {
        public override IEnumerator Open()
        {
            yield return base.Open();
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
}