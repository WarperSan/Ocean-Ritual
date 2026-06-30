//using Microsoft.Unity.VisualStudio.Editor;

using UnityEngine;
using UnityEngine.EventSystems;
using UtilsModule;
using UnityEngine.UI;

public class ZoneUIHandler : Singleton<ZoneUIHandler>, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField]
    private Image imageZone;

    public int index = -1;
    public GemData GemActif;
    public GameObject GemObject;
    private Gemcomponent GemToInventory;
    private InventorySlot ActifInventorySlot;

    [SerializeField]
    private MouseWheelManager mousManager;

    [SerializeField]
    private InventoryUI InventoryUI;

    [SerializeField]
    private Sprite sprite;

    [SerializeField]
    private WheelSocket wheelSocket;

    // Appel� quand la souris entre dans la zone de la cible
    public void OnPointerEnter(PointerEventData eventData) => ChangeState(true); // Passe � l'�tat activ�

    public void GiveRefInventorySlot(InventorySlot inventorySlot)
    {
        ActifInventorySlot = inventorySlot;
        imageZone.raycastTarget = true;
    }

    public void DeletRefInventorySlot() => ActifInventorySlot.TransformIntoGem();

    // Appel� quand la souris quitte la zone de la cible
    public void OnPointerExit(PointerEventData eventData) => ChangeState(false); // Retourne � l'�tat initial

    public void ReceiveGemSocleTOInventory(GameObject gem)
    {
        GemObject = gem;
        GemToInventory = GemObject.GetComponent<Gemcomponent>();
    }

    public void RemoveGemSocleTOInventory()
    {
        DesactivateRaycast();
        GemObject = null;
        GemToInventory = null;
    }

    private void ChangeState(bool hovering)
    {
        if (hovering)
        {
            if (index != -1)
            {
                if (GemActif.LVL == -1)
                {
                    if (!wheelSocket.IsRotating())
                    {
                        TrySpawnGemm(index);
                        DeletRefInventorySlot();
                    }
                    else
                        ResetGemme(true);
                }
            }
            // Code pour l'�tat activ� (survol�)
        }
        else
            GoToInventory();
    }

    public void activeRaycast() => imageZone.raycastTarget = true;

    public void DesactivateRaycast() => imageZone.raycastTarget = false;

    private void GoToInventory(bool cancel = false)
    {
        if (GemToInventory != null && GemToInventory.GemScript.LVL != -1)
        {
            GemData gemdata = GemHelper.ConvertGemToGemData(GemToInventory.GemScript);

            Inventory.Instance.AddItem(gemdata);
            mousManager.notSelectObject(GemToInventory.GemScript, false);
            Destroy(GemToInventory.gameObject);
            InventoryUI.UpdateSelf();
            GemToInventory.GemScript.LVL = -1;
            GemToInventory = null;
            DesactivateRaycast();
        }
        // Code pour revenir � l'�tat initial
        // Debug.Log("�tat d�sactiv�");
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

    public void ResetGemme(bool reset = false)
    {
        //  Debug.Log("reset");
        GemActif = new GemData();
        GemActif.LVL = -1;
        index = -1;

        if (reset)
            ActifInventorySlot.Cancel();

        ActifInventorySlot = null;
    }

    public void NeedReset()
    {
        if (GemActif.LVL != -1 || index != -1)
            ResetGemme();
    }

    public void GiveIndex(int index) => this.index = index;
}