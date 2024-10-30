using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using UIModule.Components;
using UIModule;
using UIModule.Interfaces;

public class HoverManager : UIComponent
{
    private void Start()
    {
        this.hoverItems = this.GetComponentsInChildren<HoverItem>(true);
    }

    private void Update()
    {
        IHoverable target = this.RaycastToUI();

        // Si aucun slot n'a �t� touch� (la souris n'est plus sur un InventorySlot), fermer les ressources
        if (target == null)
        {
            this.HideHover();
            return;
        }

        // Met � jour l'item
        if (target != this.hoveredObject)
        {
            this.hoveredObject = target;
            this.ShowHover(target.GetData());
        }
        // G�rer les mouvements de l'interface
        else
        {
            this.MoveHover();
        }
    }

    private IHoverable RaycastToUI()
    {
        // Si la souris n'est plus sur un �l�ment UI (en dehors de l'inventaire), fermer la ressource
        if (EventSystem.current == null || !EventSystem.current.IsPointerOverGameObject())
            return null;

        var pointerEventData = new PointerEventData(EventSystem.current)
        {
            position = Input.mousePosition
        };

        var results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerEventData, results);

        foreach (RaycastResult result in results)
        {
            // If not hoverable, skip
            if (!result.gameObject.TryGetComponent(out IHoverable hoverable))
                continue;

            return hoverable; // Sortir de la fonction une fois l'item trouv�
        }

        return null;
    }

    #region Movement

    private readonly Vector3[] corners = new Vector3[4];
    private void MoveHover()
    {
        Vector3 screenPosition = Input.mousePosition;

        this.Rect.GetWorldCorners(corners);
        float width = corners[2].x - corners[0].x;
        float height = corners[1].y - corners[0].y;

        // If width offscreen, invert
        if (screenPosition.x + width > Screen.width)
            screenPosition.x -= width;

        // If height offscreen, invert
        if (screenPosition.y - height < 0)
            screenPosition.y += height;

        // Met � jour la position de l'objet UI pour suivre la position de la souris
        this.Rect.position = screenPosition;
    }

    #endregion

    #region Toggle Hover

    private HoverItem[] hoverItems;
    private IHoverable hoveredObject;

    private bool ShowHover(ItemData itemData)
    {
        foreach (HoverItem hover in this.hoverItems)
        {
            // If can't show, skip
            if (!hover.CanShowData(itemData))
                continue;

            hover.SetData(itemData);
            this.Rect.sizeDelta = hover.Rect.sizeDelta; // Copy size
            this.MoveHover(); // Update position
            hover.gameObject.SetActive(true); // Set active
            return true;
        }

        return false;
    }

    private void HideHover()
    {
        if (this.hoveredObject == null)
            return;

        this.hoveredObject = null; // Annuler la s�lection de l'item

        foreach (HoverItem hover in this.hoverItems)
            hover.gameObject.SetActive(false);
    }

    #endregion
}