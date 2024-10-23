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
            //Time.timeScale = 0f;

            yield return base.Open();
        }

        public override IEnumerator Close()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            //Time.timeScale = 1f;

            yield return base.Close();
        }

        public void ResumeButton()
        {
            UIManager.Close(this);
        }

        public void ExitButton()
        {
           Application.Quit();
        }
    }
}

