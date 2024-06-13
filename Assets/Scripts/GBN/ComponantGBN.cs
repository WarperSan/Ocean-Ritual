using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComponantGBN : MonoBehaviour
{
    [SerializeField] public GBN GBNScript = new();
    // Start is called before the first frame update
    void Start()
    {
        GBNScript.ResetLists();
        GBNScript.StatCalculator();
        GBNScript.GetSocleToScriptList();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
