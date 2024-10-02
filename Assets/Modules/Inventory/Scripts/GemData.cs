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
    [SerializeField] public List<TypeQuantite<TypeWeapon>> typeWeapon;

    // List of boat types with quantities
    [SerializeField] public List<TypeQuantite<TypeBoat>> typeBoat;

    // List of net types with quantities
    [SerializeField] public List<TypeQuantite<TypeNet>> typeNet;
    #endregion

}
