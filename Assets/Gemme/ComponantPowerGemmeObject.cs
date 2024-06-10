using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComponantPowerGemmeObject : MonoBehaviour
{
    [SerializeField] public PowerGemmeObject PowerGemmeObjectScript = new();
    void Start()
    {
        Generateinitiate();
    }
    public void Generateinitiate()
    {

        PowerGemmeObjectScript.GridGemme.InitializeTableau();

        SocleGenerator.Instance.GenerateSocle(gameObject, PowerGemmeObjectScript.SocleConteneur);
        PowerGemmeObjectScript.PlacerGemme(PowerGemmeObjectScript.GemmeComponantList);
    }
  

}
