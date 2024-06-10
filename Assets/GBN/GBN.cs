using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static EnumGeneral;
[System.Serializable]
public class GBN 
{
    [SerializeField] private string Name;
    [SerializeField] private TypeDeSocle typeDeSocle;
    [SerializeField] private List<TypeQuantite<TypeArme>> typeArme = new();
    [SerializeField] private List<TypeQuantite<TypeBateau>> typeBoat = new();
    [SerializeField] private List<TypeQuantite<TypeFilet>> typeFilet = new();
    [SerializeField] private List<ComponantPowerGemmeObject> SocleListe = new();
    private List<PowerGemmeObject> PowerGemmeObjectListe = new();
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

    public void GetSocleToScriptList()
    {
        PowerGemmeObjectListe.Clear();
        foreach (ComponantPowerGemmeObject item in SocleListe)
        {
            PowerGemmeObjectListe.Add(item.PowerGemmeObjectScript);
        }
    }

    public void StatCalculator()
    {
        foreach (ComponantPowerGemmeObject socle in SocleListe)
        {
            foreach (GemmeComponant gemme in socle.PowerGemmeObjectScript.GemmeComponantList)
            {
                Gemme theGemmeScript = gemme.GemmeScript;

                switch (typeDeSocle)
                {
                    case TypeDeSocle.Arme:
                        UpdateStatsFromGemmeList<TypeArme>(theGemmeScript, typeArme);
                        break;
                    case TypeDeSocle.Bateau:
                        UpdateStatsFromGemmeList<TypeBateau>(theGemmeScript, typeBoat);
                        break;
                    case TypeDeSocle.Filet:
                        UpdateStatsFromGemmeList<TypeFilet>(theGemmeScript, typeFilet);
                        break;
                }
            }
        }
    }
    private void UpdateStatsFromGemmeList<TEnum>(Gemme theGemmeScript, List<TypeQuantite<TEnum>> list)
    {
        List<TypeQuantite<TEnum>> gemmeList = theGemmeScript.GetListType<TEnum>();

        foreach (TypeQuantite<TEnum> gemmeStat in gemmeList)
        {
            foreach (TypeQuantite<TEnum> stat in list)
            {
                if (stat.Type.Equals(gemmeStat.Type))
                {
                    stat.Quantite += gemmeStat.Quantite;
                }
            }
        }
    }
}
