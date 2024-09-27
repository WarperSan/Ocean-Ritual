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
        [SerializeField] public List<TypeQuantite<TypeWeapon>> typeWeapon = new();
        [SerializeField] public List<TypeQuantite<TypeBoat>> typeBoat = new();
        [SerializeField] public List<TypeQuantite<TypeNet>> typeNet = new();


    [SerializeField]  public List<ComponantPowerGemObject> SocleListe = new();
    // This list will be serialized but not visible in the inspector
    [HideInInspector]
    [SerializeField] public List<PowerGemObject> PowerGemmeObjectListe = new();

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
            return typeWeapon as List<TypeQuantite<TEnum>>;
        }
        else if (typeof(TEnum) == typeof(TypeBoat))
        {
            return typeBoat as List<TypeQuantite<TEnum>>;
        }
        else if (typeof(TEnum) == typeof(TypeNet))
        {
            return typeNet as List<TypeQuantite<TEnum>>;
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
                typeWeapon.Add(new TypeQuantite<TypeWeapon>(arme, 0));
            }

            foreach (TypeBoat boat in System.Enum.GetValues(typeof(TypeBoat)))
            {
                typeBoat.Add(new TypeQuantite<TypeBoat>(boat, 0));
            }

            foreach (TypeNet filet in System.Enum.GetValues(typeof(TypeNet)))
            {
                typeNet.Add(new TypeQuantite<TypeNet>(filet, 0));
            }
        }
    /// <summary>
    /// Reset the list
    /// </summary>
        // Clears and reinitializes the lists
        public void ResetLists()
        {
            typeWeapon.Clear();
            typeBoat.Clear();
            typeNet.Clear();
            InitializeLists();
        }

        #endregion

    /// <summary>
    /// only convert the SocleListe To PowerGemmeObjectListe for the test
    /// </summary>
        public void GetSocleToScriptList()
        {
            PowerGemmeObjectListe.Clear();
            foreach (ComponantPowerGemObject item in SocleListe)
            {
            
                PowerGemmeObjectListe.Add(item.PowerGemObjectScript);
            }
        
        }



    #region Stat calculator

    /// <summary>
    ///  calculate the adding stat to the GBN
    /// </summary>
    public void StatCalculator()
        {
            foreach (ComponantPowerGemObject socle in SocleListe)
            {
                foreach (GemComponant gemme in socle.PowerGemObjectScript.GemComponantList)
                {
                    Gem theGemmeScript = gemme.GemScript;

                    switch (typeSocle)
                    {
                        case TypeOfSocle.Weapon:
                            UpdateStatsFromGemmeList<TypeWeapon>(theGemmeScript, typeWeapon);
                            break;
                        case TypeOfSocle.Bateau:
                            UpdateStatsFromGemmeList<TypeBoat>(theGemmeScript, typeBoat);
                            break;
                        case TypeOfSocle.Net:
                            UpdateStatsFromGemmeList<TypeNet>(theGemmeScript, typeNet);
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
        private void UpdateStatsFromGemmeList<TEnum>(Gem theGemmeScript, List<TypeQuantite<TEnum>> list)
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
