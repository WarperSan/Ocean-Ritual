using ControllerModule.Interfaces.UI;
using System.Collections;
using System.Collections.Generic;
using UIModule;
using UnityEngine;

public class ParentSelection : UIComponent, ITabable
{
    [SerializeField] GameObject Selection;
    public void OnTabNext() => activate();
    public void OnTabPrevious() => activate();

    public void activate()
    {
        if (Selection != null && !Selection.activeSelf)
        {
            Selection.SetActive(true); // Active l'objet seulement s'il est actuellement désactivé
        }
        else
        {
            deactivate();
        }
    }

    public void deactivate()
    {
        if (Selection != null && Selection.activeSelf)
        {
            Selection.SetActive(false); // Désactive l'objet seulement s'il est actuellement activé
        }
    }
}
