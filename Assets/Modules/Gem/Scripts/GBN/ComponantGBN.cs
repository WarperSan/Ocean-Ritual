using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComponantGBN : MonoBehaviour
{
    [SerializeField] public GBN GBNScript = new();
    // Start is called before the first frame update



    public void GenerationSocle()
    {
        foreach (ComponantPowerGemObject socle in GBNScript.SocleListe)
        {
            socle.Generateinitiate();
        }
    }

  
    
}
