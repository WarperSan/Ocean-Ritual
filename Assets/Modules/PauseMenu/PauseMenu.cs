using DhafinFawwaz.AnimationUILib;
using DhafinFawwaz.AnimationUILib.Demo;
using ExtensionsModule;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace UIModule.Menus
{
    public class PauseMenu : AnimatedMenu
    {
        #region AnimatedMenu

        [SerializeField] string openSound;
        [SerializeField] string closeSound;

        /// <inheritdoc/>
        public override IEnumerator Open()
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            //Time.timeScale = 0f;

            SoundManager.Instance.PlaySound(openSound, SoundType.UI);

            yield return base.Open();

            this.resumeBtn.OnClick.AddListener(this.ResumeButton);
            this.mainMenuBtn.OnClick.AddListener(this.MainMenuButton);
            this.exitBtn.OnClick.AddListener(this.ExitButton);
        }

        /// <inheritdoc/>
        public override IEnumerator Close()
        {
            this.resumeBtn.OnClick.RemoveListener(this.ResumeButton);
            this.mainMenuBtn.OnClick.RemoveListener(this.MainMenuButton);
            this.exitBtn.OnClick.RemoveListener(this.ExitButton);

            SoundManager.Instance.PlaySound(closeSound, SoundType.UI);

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            //Time.timeScale = 1f;

            yield return base.Close();
        }

        #endregion

        #region Buttons

        [Header("Buttons")]
        [SerializeField]
        private ButtonUI resumeBtn;

        [SerializeField]
        private ButtonUI mainMenuBtn;

        [SerializeField]
        protected ButtonUI exitBtn;

        [SerializeField]
        private AnimationUI mainMenuTransition;

        private void ResumeButton() => UIManager.Close(this);

        private void MainMenuButton() => this.mainMenuTransition.Play();

        private void ExitButton() => Application.Quit();

        #endregion

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

