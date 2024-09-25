using DhafinFawwaz.AnimationUILib;
using System.Collections;
using UnityEngine;

namespace UIModule.Menus
{
    public class InventoryMenu : UIMenu
    {
        [SerializeField] AnimationUI openAnimation;
        [SerializeField] AnimationUI closeAnimation;

        public override IEnumerator Open()
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            openAnimation.Play();
            yield return null;
        }

        public override IEnumerator Close()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            closeAnimation.Play();
            yield return null;
        }
    }
}