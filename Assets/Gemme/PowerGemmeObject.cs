using static EnumGeneral;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PowerGemmeObject : MonoBehaviour
{
    GemmeGrid GridGemme;
    [SerializeField] private GameObject SocleConteneur;
    [SerializeField] private GameObject GemmeConteneur;
    [SerializeField] private TypeDeSocle typeDeSocle;
    [SerializeField] private List<TypeQuantite<TypeArme>> typeArme = new List<TypeQuantite<TypeArme>>();
    [SerializeField] private List<TypeQuantite<TypeBateau>> typeBoat = new List<TypeQuantite<TypeBateau>>();
    [SerializeField] private List<TypeQuantite<TypeFilet>> typeFilet = new List<TypeQuantite<TypeFilet>>();
    [SerializeField] List<GemmeComponant> GemmeList = new ();
    // Start is called before the first frame update
    void Start()
    {

        GridGemme = GetComponent<GemmeGrid>();
        GridGemme.InitializeTableau();
        ResetLists();
       // SocleGenerator.Instance.GenerateSocle(gameObject, SocleConteneur);
        PlacerGemme(GemmeList);
       
     
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void PlacerGemme(List<GemmeComponant> ListGemme)
    {
        foreach (GemmeComponant Gemmes  in ListGemme)
        {
          
            GridGemme.PlacerObjet(Gemmes.GemmeScript.PositionX, Gemmes.GemmeScript.PositionZ, Gemmes.GemmeScript.forme.GetForme());
          GameObject theGemme =   GeneratorGemme.Instance.CreatGemmeObject(Gemmes.GemmeScript, GemmeConteneur.transform);
            //theGemme.transform.position = new Vector3(Gemmes.GemmeScript.PositionX, 0, Gemmes.GemmeScript.PositionZ);
        }
    }

    private void InitializeLists()
    {
        // Initialize typeArme list
        foreach (TypeArme arme in System.Enum.GetValues(typeof(TypeArme)))
        {
            typeArme.Add(new TypeQuantite<TypeArme>(arme, 1));
        }

        // Initialize typeBoat list
        foreach (TypeBateau boat in System.Enum.GetValues(typeof(TypeBateau)))
        {
            typeBoat.Add(new TypeQuantite<TypeBateau>(boat, 1));
        }

        // Initialize typeFilet list
        foreach (TypeFilet filet in System.Enum.GetValues(typeof(TypeFilet)))
        {
            typeFilet.Add(new TypeQuantite<TypeFilet>(filet, 1));
        }
    }
    public void ReceiveGemme(Gemme oneGemme)
    {

    }
    public void DeletedGemme()
    {

    }

    public void ResetLists()
    {
        typeArme.Clear();
        typeBoat.Clear();
        typeFilet.Clear();
        InitializeLists();
    }
}
