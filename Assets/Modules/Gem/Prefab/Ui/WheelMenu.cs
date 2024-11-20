using BlacksmithModule;
using UnityEngine.UI;
using UnityEngine;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
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
            GetAllUpgradeableItem();
            TransitionCam.Instance.SwitchToCamA();

            yield return base.Close();
            
        }
        private List<Equipment> forgeableItems = new();
        private void GetAllUpgradeableItem()
        {
            //Debug.Log("Nombre d'objets forgeables trouvés : " + forgeableItems.Count);
            this.forgeableItems = FindObjectsOfType<MonoBehaviour>().OfType<Equipment>().ToList();
            //Debug.Log("Nombre d'objets forgeables trouvés après recherche : " + forgeableItems.Count);

            foreach (var item in forgeableItems)
            {
                item.UpdateStat();
                Debug.Log("Nom de l'objet : " + ((MonoBehaviour)item).gameObject.name);
            }
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
