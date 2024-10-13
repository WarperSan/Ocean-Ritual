using System.Collections;
using UnityEngine;

namespace UIModule.Menus
{
    public class BlacksmithMenu : AnimatedMenu
    {
        /// <inheritdoc/>
        public override IEnumerator Open()
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            yield return base.Open();
        }

        /// <inheritdoc/>
        public override IEnumerator Close()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            yield return base.Close();
        }

        public void CloseButton() => UIManager.Toggle<BlacksmithMenu>();
    }
}
