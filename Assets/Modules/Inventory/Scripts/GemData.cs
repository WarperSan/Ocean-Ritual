using System.Collections.Generic;
using UnityEngine;
using static EnumGeneral;

[System.Serializable]
public class GemData : ItemData
{
    #region Data

    // Shape of the gems (2D boolean array)
    [SerializeField]
    public FormBool Shape;

    // Name of the gems's color
    [SerializeField]
    public string GemColorsName;

    // Level of the gems
    [SerializeField]
    public int LVL;

    // List of weapon types with quantities
    [SerializeField]
    public List<TypeQuantity<TypeWeapon>> typeWeapon;

    // List of boat types with quantities
    [SerializeField]
    public List<TypeQuantity<TypeBoat>> typeBoat;

    // List of net types with quantities
    [SerializeField]
    public List<TypeQuantity<TypeNet>> typeNet;

    #endregion

    public GemData() { }

    public GemData(GemData copy)
    {
        Shape = new FormBool(copy.Shape.flatForme, copy.Shape.width, copy.Shape.height);
        GemColorsName = copy.GemColorsName;
        LVL = copy.LVL;
        typeWeapon = new List<TypeQuantity<TypeWeapon>>(copy.typeWeapon);
        typeBoat = new List<TypeQuantity<TypeBoat>>(copy.typeBoat);
        typeNet = new List<TypeQuantity<TypeNet>>(copy.typeNet);
    }
}