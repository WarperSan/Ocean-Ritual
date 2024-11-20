    using ExtensionsModule;
    using TMPro;
    using UIModule;
    using UIModule.Interfaces;
    using UnityEngine;
using System.Threading.Tasks;
using UnityEngine.EventSystems;
using UnityEngine.UI;

    public class InventorySlot : UIComponent, IHoverable, IDraggable, IDragReceivable<InventorySlot>
    {
        [SerializeField] Image itemImage;
        [SerializeField] TextMeshProUGUI quantity;
        [SerializeField] Graphic background;
  
        public int slotIndex;

        private void Start()
        {
            this.canvasParent = this.GetComponentInParent<Canvas>().transform;
        }

        /// <summary>
        ///  Sets the inventory slot with the sprite and quantity of an item
        /// </summary>
        public void SetSlot(int slotIndex, Sprite sprite, uint qty = 1, bool isStackable = true)
        {
            this.slotIndex = slotIndex;
            if (sprite == null)
            {
                this.ClearSlot();
                return;
            }

            this.itemImage.sprite = sprite;

            this.quantity.text = isStackable ? "x" + qty.ToString() : "";
            this.itemImage.SetAlpha(1);

            this.enabled = true;
        }

        /// <summary>
        /// Clears an inventory slot
        /// </summary>
        public void ClearSlot()
        {
            this.itemImage.SetAlpha(0);
            this.quantity.text = "";

            this.enabled = false;
        }
    public void ClearSlotCancel()
    {

        CancelDragTemporarily();
       
    }

    public void SimulateMouseRelease()
    {
        // Crée un PointerEventData pour simuler le relâchement
        PointerEventData pointerEventData = new PointerEventData(EventSystem.current)
        {
            pointerId = -1, // Id de la souris gauche
            position = Input.mousePosition // Position actuelle de la souris
        };

        // Envoie l'événement "Pointer Up" pour simuler le relâchement
        ExecuteEvents.Execute(gameObject, pointerEventData, ExecuteEvents.pointerUpHandler);
    }
    private bool dragCancelled = false;
    public async void CancelDragTemporarily()
    {
        // Annuler l’action de drag actuelle
        dragCancelled = true;
        this.enabled = false;
        this.canvasGroup.blocksRaycasts = false;

        // Forcer la fin du drag et simuler le relâchement
        DragEnd();
        SimulateMouseRelease();

     
        await Task.Delay(100);

   

      
        while (Input.GetMouseButton(0))
        {
            await Task.Yield();
        }
        this.enabled = true;
        this.canvasGroup.blocksRaycasts = true;
    
        dragCancelled = false;
    }

    /// <summary>
    /// Sets the alpha of the background for this slot
    /// </summary>
    /// <param name="alpha">Value between 0 and 1</param>
    public void SetBackgroundAlpha(float alpha)
        {
            Color bgColor = this.background.color;
            bgColor.a = Mathf.Clamp01(alpha);
            this.background.color = bgColor;
        }

        #region IDraggable

        [Header("Drag")]
        [SerializeField] CanvasGroup canvasGroup;

        Transform originalParent;
        GameObject fillingChild;
        Transform canvasParent;
        int childIndex;

        /// <inheritdoc/>
        public void OnDragStart()
        {

        if (dragCancelled)
        {
            return;
        }
        if (WheelActif())
        {
            ZoneUIHandler.Instance.GiveRefInventorySlot(this);
            ZoneUIHandler.Instance.GiveIndex(slotIndex);
        }
       
            // Set up slot for drag
            this.originalParent = this.transform.parent;
            this.canvasGroup.blocksRaycasts = false;

            this.childIndex = this.transform.GetSiblingIndex();

            // Adds temporary ghost slot
            this.fillingChild = Instantiate(this.gameObject, this.transform.parent);
            this.fillingChild.GetComponent<CanvasGroup>().alpha = 0.3f;
            this.fillingChild.transform.SetSiblingIndex(this.childIndex);

            this.transform.SetParent(this.canvasParent);

            this.SetBackgroundAlpha(0f);
        }

    public bool WheelActif()
    {
     GameObject  WheelObject = GameObject.FindGameObjectWithTag("Roue");
        if(WheelObject == null|| !WheelObject.activeInHierarchy)
        {
           return false;
        }
        else
        {
            return true;
        }
    }
        public void TransformIntoGem()
        {
        Debug.Log("transform gemme");
        Inventory.Instance.DropItem(this.slotIndex);
            this.ClearSlot();
            this.ReturnToPosition();
            this.DragEnd();
        }
        public void Cancel()
        {
        Debug.Log("allo");
        this.ClearSlotCancel();
        this.ReturnToPosition();
        this.DragEnd();
      
        //Debug.Log("allo");
        //    this.ReturnToPosition();
        //   OnDragEnd(null, null);
        // this.canvasGroup.blocksRaycasts = true;
        // Supprime la sélection actuelle de l'EventSystem
        // EventSystem.current.SetSelectedGameObject(null);
    }
    
        /// <inheritdoc/>
        public void OnDragEnd(IDragReceivable receivable, RectTransform target)
        {
            // If ended on no receiver, return to original position
            if (receivable == null)
            {
                // If ended on nothing, drop
                if (target == null)
                {
                Debug.Log("ALLO");
                    Inventory.Instance.DropItem(this.slotIndex);
                    this.ClearSlot();
                }

                this.ReturnToPosition();
            }

            this.DragEnd();
        }
    

    public void DragEnd()
        {
            // Destroy filling child
            if (this.fillingChild != null)
                Destroy(this.fillingChild);

            this.canvasGroup.blocksRaycasts = true;
            this.SetBackgroundAlpha(1f);
        if (WheelActif())
        {
            ZoneUIHandler.Instance.NeedReset();
        }
        
        }

        public void ReturnToPosition()
        {
            this.transform.SetParent(this.originalParent);
            this.transform.SetSiblingIndex(this.slotIndex);
        }

        #endregion

        #region IDragReceivable

        /// <inheritdoc/>
        public void OnDragReceive(InventorySlot slot)
        {
            Inventory.Instance.SwapPlace(this.slotIndex, slot.slotIndex);

            Transform parent = this.transform.parent;
            int index = this.transform.GetSiblingIndex();

            this.transform.SetParent(slot.originalParent);
            this.transform.SetSiblingIndex(slot.childIndex);

            Transform otherParent = slot.originalParent;

            // If same container but higher
            if (otherParent == parent && this.slotIndex > slot.slotIndex)
                index++;

            slot.transform.SetParent(parent);
            slot.transform.SetSiblingIndex(index);

            (this.slotIndex, slot.slotIndex) = (slot.slotIndex, this.slotIndex);
        }

        /// <inheritdoc/>
        public void OnDragLeave(InventorySlot slot) { }

        #endregion

        #region IHoverable

        /// <inheritdoc/>
        public ItemData GetData() => Inventory.Instance.GetItem(this.slotIndex);

        #endregion
    }