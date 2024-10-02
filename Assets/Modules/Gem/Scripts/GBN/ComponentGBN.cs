using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class componentGBN : MonoBehaviour
{
    [SerializeField] public GBN GBNScript = new();
    // Start is called before the first frame update



    public void GenerationSocle()
    {
        foreach (componentPowerGemObject socle in GBNScript.SocleListe)
        {
            socle.Generateinitiate();
        }
    }

  
    
}
