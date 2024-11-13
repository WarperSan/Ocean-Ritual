using DhafinFawwaz.AnimationUILib;
using ExtensionsModule;
using System.Collections;
using UnityEngine;

namespace UIModule.Menus
{
    /// <summary>
    /// Class that represents a menu that has an opening and/or a closing animation
    /// </summary>
    [RequireComponent(typeof(RectTransform))]
    public abstract class AnimatedMenu : UIMenu
    {
        [Header("Animations")]
        [SerializeField, Tooltip("Animation played when this menu is opened")]
        private AnimationUI openAnimation;

        [SerializeField, Tooltip("Animation played when this menu is closed")]
        private AnimationUI closeAnimation;

        /// <inheritdoc/>
        public override IEnumerator Open() => this.openAnimation.PlayAnimation();

        /// <inheritdoc/>
        public override IEnumerator Close() => this.closeAnimation.PlayAnimation();
    }
}

