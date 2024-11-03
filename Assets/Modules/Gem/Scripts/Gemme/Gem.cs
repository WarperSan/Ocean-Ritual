using System;
using System.Collections.Generic;
using UnityEngine;
using static EnumGeneral;

[Serializable]
public class Gem
{

    public Gem()
    {

    }
    public Gem(GemData gemdata)
    {
        var gemConvert = GemHelper.ConvertGemDataToGem(gemdata);
        this.PositionX = gemConvert.PositionX;
        this.PositionZ = gemConvert.PositionZ;
        this.form = gemConvert.form;
        this.GemColorsName = gemConvert.GemColorsName;
        this.LVL = gemConvert.LVL;
        this.typeWeapon = gemConvert.typeWeapon;
        this.typeBoat = gemConvert.typeBoat;
        this.typeNet = gemConvert.typeNet;
        this.sprite = gemConvert.sprite;
    }
    #region Data
    [SerializeField] public  Sprite sprite =null;
    // X coordinate of the gems's position
    [SerializeField] public int PositionX = 0;

    // Z coordinate of the gems's position
    [SerializeField] public int PositionZ = 0;

    // Shape of the gems (2D boolean array)
    [SerializeField]
    public FormBool form = new(new bool[,]
   {
        { false, false, true },
        { true, true, true },
        { false, false, true }
   }, 3, 3);

    // Name of the gems's color
    [SerializeField] public string GemColorsName;

    // Level of the gems
    [SerializeField] public int LVL = 0;

    // List of weapon types with quantities
        [SerializeField] public List<TypeQuantity<TypeWeapon>> typeWeapon;

        // List of boat types with quantities
        [SerializeField] public List<TypeQuantity<TypeBoat>> typeBoat;

        // List of net types with quantities
        [SerializeField] public List<TypeQuantity<TypeNet>> typeNet;
    #endregion





    #region Type Lists Management

    
    // Method to get a list of types with quantities based on the generic type TEnum

    public List<TypeQuantity<TEnum>> GetListType<TEnum>()
    {
        if (typeof(TEnum) == typeof(TypeWeapon))
        {
            return typeWeapon as List<TypeQuantity<TEnum>>;
        }
        else if (typeof(TEnum) == typeof(TypeBoat))
        {
            return typeBoat as List<TypeQuantity<TEnum>>;
        }
        else if (typeof(TEnum) == typeof(TypeNet))
        {
            return typeNet as List<TypeQuantity<TEnum>>;
        }
        else
        {
            Debug.LogError("Unsupported type.");
            return null;
        }
    }

    #endregion
}