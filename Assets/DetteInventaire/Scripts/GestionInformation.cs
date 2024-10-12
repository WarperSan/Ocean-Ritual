using UnityEngine;
using UIModule.Components;
using UtilsModule;

[RequireComponent(typeof(RectTransform))]
public class GestionInformation : Singleton<GestionInformation>
{
    private HoverItem[] hoverItems;
    private RectTransform rectTransform;

    protected override void OnAwake()
    {
        this.hoverItems = this.GetComponentsInChildren<HoverItem>(true);
        this.rectTransform = this.GetComponent<RectTransform>();
    }

    private readonly Vector3[] corners = new Vector3[4];
    public void InterfaceMovement()
    {
        // R�cup�re la position actuelle de la souris dans l'�cran (en pixels)
        Vector3 mousePosition = Input.mousePosition;

        rectTransform.GetWorldCorners(corners);
        float width = corners[2].x - corners[0].x;
        float height = corners[1].y - corners[0].y;

        // If width offscreen, invert
        if (mousePosition.x + width > Screen.width)
            mousePosition.x -= width;

        // If height offscreen, invert
        if (mousePosition.y - height < 0)
            mousePosition.y += height;

        // Met � jour la position de l'objet UI pour suivre la position de la souris
        this.rectTransform.position = mousePosition;
    }

    public bool OpenHover(ItemData itemData)
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

    public void CloseHover()
    {
        foreach (HoverItem hover in this.hoverItems)
            hover.gameObject.SetActive(false);
    }

    #region Singleton

    /// <inheritdoc/>
    protected override bool DestroyOnLoad => true;

    #endregion
}
