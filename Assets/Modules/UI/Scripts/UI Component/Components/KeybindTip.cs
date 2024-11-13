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

        #region Controller Mode

        public bool IsKeyboard { get; private set; } = true;

        #endregion

        #region Key

        private Coroutine showKeyCoroutine = null;

        public static void ShowKey(KeyCode key, string tag) => Instance.ShowKeyInstance(key, tag);

        private void ShowKeyInstance(KeyCode key, string tag)
        {
            if (this.usedTips.ContainsKey(tag))
                return;

            if (this.showKeyCoroutine != null)
                this.StopCoroutine(this.showKeyCoroutine);

            this.showKeyCoroutine = this.StartCoroutine(this.ShowKeyCoroutine(key, tag));
        }

        private IEnumerator ShowKeyCoroutine(KeyCode key, string tag)
        {
            this.usedTips[tag] = false;

            yield return this.OpenAnimation.PlayAnimation();

            bool pressed = true;

            while (this.usedTips.ContainsKey(tag) && !this.usedTips[tag])
            {
                yield return new WaitForSeconds(1f);

                this.SetKey(key, pressed);

                pressed = !pressed;
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

        #region Tips

        private readonly Dictionary<string, bool> usedTips = new();

        public static void UseTip(string tag) => Instance.usedTips[tag] = true;

        public static void DiscardTip(string tag)
        {
            // If tag not registered, skip
            if (!Instance.usedTips.ContainsKey(tag))
                return;

            // If tag used, skip
            if (Instance.usedTips[tag])
                return;

            Instance.usedTips.Remove(tag);
        }

        #endregion

        #region Singleton

        /// <inheritdoc/>
        protected override bool DestroyOnLoad => true;

        protected override void OnAwake()
        {
            foreach (Sprite item in this.images)
                this.iconsForName.Add(item.name, item);
        }

        #endregion
    }
}