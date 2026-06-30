using UnityEngine;

public class componentGBN : MonoBehaviour
{
    [SerializeField]
    public GBN GBNScript = new();
    // Start is called before the first frame update

    public void GenerationSocle()
    {
        foreach (componentPowerGemObject socle in GBNScript.SocleListe)
            socle.Generateinitiate();
    }

    public void AddNewSocle(GameObject soclePrefab)
    {
        // Instancier le prefab en tant qu'enfant de l'objet actuel
        GameObject newSocle = Instantiate(soclePrefab, transform);

        // Obtenir le script componentPowerGemObject attach� � l'instance du socle
        componentPowerGemObject socleComponent = newSocle.GetComponent<componentPowerGemObject>();

        if (socleComponent != null)
        {
            // Ajouter le socle � la liste SocleListe
            GBNScript.SocleListe.Add(socleComponent);

            // Mise � jour de la liste PowerGemObjectListe
            GBNScript.PowerGemObjectListe.Add(socleComponent.PowerGemObjectScript);
        }
        else
            Debug.LogError("Le prefab ne contient pas de componentPowerGemObject.");
    }
}