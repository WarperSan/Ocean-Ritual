using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static EnumGeneral;

[System.Serializable]
public class GemmeData : ItemData
{
    #region Data
    // X coordinate of the gem's position
    [SerializeField] public int PositionX = 0;

    // Z coordinate of the gem's position
    [SerializeField] public int PositionZ = 0;

    // Shape of the gem (2D boolean array)
    [SerializeField]
    public FormeBool? forme;

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

}
