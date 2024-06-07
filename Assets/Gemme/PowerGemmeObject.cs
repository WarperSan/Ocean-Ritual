using static EnumGeneral;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PowerGemmeObject : MonoBehaviour
{
    #region Fields and Properties

    private GemmeGrid GridGemme;
    [SerializeField] private GameObject SocleConteneur;
    [SerializeField] private GameObject GemmeConteneur;
    [SerializeField] private TypeDeSocle typeDeSocle;
    [SerializeField] private List<TypeQuantite<TypeArme>> typeArme = new List<TypeQuantite<TypeArme>>();
    [SerializeField] private List<TypeQuantite<TypeBateau>> typeBoat = new List<TypeQuantite<TypeBateau>>();
    [SerializeField] private List<TypeQuantite<TypeFilet>> typeFilet = new List<TypeQuantite<TypeFilet>>();
    [SerializeField] private List<GemmeComponant> GemmeList = new();

    #endregion

    #region Unity Methods

    // Start is called before the first frame update
    void Start()
    {
        GridGemme = GetComponent<GemmeGrid>();
        GridGemme.InitializeTableau();
        ResetLists();
        SocleGenerator.Instance.GenerateSocle(gameObject, SocleConteneur);
        PlacerGemme(GemmeList);
    }

    // Update is called once per frame
    void Update()
    {

    }

    #endregion

    #region Gemme Placement

    // Places gems in the grid and sets their position
    public void PlacerGemme(List<GemmeComponant> ListGemme)
    {
        foreach (GemmeComponant Gemmes in ListGemme)
        {
            GridGemme.PlaceObject(Gemmes.GemmeScript.PositionX, Gemmes.GemmeScript.PositionZ, Gemmes.GemmeScript.forme.GetForme());
            GameObject theGemme = GeneratorGemme.Instance.CreatGemmeObject(Gemmes.GemmeScript, GemmeConteneur.transform);
            theGemme.transform.position += new Vector3(Gemmes.GemmeScript.PositionX * SocleGenerator.Instance.spaceBetweenCube, 0, Gemmes.GemmeScript.PositionZ * SocleGenerator.Instance.spaceBetweenCube);
        }
    }

    #endregion

    #region List Initialization and Reset

    // Initializes the lists with default values
    private void InitializeLists()
    {
        foreach (TypeArme arme in System.Enum.GetValues(typeof(TypeArme)))
        {
            typeArme.Add(new TypeQuantite<TypeArme>(arme, 1));
        }

        foreach (TypeBateau boat in System.Enum.GetValues(typeof(TypeBateau)))
        {
            typeBoat.Add(new TypeQuantite<TypeBateau>(boat, 1));
        }

        foreach (TypeFilet filet in System.Enum.GetValues(typeof(TypeFilet)))
        {
            typeFilet.Add(new TypeQuantite<TypeFilet>(filet, 1));
        }
    }

    // Clears and reinitializes the lists
    public void ResetLists()
    {
        typeArme.Clear();
        typeBoat.Clear();
        typeFilet.Clear();
        InitializeLists();
    }

    #endregion

    #region Gemme Management

    // Receives a gem (implementation needed)
    public void ReceiveGemme(Gemme oneGemme)
    {

    }

    // Deletes a gem (implementation needed)
    public void DeletedGemme()
    {

    }

    #endregion
}