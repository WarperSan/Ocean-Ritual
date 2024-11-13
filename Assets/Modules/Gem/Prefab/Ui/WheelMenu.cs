using BlacksmithModule;
using UnityEngine.UI;
using UnityEngine;
using System.Collections;
namespace UIModule.Menus
{
    public class WheelMenu : AnimatedMenu
    {
        public InventoryUI inventoryUI;

      
        
        /// <inheritdoc/>
        public override IEnumerator Open()
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            inventoryUI.UpdateSelf();

            yield return base.Open();
        }

        /// <inheritdoc/>
        public override IEnumerator Close()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            
            TransitionCam.Instance.SwitchToCamA();
            yield return base.Close();
            
        }

        public void CloseButton() => UIManager.Close<WheelMenu>();

        #region Page

        [Header("Page")]
        [SerializeField]
        private RectMask2D pageMask;

        public void TogglePageMask(bool isEnable) => this.pageMask.enabled = isEnable;

        #endregion
    }
}
