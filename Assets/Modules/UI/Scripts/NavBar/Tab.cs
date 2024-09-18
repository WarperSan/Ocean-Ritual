using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tab : MonoBehaviour
{
    [HideInInspector] public NavBarManager navBarManager;
    [SerializeField] GameObject TabUI;
    [SerializeField] GameObject selectedBar;

    public void Select()
    {
        navBarManager.SelectTab(this);
    }
    public void Open()
    {
        if(TabUI != null)
            TabUI.SetActive(true);

        if(selectedBar != null)
            selectedBar.SetActive(true);
    }
    public void Close()
    {
        if (TabUI != null)
            TabUI.SetActive(false);

        if (selectedBar != null)
            selectedBar.SetActive(false);
    }
}
