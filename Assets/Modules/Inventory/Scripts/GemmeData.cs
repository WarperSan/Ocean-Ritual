using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static EnumGeneral;

[System.Serializable]
public class GemmeData : ItemData
{
    #region Data


    // Shape of the gem (2D boolean array)
    [SerializeField]
    public FormBool? forme;

    // Name of the gem's color
    [SerializeField] public string GemmeColorsName;

    // Level of the gem
    [SerializeField] public int LVL = 0;

    // List of weapon types with quantities
    [SerializeField] public List<TypeQuantity<TypeWeapon>> typeArme;

    // List of boat types with quantities
    [SerializeField] public List<TypeQuantity<TypeBoat>> typeBoat;

    // List of net types with quantities
    [SerializeField] public List<TypeQuantity<TypeNet>> typeFilet;
    #endregion

}
