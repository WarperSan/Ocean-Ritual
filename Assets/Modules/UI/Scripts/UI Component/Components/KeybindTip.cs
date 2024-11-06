using DhafinFawwaz.AnimationUILib;
using ExtensionsModule;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UtilsModule;

namespace UIModule.Components
{
    public class KeybindTip : Singleton<KeybindTip>
    {
        #region Fields

        [Header("Fields")]
        [SerializeField]
        private Image icon;

        [SerializeField]
        private Sprite[] images;
        private readonly Dictionary<string, Sprite> iconsForName = new();

        [SerializeField]
        private AnimationUI OpenAnimation;

        [SerializeField]
        private AnimationUI CloseAnimation;

        #endregion

        private void Start()
        {
            foreach (Sprite item in this.images)
                this.iconsForName.Add(item.name, item);
        }

        #region Controller Mode

        public bool IsKeyboard { get; private set; } = true;

        #endregion

        #region Key

        private Coroutine showKeyCoroutine = null;

        public static void ShowKey(KeyCode key)
        {
            if (Instance.showKeyCoroutine != null)
                Instance.StopCoroutine(Instance.showKeyCoroutine);

            Instance.showKeyCoroutine = Instance.StartCoroutine(Instance.ShowKeyCoroutine(key));
        }

        private IEnumerator ShowKeyCoroutine(KeyCode key)
        {
            yield return this.OpenAnimation.PlayAnimation();

            int clickCount = 10;

            while (clickCount > 0)
            {
                yield return new WaitForSeconds(1f);

                this.SetKey(key, true);

                yield return new WaitForSeconds(1f);

                this.SetKey(key, false);

                clickCount--;
            }

            yield return this.CloseAnimation.PlayAnimation();

            this.showKeyCoroutine = null;
        }

        private void SetKey(KeyCode key, bool isPressed)
        {
            string inputMethod = this.IsKeyboard ? "keyboard" : "xbox";
            string keyName = key.ToString().ToLower();
            string state = isPressed ? "_pressed" : "";

            this.SetKey($"{inputMethod}_{keyName}{state}");
        }

        private void SetKey(string name)
        {
            if (this.iconsForName.TryGetValue(name, out Sprite sprite))
            {
                this.icon.sprite = sprite;
            }
        }

        #endregion

        #region Singleton

        /// <inheritdoc/>
        protected override bool DestroyOnLoad => true;

        #endregion
    }
}