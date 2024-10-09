using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections.Generic;

public class InventoryManager : MonoBehaviour
{
    [SerializeField] private GameObject ActifInventorie;
    [SerializeField] private ItemData Item;
    private bool upDate = false;

    void Update()
    {
        // Si l'inventaire est actif, lancer un raycast à partir de la souris
        if (ActifInventorie.activeSelf)
        {
            RaycastToUI();
            if (upDate)
            {
                upDate = !upDate;

            }
        }
    }

    void RaycastToUI()
    {
        // On ne lance un raycast que si la souris est sur un élément UI
        if (EventSystem.current.IsPointerOverGameObject())
        {
            PointerEventData pointerEventData = new PointerEventData(EventSystem.current)
            {
                position = Input.mousePosition
            };

            List<RaycastResult> results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(pointerEventData, results);

            foreach (RaycastResult result in results)
            {
                // Tente de récupérer le composant InventorySlot sur l'objet touché
                InventorySlot slot = result.gameObject.GetComponent<InventorySlot>();
                if (slot != null)
                {
                    if(Item != slot.GetItem())
                    {
                        Item = slot.GetItem();
                        upDate = true;
                    }
                       
                                       
                }
            }
        }
    }
}
