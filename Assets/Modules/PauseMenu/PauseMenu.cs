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

        [SerializeField]
        private AudioClip openSound;

        [SerializeField]
        private AudioClip closeSound;

        /// <inheritdoc/>
        public override IEnumerator Open()
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            SoundManager.Instance?.PlaySound(openSound, SoundType.UI);

            yield return base.Open();

            resumeBtn.OnClick.AddListener(ResumeButton);
            mainMenuBtn.OnClick.AddListener(MainMenuButton);
            exitBtn.OnClick.AddListener(ExitButton);
        }

        /// <inheritdoc/>
        public override IEnumerator Close()
        {
            resumeBtn.OnClick.RemoveListener(ResumeButton);
            mainMenuBtn.OnClick.RemoveListener(MainMenuButton);
            exitBtn.OnClick.RemoveListener(ExitButton);

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            SoundManager.Instance?.PlaySound(closeSound, SoundType.UI);

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

        private void MainMenuButton() => mainMenuTransition.Play();

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
            letterTop.SetAlpha(1f);
            letterBottom.sprite = letterOpenBottom;
        }

        public void CloseLetter()
        {
            letterTop.SetAlpha(0f);
            letterBottom.sprite = letterCloseBottom;
        }

        #endregion
    }
}