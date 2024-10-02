using System;
using System.Collections.Generic;
using UnityEngine;
using static EnumGeneral;

[Serializable]
public class Gemme
{

    #region Data
    // X coordinate of the gem's position
    [SerializeField] public int PositionX = 0;

    // Z coordinate of the gem's position
    [SerializeField] public int PositionZ = 0;

    // Shape of the gem (2D boolean array)
    [SerializeField]
    public FormBool forme = new(new bool[,]
   {
        { false, false, true },
        { true, true, true },
        { false, false, true }
   }, 3, 3);

    // Name of the gem's color
    [SerializeField] public string GemmeColorsName;

    // Level of the gem
    [SerializeField] public int LVL = 0;

    // List of weapon types with quantities
        [SerializeField] public List<TypeQuantite<TypeWeapon>> typeArme;

        // List of boat types with quantities
        [SerializeField] public List<TypeQuantite<TypeBoat>> typeBoat;

        // List of net types with quantities
        [SerializeField] public List<TypeQuantite<TypeNet>> typeFilet;
    #endregion





    #region Type Lists Management

    
    // Method to get a list of types with quantities based on the generic type TEnum

    public List<TypeQuantite<TEnum>> GetListType<TEnum>()
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

    #endregion
}