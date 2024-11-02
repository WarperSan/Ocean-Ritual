using UnityEngine;
using UnityEngine.EventSystems;
using UtilsModule;

public class ZoneUIHandler : Singleton<ZoneUIHandler>, IPointerEnterHandler, IPointerExitHandler
{
    private bool isHovering = false;
    public int index =-1;
    public GemData GemActif = null;
 
    [SerializeField] MouseWheelManager mousManager;

  
    // Appelé quand la souris entre dans la zone de la cible
    public void OnPointerEnter(PointerEventData eventData)
    {
        isHovering = true;
        ChangeState(true); // Passe à l'état activé
    }

    // Appelé quand la souris quitte la zone de la cible
    public void OnPointerExit(PointerEventData eventData)
    {
        isHovering = false;
        ChangeState(false); // Retourne à l'état initial
    }

    private void ChangeState(bool hovering)
    {
        
   
       
        if (hovering)
        {
            if(index!= -1 )
            {
                
                if (GemActif.LVL== -1)
                {
                    TrySpawnGemm(index);
                }
          
            }
            // Code pour l'état activé (survolé)
            Debug.Log("État activé");
        }
        else
        {
            // Code pour revenir à l'état initial
            Debug.Log("État désactivé");
        }
    }
    protected override bool DestroyOnLoad => true;
    public void TrySpawnGemm(int index)
    {
        
        ItemData itemData = Inventory.Instance.GetItem(index);
        if (itemData is GemData gemData)
        {
            Debug.Log("allo");
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