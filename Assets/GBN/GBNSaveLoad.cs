using Save;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GBNSaveLoad :SaveBehaviour
{
    [SerializeField] private ListeGBNData dataGBN =new ();
    [SerializeField] private List<ComponantGBN> ListGBN = new();

    public void SaveData()
    {
        
    }

    public void LoadData()
    {
       
    }

    protected override void OnLoad(SaveData data)
    {

     
    }

    protected override void OnSave(ref SaveData data)
    {
     
    }
}
