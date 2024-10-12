using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class InventoryManager : MonoBehaviour
{
    private GameObject hoveredObject;

    private void Update()
    {
        this.RaycastToUI();

        // G�rer les mouvements de l'interface
        GestionInformation.Instance.InterfaceMovement();
    }

    private void RaycastToUI()
    {
        // Si la souris n'est plus sur un �l�ment UI (en dehors de l'inventaire), fermer la ressource
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            this.UnSelect();
            return;
        }

        var pointerEventData = new PointerEventData(EventSystem.current)
        {
            position = Input.mousePosition
        };

        var results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerEventData, results);

        foreach (RaycastResult result in results)
        {
            // Tente de r�cup�rer le composant InventorySlot sur l'objet touch�
            if (!result.gameObject.TryGetComponent(out InventorySlot slot))
                continue;

            // If hovering the same object, skip
            if (this.hoveredObject == result.gameObject)
                return;

            // Met � jour l'item et le flag upDate
            this.hoveredObject = result.gameObject;
            GestionInformation.Instance.OpenHover(slot.GetItem());
            return; // Sortir de la fonction une fois l'item trouv�
        }

        // Si aucun slot n'a �t� touch� (la souris n'est plus sur un InventorySlot), fermer les ressources
        this.UnSelect();
    }

    private void UnSelect()
    {
        if (this.hoveredObject == null)
            return;

        this.hoveredObject = null; // Annuler la s�lection de l'item
        GestionInformation.Instance.CloseHover();
    }
}
