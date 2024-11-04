using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.EventSystems;
using UtilsModule;

public class ZoneUIHandler : Singleton<ZoneUIHandler>, IPointerEnterHandler, IPointerExitHandler
{
    private bool isHovering = false;
    public int index =-1;
    public GemData GemActif = null;
    public GameObject GemObject = null;
    private Gemcomponent GemToInventory;
    InventorySlot ActifInventorySlot;
    [SerializeField] MouseWheelManager mousManager;
    [SerializeField] InventoryUI InventoryUI;
    [SerializeField] Sprite sprite;
    [SerializeField] WheelSocket wheelSocket;
    // Appelé quand la souris entre dans la zone de la cible
    public void OnPointerEnter(PointerEventData eventData)
    {
        isHovering = true;
        ChangeState(true); // Passe à l'état activé
    }
    public void GiveRefInventorySlot(InventorySlot inventorySlot)
    {
        ActifInventorySlot = inventorySlot;
    }
    public void DeletRefInventorySlot( )
    {
        ActifInventorySlot.TransformIntoGem();
    }
    // Appelé quand la souris quitte la zone de la cible
    public void OnPointerExit(PointerEventData eventData)
    {
        isHovering = false;
        ChangeState(false); // Retourne à l'état initial
    }

    public void ReceiveGemSocleTOInventory(GameObject gem)
    {
        GemObject = gem;
        GemToInventory = GemObject.GetComponent<Gemcomponent>() ;
    }
    public void RemoveGemSocleTOInventory()
    {
        GemObject = null;
        GemToInventory = null;
    }
    private void ChangeState(bool hovering)
    {
        
   
       
        if (hovering)
        {
            if(index!= -1 )
            {
                
                if (GemActif.LVL== -1)
                {
                    if (!wheelSocket.IsRotating())
                    {
                        TrySpawnGemm(index);
                        DeletRefInventorySlot();
                    }
                    else
                    {
                        //TrySpawnGemm(index);
                       // DeletRefInventorySlot();
                      //  GoToInventory();
                    }
                  
                }
          
            }
            // Code pour l'état activé (survolé)
            Debug.Log("État activé");
        }
        else
        {
            GoToInventory();
        }
    }
    private void GoToInventory(bool cancel = false)
    {
        
        if (GemToInventory != null && GemToInventory.GemScript.LVL != -1)
        {
            Debug.Log("allo");
            GemData gemdata = GemHelper.ConvertGemToGemData(GemToInventory.GemScript);

            Inventory.Instance.AddItem(gemdata);
            mousManager.notSelectObject(GemToInventory.GemScript, false);
            Destroy(GemToInventory.gameObject);
            InventoryUI.UpdateSelf();
            GemToInventory.GemScript.LVL = -1;
            GemToInventory = null;

        }
        // Code pour revenir à l'état initial
        Debug.Log("État désactivé");
    }
    protected override bool DestroyOnLoad => true;
    public void TrySpawnGemm(int index)
    {
        
        ItemData itemData = Inventory.Instance.GetItem(index);
        if (itemData is GemData gemData)
        {
            
            GemActif = gemData;
            mousManager.TestSpawnGem(gemData); 
        }
    }
    public void ResetGemme()
    {
        GemActif = new();
        GemActif.LVL =-1;
        index = -1;
    }
    public void GiveIndex(int index)
    {
        this.index = index;

    }
}