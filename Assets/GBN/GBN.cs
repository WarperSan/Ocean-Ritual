using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static EnumGeneral;

public class GBN : MonoBehaviour
{
    [SerializeField] private TypeDeSocle typeDeSocle;
    [SerializeField] private List<TypeQuantite<TypeArme>> typeArme = new();
    [SerializeField] private List<TypeQuantite<TypeBateau>> typeBoat = new();
    [SerializeField] private List<TypeQuantite<TypeFilet>> typeFilet = new();
    [SerializeField] private List<PowerGemmeObject> SocleListe = new();
    // Start is called before the first frame update
    void Start()
    {
        ResetLists();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
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
}
