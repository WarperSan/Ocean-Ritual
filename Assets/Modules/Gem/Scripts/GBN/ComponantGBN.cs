using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComponantGBN : MonoBehaviour
{
    [SerializeField] public GBN GBNScript = new();
    // Start is called before the first frame update



    public void GenereSocle()
    {
        foreach (ComponantPowerGemmeObject socle in GBNScript.SocleListe)
        {
            socle.Generateinitiate();
        }
    }

    // Update is called once per frame
    void Update()
    {
       
    }
    
}
