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
    #region List Initialization and Reset

    // Initializes the lists with default values
    /// <summary>
    /// Initiate the list
    /// </summary>
    private void InitializeLists()
        {
            foreach (TypeWeapon arme in System.Enum.GetValues(typeof(TypeWeapon)))
            {
                typeArme.Add(new TypeQuantite<TypeWeapon>(arme, 1));
            }

            foreach (TypeBoat boat in System.Enum.GetValues(typeof(TypeBoat)))
            {
                typeBoat.Add(new TypeQuantite<TypeBoat>(boat, 1));
            }

            foreach (TypeNet filet in System.Enum.GetValues(typeof(TypeNet)))
            {
                typeFilet.Add(new TypeQuantite<TypeNet>(filet, 1));
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
