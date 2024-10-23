using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UIModule.Menus
{
    public class PauseMenu : AnimatedMenu
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

