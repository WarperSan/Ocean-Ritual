using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using UIModule.Components;
using UIModule;
using UIModule.Interfaces;

public class HoverManager : UIComponent
{
    private GameObject hoveredObject;

    private void Start()
    {
        this.hoverItems = this.GetComponentsInChildren<HoverItem>(true);
    }

    private void Update()
    {
        this.RaycastToUI();

        // G�rer les mouvements de l'interface
        this.MoveHover(Input.mousePosition);
    }

    private void RaycastToUI()
    {
        // Si la souris n'est plus sur un �l�ment UI (en dehors de l'inventaire), fermer la ressource
        if (EventSystem.current == null || !EventSystem.current.IsPointerOverGameObject())
        {
            this.HideHover();
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
            // If not hoverable, skip
            if (!result.gameObject.TryGetComponent(out IHoverable hoverable))
                continue;

            // If hovering the same object, skip
            if (this.hoveredObject == result.gameObject)
                return;

            // Met � jour l'item et le flag upDate
            this.hoveredObject = result.gameObject;
            this.ShowHover(hoverable.GetData());
            return; // Sortir de la fonction une fois l'item trouv�
        }

        // Si aucun slot n'a �t� touch� (la souris n'est plus sur un InventorySlot), fermer les ressources
        this.HideHover();
    }

    #region Movement

    private readonly Vector3[] corners = new Vector3[4];
    private void MoveHover(Vector3 screenPosition)
    {
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

    private bool ShowHover(ItemData itemData)
    {
        foreach (HoverItem hover in this.hoverItems)
        {
            // If can't show, skip
            if (!hover.CanShowData(itemData))
                continue;

            hover.SetData(itemData);
            hover.gameObject.SetActive(true);
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