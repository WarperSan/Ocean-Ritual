using Fishing;
using UnityEngine;

public class GestionnairePeche : MonoBehaviour
{
    [SerializeField] GameObject DetecteurTerritoir;
    [SerializeField] GameObject DrapeauPeche;
    [SerializeField] GameObject GestionDelimitation;
    [SerializeField] uint PoidFillet = 100;
    

    private void OnEnable()
    {
        GestionDelimitation.GetComponent<GestionDelimitation>().DebutLimitation();
      
        DetecteurTerritoir.GetComponent<FiiletPeche>().DetectionTerritoir();
        DrapeauPeche.GetComponent<FishingBuoy>().StartNew(PoidFillet);
    }
}
