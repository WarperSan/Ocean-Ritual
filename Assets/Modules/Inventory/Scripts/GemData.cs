using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static EnumGeneral;

[System.Serializable]
public class GemData : ItemData
{
    #region Data


    // Shape of the gem (2D boolean array)
    [SerializeField]
    public FormBool? Shape;

    // Name of the gem's color
    [SerializeField] public string GemColorsName;

    // Level of the gem
    [SerializeField] public int LVL = 0;

    // List of weapon types with quantities
    [SerializeField] public List<TypeQuantity<TypeWeapon>> typeWeapon;

    // List of boat types with quantities
    [SerializeField] public List<TypeQuantity<TypeBoat>> typeBoat;

    // List of net types with quantities
    [SerializeField] public List<TypeQuantity<TypeNet>> typeNet;
    #endregion

    public GemData() { }
    public GemData(GemData copy)
    {
        this.Shape = new FormBool(copy.Shape.flatForme, copy.Shape.width, copy.Shape.height);
        this.GemColorsName = copy.GemColorsName;
        this.LVL = copy.LVL;
        this.typeWeapon = new(copy.typeWeapon);
        this.typeBoat = new(copy.typeBoat);
        this.typeNet = new(copy.typeNet);
    }

}
