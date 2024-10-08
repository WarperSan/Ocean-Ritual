using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class NavBarManager : MonoBehaviour
{
    private Tab currentTab = null;

    public void Start()
    {
        Tab[] tabs = GetComponentsInChildren<Tab>();
        foreach (Tab tab in tabs)
        {
            tab.navBarManager = this;
            tab.Close();
        }
        SelectTab(tabs[0]);
    }
    public void SelectTab(Tab tab)
    {
 
        if (currentTab == tab)
            return;

        if (currentTab != null)
            currentTab.Close();

        tab.Open();
        currentTab = tab;
    }
}
