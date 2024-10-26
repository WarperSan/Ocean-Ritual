using ExtensionsModule;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

        #region Letter

        [Header("Letter")]
        [SerializeField]
        private Sprite letterCloseBottom;

        [SerializeField]
        private Sprite letterOpenBottom;

        [SerializeField]
        private Graphic letterTop;

        [SerializeField]
        private Image letterBottom;

        public void OpenLetter()
        {
            this.letterTop.SetAlpha(1f);
            this.letterBottom.sprite = this.letterOpenBottom;
        }

        public void CloseLetter()
        {
            this.letterTop.SetAlpha(0f);
            this.letterBottom.sprite = this.letterCloseBottom;
        }

        #endregion
    }
}

