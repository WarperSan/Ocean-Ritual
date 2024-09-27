using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComponantPowerGemObject : MonoBehaviour
{
    [SerializeField] public PowerGemObject PowerGemObjectScript = new();
    
    public void Generateinitiate()
    {
        PowerGemObjectScript.GetGemToScriptList();
        PowerGemObjectScript.GridGemme.InitializeTab();
      
        SocleGenerator.Instance.GenerateSocle(gameObject, PowerGemObjectScript.SocleContainer);
        PowerGemObjectScript.PlaceGem(PowerGemObjectScript.GemComponantList);
    }
  

    

}
