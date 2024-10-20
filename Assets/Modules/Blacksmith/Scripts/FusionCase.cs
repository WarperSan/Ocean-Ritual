using System.Collections;
using System.Collections.Generic;
using UIModule.Interfaces;
using UIModule;
using UnityEngine;
using BlacksmithModule;
using GemModule.UI;
using UIModule.Menus;
using UnityEngine.UI;


public class FusionCase : UIComponent, IDragReceivable<InventorySlot>, IDraggable
{
    private int slotIndex = -1;
    public GemData gems = null;
    [SerializeField] int CasePostion = 0;// 1 left ,2= right ,3 = mid
    #region Fields

    [Header("Fields")]

   

    [SerializeField]
    private Image Icon;

    #endregion
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void ReceiveGem(GemData gem)
    {
        this.gems = gem;
        this.Icon.sprite = gem.sprite;
        
        this.enabled = true;  
    }
    public void ReceiveGemFromFusion(GemData gem)
    {
        this.gems = gem;
        Debug.Log(gem.sprite);
        Debug.Log(this.Icon.sprite);
        this.Icon.sprite = gem.sprite;

        this.enabled = true;

   
    }
    public void Resete()
    {
        slotIndex = -1;
          gems = null;
        this.Icon.sprite = null;

    }

    public void OnDragReceive(InventorySlot slot)
    {
    
        if (slot.GetData() is not GemData gem)
        {
            Debug.Log("je recois pas gemdata1");
            return;
        }
          
        if(CasePostion!=1 && CasePostion!= 2)
        {
            Debug.Log("je recois pas gemdata2");
            return;
        }
        Debug.Log("je recois");
        Inventory.Instance.DropItem(slot.slotIndex);

        if (this.slotIndex != -1)
            Inventory.Instance.AddItem(this.gems, slot.slotIndex);

        this.ReceiveGem(gem);
        this.slotIndex = slot.slotIndex;
       
        TestBlackSmith.Instance.UpdateUI();
        slot.DragEnd();
        Destroy(slot.gameObject);
    }
    #region IDraggable

    private Transform originalParent;
    private GameObject fillingChild;
    private Transform canvasParent;

    /// <inheritdoc/>
    public void OnDragStart()
    {
 
    }

    /// <inheritdoc/>
    public void OnDragEnd(IDragReceivable receivable, RectTransform target)
    {
     
    }

    #endregion
  
    public void OnDragLeave(InventorySlot slot) { }
    bool IDragReceivable<InventorySlot>.CanReceiveDraggable(InventorySlot slot) => slot.GetData() is GemData;
}
