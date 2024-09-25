using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComponantPowerGemmeObject : MonoBehaviour
{
    [SerializeField] public PowerGemmeObject PowerGemmeObjectScript = new();
    
    public void Generateinitiate()
    {
        PowerGemmeObjectScript.GetGemmeToScriptList();
        PowerGemmeObjectScript.GridGemme.InitializeTab();
      
        SocleGenerator.Instance.GenerateSocle(gameObject, PowerGemmeObjectScript.SocleConteneur);
        PowerGemmeObjectScript.PlaceGemme(PowerGemmeObjectScript.GemmeComponantList);
    }
  

    

}
