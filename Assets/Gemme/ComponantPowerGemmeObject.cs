using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComponantPowerGemmeObject : MonoBehaviour
{
    [SerializeField] private PowerGemmeObject PowerGemmeObjectScript = new();
    void Start()
    {
        
        PowerGemmeObjectScript.GridGemme.InitializeTableau();

        SocleGenerator.Instance.GenerateSocle(gameObject, PowerGemmeObjectScript.SocleConteneur);
        PowerGemmeObjectScript.PlacerGemme(PowerGemmeObjectScript.GemmeList);
    }

}
