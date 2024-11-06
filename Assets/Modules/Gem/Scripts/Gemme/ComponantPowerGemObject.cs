using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class componentPowerGemObject : MonoBehaviour
{
    [SerializeField] public PowerGemObject PowerGemObjectScript = new();
    bool firstStart = true;
    public void Generateinitiate()
    {
        if(firstStart)
        {
            PowerGemObjectScript.GetGemToScriptList();
            firstStart = false;
        }
     
        PowerGemObjectScript.GridGemme.InitializeTab();
        PowerGemObjectScript.GetParentTransform(this.transform);
        SocleGenerator.Instance.GenerateSocle(gameObject, PowerGemObjectScript.SocleContainer);
        PowerGemObjectScript.PlaceGem();
    }
  

   
}
