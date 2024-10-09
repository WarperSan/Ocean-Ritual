using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Data;

public class InventoryManager : MonoBehaviour
{
    [SerializeField] private GameObject ActifInventorie;
    [SerializeField] private ItemData Item;
    private bool upDate = false;
    private bool RessourceClose = false;
    private bool UpdateRessource = false;
    void Update()
    {
        // Si l'inventaire est actif, lancer un raycast à partir de la souris
        if (ActifInventorie.activeSelf)
        {
            RaycastToUI();

            if (upDate)
            {
                upDate = false;

                // Si un item est trouvé et c'est un nouvel item ou différent de l'actuel
                if (Item != null)
                {
                    if (Item is FishData fish)
                    {
                        RessourceClose = true;
                        GestionInformation.Instance.GetNewInformationFish(fish);
                    }
                    else if (Item is GemData gem)
                    {
                        RessourceClose = true;
                        GestionInformation.Instance.GetNewInformationGem(gem);
                    }
                }
                else
                {
                    // Si aucun item n'est sélectionné, fermer la ressource
                    GestionInformation.Instance.closeRessource();
                }
            }

            // Si UpdateRessource est vrai, fermer la ressource
            if (UpdateRessource)
            {
                UpdateRessource = false;
                GestionInformation.Instance.closeRessource();
            }

            // Gérer les mouvements de l'interface
            GestionInformation.Instance.InterfaceMovement(Item);
        }
        else
        {
            // Fermer la ressource si l'inventaire est désactivé
            if (RessourceClose)
            {
                RessourceClose = false;
                GestionInformation.Instance.closeRessource();
            }
        }
    }

    void RaycastToUI()
    {
        // Vérifie si la souris est sur un élément UI
        if (EventSystem.current.IsPointerOverGameObject())
        {
            PointerEventData pointerEventData = new PointerEventData(EventSystem.current)
            {
                position = Input.mousePosition
            };

            List<RaycastResult> results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(pointerEventData, results);

            bool isSlotTouched = false; // Indicateur pour savoir si un slot a été touché

            foreach (RaycastResult result in results)
            {
                // Tente de récupérer le composant InventorySlot sur l'objet touché
                InventorySlot slot = result.gameObject.GetComponent<InventorySlot>();
                if (slot != null)
                {
                    isSlotTouched = true;

                    // Si l'item sélectionné est différent de l'item actuel
                    if (Item != slot.GetItem())
                    {
                        
                       

                        // Met à jour l'item et le flag upDate
                        Item = slot.GetItem();
                        upDate = true;
                        break; // Sortir de la boucle une fois l'item trouvé
                    }
                }
            }

            // Si aucun slot n'a été touché (la souris n'est plus sur un InventorySlot), fermer les ressources
            if (!isSlotTouched && Item != null)
            {
                // Si la souris n'est plus sur un élément UI valide, fermer la ressource
                UpdateRessource = true;
                Item = null; // Annuler la sélection de l'item
            }
        }
        else
        {
            // Si la souris n'est plus sur un élément UI (en dehors de l'inventaire), fermer la ressource
            if (Item != null)
            {
                UpdateRessource = true;
                Item = null; // Annuler la sélection de l'item
            }
        }
    }

}
