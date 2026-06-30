using UnityEngine;

public class componentPowerGemObject : MonoBehaviour
{
    [SerializeField]
    public PowerGemObject PowerGemObjectScript = new();

    private bool firstStart = true;

    public void Generateinitiate()
    {
        if (firstStart)
        {
            PowerGemObjectScript.GetGemToScriptList();
            firstStart = false;
        }

        PowerGemObjectScript.GridGemme.InitializeTab();
        PowerGemObjectScript.GetParentTransform(transform);
        SocleGenerator.Instance.GenerateSocle(gameObject, PowerGemObjectScript.SocleContainer);
        PowerGemObjectScript.PlaceGem();
    }
}