//using SaveModule;
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using static EnumGeneral;

using UnityEngine;

public class GBNSaveLoad: MonoBehaviour
{
//    [SerializeField] private ListeGBNData dataGBN = new();
//    [SerializeField] private List<ComponantGBN> ListGBN = new();

//    #region Save and Load
//    /// <summary>
//    /// Load the data  of dataGBN
//    /// </summary>
//    /// <param name="data"></param>
//    protected override void OnLoad(SaveData data)
//    {
//        dataGBN = data.gbnData;

//        // Convert dataGBN to ListGBN
//        ListGBN = new List<ComponantGBN>();
//        foreach (GBNData gbnData in dataGBN.ListdataGBN)
//        {
//            GameObject obj = new GameObject("ComponantGBN");
//            ComponantGBN componantGBN = obj.AddComponent<ComponantGBN>();
//            componantGBN.GBNScript = ConvertFromGBNData(gbnData);
//            ListGBN.Add(componantGBN);
//        }
//    }


//    /// <summary>
//    /// Save the data  of dataGBN
//    /// </summary>
//    /// <param name="data"></param>
//    protected override void OnSave(ref SaveData data)
//    {
//        //// Convert ListGBN to dataGBN
//        //dataGBN.ListdataGBN = new List<GBNData>();
//        //Debug.Log(" il y a " + ListGBN.Count + " object dans la Lastion ListGbN");
//        //foreach (ComponantGBN componantGBN in ListGBN)
//        //{
//        //    dataGBN.ListdataGBN.Add(ConvertToGBNData(componantGBN.GBNScript));
//        //}
//        //data.test = 3;
//        //data.gbnData = dataGBN;
//    }

//    #endregion




//    #region Convert To section
//    /// <summary>
//    ///  convert GBN to GBN data
//    /// </summary>
//    /// <param name="gbn"></param>
//    /// <returns></returns>

//    private GBNData ConvertToGBNData(GBN gbn)
//    {
        
//        var gbnData = new GBNData
//        {
//            Name = gbn.Name,
//            typeOfSocle = gbn.typeSocle,
//            typeWeapon = gbn.typeWeapon,
//            typeBoat = gbn.typeBoat,
//            typeNet = gbn.typeNet,
//            ListPowerGemmeObjectData = new List<PowerGemmeObjectData>()
//        };
      
//        foreach (PowerGemObject powerGemmeObject in gbn.PowerGemmeObjectListe)
//        {
            
//            gbnData.ListPowerGemmeObjectData.Add(ConvertToPowerGemmeObjectData(powerGemmeObject));
//        }

//        return gbnData;
//    }
//    /// <summary>
//    ///  convert PowerGemObject to PowerGemmeObjectData
//    /// </summary>
//    /// <param name="powerGemmeObject"></param>
//    /// <returns></returns>
//    private PowerGemmeObjectData ConvertToPowerGemmeObjectData(PowerGemObject powerGemmeObject)
//    {
//        var powerGemmeObjectData = new PowerGemmeObjectData
//        {
//            GridGemme = ConvertToGridData(powerGemmeObject.GridGemme),
//            ListGemmeData = new List<GemmeData>()
//        };
//        Debug.Log(" il y a " + powerGemmeObject.GemmeList.Count + " object dans la listede gemme");

//        foreach (var gemme in powerGemmeObject.GemmeList)
//        {
//            powerGemmeObjectData.ListGemmeData.Add(ConvertToGemmeData(gemme));
//        }

//        return powerGemmeObjectData;
//    }
//    /// <summary>
//    ///  convert gemme to gemme data
//    /// </summary>
//    /// <param name="gemme"></param>
//    /// <returns></returns>
//    private GemmeData ConvertToGemmeData(Gem gemme)
//    {
//        return new GemmeData
//        {
//            PositionX = gemme.PositionX,
//            PositionZ = gemme.PositionZ,
//            Shape = gemme.Shape,
//            GemColorsName = gemme.GemColorsName,
//            LVL = gemme.LVL,
//            typeWeapon = gemme.typeWeapon,
//            typeBoat = gemme.typeBoat,
//            typeNet = gemme.typeNet
//        };
//    }
//    /// <summary>
//    ///  convert GemmeGrid to Grid data
//    /// </summary>
//    /// <param name="gemme"></param>
//    /// <returns></returns>
//    private GridtData ConvertToGridData(GemmeGrid gridGemme)
//    {
//        return new GridtData
//        {
//            width = gridGemme.width,
//            height = gridGemme.height,
//            Grid = gridGemme.Grid
//        };
//    }
//    #endregion



//    #region Convert From section
//    /// <summary>
//    /// Convert GridtData to GemmeGrid
//    /// </summary>
//    /// <param name="gridData"></param>
//    /// <returns></returns>

//    private GemmeGrid ConvertFromGridData(GridtData gridData)
//    {
//        return new GemmeGrid
//        {
//            width = gridData.width,
//            height = gridData.height,
//            Grid =gridData.Grid
//        };
//    }

//    /// <summary>
//    /// Convert GBNData to GBN
//    /// </summary>
//    /// <param name="gbnData"></param>
//    /// <returns></returns>
//    private GBN ConvertFromGBNData(GBNData gbnData)
//    {
//        var gbn = new GBN
//        {
//            Name = gbnData.Name,
//            typeSocle = gbnData.typeOfSocle,
//            typeWeapon = gbnData.typeWeapon,
//            typeBoat = gbnData.typeBoat,
//            typeNet = gbnData.typeNet
//        };

//        foreach (var powerGemmeObjectData in gbnData.ListPowerGemmeObjectData)
//        {
//            gbn.PowerGemmeObjectListe.Add(ConvertFromPowerGemmeObjectData(powerGemmeObjectData));
//        }

//        return gbn;
//    }
//    /// <summary>
//    /// Convert PowerGemmeObjectData to PowerGemObject
//    /// </summary>
//    /// <param name="powerGemmeObjectData"></param>
//    /// <returns></returns>
//    private PowerGemObject ConvertFromPowerGemmeObjectData(PowerGemmeObjectData powerGemmeObjectData)
//    {
//        var powerGemmeObject = new PowerGemObject
//        {
//            GridGemme = ConvertFromGridData(powerGemmeObjectData.GridGemme),
//            GemmeList = new List<Gem>()
//        };

//        foreach (var gemmeData in powerGemmeObjectData.ListGemmeData)
//        {
//            powerGemmeObject.GemmeList.Add(ConvertFromGemmeData(gemmeData));
//        }

//        return powerGemmeObject;
//    }
//    /// <summary>
//    /// Convert GemmeData to Gem
//    /// </summary>
//    /// <param name="gemmeData"></param>
//    /// <returns></returns>
//    private Gem ConvertFromGemmeData(GemmeData gemmeData)
//    {
//        return new Gem
//        {
//            PositionX = gemmeData.PositionX,
//            PositionZ = gemmeData.PositionZ,
//            Shape = gemmeData.Shape,
//            GemColorsName = gemmeData.GemColorsName,
//            LVL = gemmeData.LVL,
//            typeWeapon = gemmeData.typeWeapon,
//            typeBoat = gemmeData.typeBoat,
//            typeNet = gemmeData.typeNet
//        };
//    }
//    #endregion
}