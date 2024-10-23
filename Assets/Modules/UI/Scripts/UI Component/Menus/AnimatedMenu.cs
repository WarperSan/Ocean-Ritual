using DhafinFawwaz.AnimationUILib;
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
        public override IEnumerator Open() 
        {
            bool hasAnimationEnded = false;
            void callback() => hasAnimationEnded = true;
            
            this.openAnimation.OnAnimationEnded += callback;
            this.openAnimation.Play();

            while (!hasAnimationEnded)
                yield return null;

            this.openAnimation.OnAnimationEnded -= callback;
        }

        /// <inheritdoc/>
        public override IEnumerator Close() 
        {
            bool hasAnimationEnded = false;
            void callback() => hasAnimationEnded = true;
            
            this.closeAnimation.OnAnimationEnded += callback;
            this.closeAnimation.Play();

            while (!hasAnimationEnded)
                yield return null;

            this.closeAnimation.OnAnimationEnded -= callback;
        }
    }
}

