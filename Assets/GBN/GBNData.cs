using Save;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static EnumGeneral;
[System.Serializable]
public class ListeGBNData 
{
    [SerializeField] public List<GBNData> ListdataGBN;

}

[System.Serializable]
public class GBNData
{
    [SerializeField] public string Name;
    [SerializeField] public TypeDeSocle typeOfSocle;
    [SerializeField] public List<TypeQuantite<TypeArme>> typeArme = new();
    [SerializeField] public List<TypeQuantite<TypeBateau>> typeBoat = new();
    [SerializeField] public List<TypeQuantite<TypeFilet>> typeFilet = new();
    [SerializeField] public List<PowerGemmeObjectData> ListPowerGemmeObjectData;

}

[System.Serializable]
public class PowerGemmeObjectData
{
    [SerializeField] public GridtData GridGemme;
    [SerializeField] public List<GemmeData> ListGemmeData;

}
[System.Serializable]
public class GridtData
{
    [SerializeField] public int width;
    [SerializeField] public int height;
    [SerializeField] public bool[,] tableau;

}
[System.Serializable]
public class GemmeData
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
    [SerializeField] public List<TypeQuantite<TypeArme>> typeArme;

    // List of boat types with quantities
    [SerializeField] public List<TypeQuantite<TypeBateau>> typeBoat;

    // List of net types with quantities
    [SerializeField] public List<TypeQuantite<TypeFilet>> typeFilet;
    #endregion


}


