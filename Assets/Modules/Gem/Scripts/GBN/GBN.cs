using System;
using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
  

using static EnumGeneral;
    [System.Serializable]
    public class GBN 
    {
        [SerializeField] public string Name;
        [SerializeField] public TypeOfSocle typeSocle;
        [SerializeField] public List<TypeQuantite<TypeWeapon>> typeArme = new();
        [SerializeField] public List<TypeQuantite<TypeBoat>> typeBoat = new();
        [SerializeField] public List<TypeQuantite<TypeNet>> typeFilet = new();


    [SerializeField]  public List<ComponantPowerGemmeObject> SocleListe = new();
    // This list will be serialized but not visible in the inspector
    [HideInInspector]
    [SerializeField] public List<PowerGemmeObject> PowerGemmeObjectListe = new();

    public void GetStat()
    {
        ResetLists();
       StatCalculator();
       GetSocleToScriptList();
    }
    private List<TypeQuantite<TEnum>> GetListForEnum<TEnum>()
    {
        if (typeof(TEnum) == typeof(TypeWeapon))
        {
            return typeArme as List<TypeQuantite<TEnum>>;
        }
        else if (typeof(TEnum) == typeof(TypeBoat))
        {
            return typeBoat as List<TypeQuantite<TEnum>>;
        }
        else if (typeof(TEnum) == typeof(TypeNet))
        {
            return typeFilet as List<TypeQuantite<TEnum>>;
        }
        else
        {
            Debug.LogError("Unsupported type.");
            return null;
        }
    }

    public void UpdateStatsWithBoost<TEnum>(List<TypeQuantite<TEnum>> baseStats, List<TypeQuantite<TEnum>> boostedStats)
    {
        List<TypeQuantite<TEnum>> correspondingList = GetListForEnum<TEnum>();

        if (correspondingList == null)
        {
            Debug.LogError("No corresponding list found.");
            return;
        }

        foreach (var boostedStat in boostedStats)
        {
            var baseStat = baseStats.Find(x => x.Type.Equals(boostedStat.Type));
            if (baseStat != null)
            {
                boostedStat.Quantite = baseStat.Quantite + correspondingList.Find(x => x.Type.Equals(boostedStat.Type))?.Quantite ?? baseStat.Quantite;
            }
        }
    }

    #region List Initialization and Reset

    // Initializes the lists with default values
    /// <summary>
    /// Initiate the list
    /// </summary>
    /// 

    private void InitializeLists()
        {
            foreach (TypeWeapon arme in System.Enum.GetValues(typeof(TypeWeapon)))
            {
                typeArme.Add(new TypeQuantite<TypeWeapon>(arme, 0));
            }

            foreach (TypeBoat boat in System.Enum.GetValues(typeof(TypeBoat)))
            {
                typeBoat.Add(new TypeQuantite<TypeBoat>(boat, 0));
            }

            foreach (TypeNet filet in System.Enum.GetValues(typeof(TypeNet)))
            {
                typeFilet.Add(new TypeQuantite<TypeNet>(filet, 0));
            }
        }
    /// <summary>
    /// Reset the list
    /// </summary>
        // Clears and reinitializes the lists
        public void ResetLists()
        {
            typeArme.Clear();
            typeBoat.Clear();
            typeFilet.Clear();
            InitializeLists();
        }

        #endregion

    /// <summary>
    /// only convert the SocleListe To PowerGemmeObjectListe for the test
    /// </summary>
        public void GetSocleToScriptList()
        {
            PowerGemmeObjectListe.Clear();
            foreach (ComponantPowerGemmeObject item in SocleListe)
            {
            
                PowerGemmeObjectListe.Add(item.PowerGemmeObjectScript);
            }
        
        }



    #region Stat calculator

    /// <summary>
    ///  calculate the adding stat to the GBN
    /// </summary>
    public void StatCalculator()
        {
            foreach (ComponantPowerGemmeObject socle in SocleListe)
            {
                foreach (GemmeComponant gemme in socle.PowerGemmeObjectScript.GemmeComponantList)
                {
                    Gemme theGemmeScript = gemme.GemmeScript;

                    switch (typeSocle)
                    {
                        case TypeOfSocle.Arme:
                            UpdateStatsFromGemmeList<TypeWeapon>(theGemmeScript, typeArme);
                            break;
                        case TypeOfSocle.Bateau:
                            UpdateStatsFromGemmeList<TypeBoat>(theGemmeScript, typeBoat);
                            break;
                        case TypeOfSocle.Filet:
                            UpdateStatsFromGemmeList<TypeNet>(theGemmeScript, typeFilet);
                            break;
                    }
                }
            }
        }
    /// <summary>
    /// add the stat form the gemme to the good list of stat
    /// </summary>
    /// <typeparam name="TEnum"></typeparam>
    /// <param name="theGemmeScript"></param>
    /// <param name="list"></param>
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
    #endregion
}
