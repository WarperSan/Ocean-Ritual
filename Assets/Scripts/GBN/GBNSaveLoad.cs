using Save;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static EnumGeneral;

public class GBNSaveLoad : SaveBehaviour
{
    [SerializeField] private ListeGBNData dataGBN = new();
    [SerializeField] private List<ComponantGBN> ListGBN = new();

    #region Save and Load
    /// <summary>
    /// Load the data  of dataGBN
    /// </summary>
    /// <param name="data"></param>
    protected override void OnLoad(SaveData data)
    {
        dataGBN = data.gbnData;

        // Convert dataGBN to ListGBN
        ListGBN = new List<ComponantGBN>();
        foreach (GBNData gbnData in dataGBN.ListdataGBN)
        {
            GameObject obj = new GameObject("ComponantGBN");
            ComponantGBN componantGBN = obj.AddComponent<ComponantGBN>();
            componantGBN.GBNScript = ConvertFromGBNData(gbnData);
            ListGBN.Add(componantGBN);
        }
    }


    /// <summary>
    /// Save the data  of dataGBN
    /// </summary>
    /// <param name="data"></param>
    protected override void OnSave(ref SaveData data)
    {
        //// Convert ListGBN to dataGBN
        //dataGBN.ListdataGBN = new List<GBNData>();
        //Debug.Log(" il y a " + ListGBN.Count + " object dans la Lastion ListGbN");
        //foreach (ComponantGBN componantGBN in ListGBN)
        //{
        //    dataGBN.ListdataGBN.Add(ConvertToGBNData(componantGBN.GBNScript));
        //}
        //data.test = 3;
        //data.gbnData = dataGBN;
    }

    #endregion




    #region Convert To section
    /// <summary>
    ///  convert GBN to GBN data
    /// </summary>
    /// <param name="gbn"></param>
    /// <returns></returns>

    private GBNData ConvertToGBNData(GBN gbn)
    {
        
        var gbnData = new GBNData
        {
            Name = gbn.Name,
            typeOfSocle = gbn.typeSocle,
            typeArme = gbn.typeArme,
            typeBoat = gbn.typeBoat,
            typeFilet = gbn.typeFilet,
            ListPowerGemmeObjectData = new List<PowerGemmeObjectData>()
        };
      
        foreach (PowerGemmeObject powerGemmeObject in gbn.PowerGemmeObjectListe)
        {
            
            gbnData.ListPowerGemmeObjectData.Add(ConvertToPowerGemmeObjectData(powerGemmeObject));
        }

        return gbnData;
    }
    /// <summary>
    ///  convert PowerGemmeObject to PowerGemmeObjectData
    /// </summary>
    /// <param name="powerGemmeObject"></param>
    /// <returns></returns>
    private PowerGemmeObjectData ConvertToPowerGemmeObjectData(PowerGemmeObject powerGemmeObject)
    {
        var powerGemmeObjectData = new PowerGemmeObjectData
        {
            GridGemme = ConvertToGridData(powerGemmeObject.GridGemme),
            ListGemmeData = new List<GemmeData>()
        };
        Debug.Log(" il y a " + powerGemmeObject.GemmeList.Count + " object dans la listede gemme");

        foreach (var gemme in powerGemmeObject.GemmeList)
        {
            powerGemmeObjectData.ListGemmeData.Add(ConvertToGemmeData(gemme));
        }

        return powerGemmeObjectData;
    }
    /// <summary>
    ///  convert gemme to gemme data
    /// </summary>
    /// <param name="gemme"></param>
    /// <returns></returns>
    private GemmeData ConvertToGemmeData(Gemme gemme)
    {
        return new GemmeData
        {
            PositionX = gemme.PositionX,
            PositionZ = gemme.PositionZ,
            forme = gemme.forme,
            GemmeColorsName = gemme.GemmeColorsName,
            LVL = gemme.LVL,
            typeArme = gemme.typeArme,
            typeBoat = gemme.typeBoat,
            typeFilet = gemme.typeFilet
        };
    }
    /// <summary>
    ///  convert GemmeGrid to Grid data
    /// </summary>
    /// <param name="gemme"></param>
    /// <returns></returns>
    private GridtData ConvertToGridData(GemmeGrid gridGemme)
    {
        return new GridtData
        {
            width = gridGemme.width,
            height = gridGemme.height,
            tableau = gridGemme.tableau
        };
    }
    #endregion



    #region Convert From section
    /// <summary>
    /// Convert GridtData to GemmeGrid
    /// </summary>
    /// <param name="gridData"></param>
    /// <returns></returns>

    private GemmeGrid ConvertFromGridData(GridtData gridData)
    {
        return new GemmeGrid
        {
            width = gridData.width,
            height = gridData.height,
            tableau =gridData.tableau
        };
    }

    /// <summary>
    /// Convert GBNData to GBN
    /// </summary>
    /// <param name="gbnData"></param>
    /// <returns></returns>
    private GBN ConvertFromGBNData(GBNData gbnData)
    {
        var gbn = new GBN
        {
            Name = gbnData.Name,
            typeSocle = gbnData.typeOfSocle,
            typeArme = gbnData.typeArme,
            typeBoat = gbnData.typeBoat,
            typeFilet = gbnData.typeFilet
        };

        foreach (var powerGemmeObjectData in gbnData.ListPowerGemmeObjectData)
        {
            gbn.PowerGemmeObjectListe.Add(ConvertFromPowerGemmeObjectData(powerGemmeObjectData));
        }

        return gbn;
    }
    /// <summary>
    /// Convert PowerGemmeObjectData to PowerGemmeObject
    /// </summary>
    /// <param name="powerGemmeObjectData"></param>
    /// <returns></returns>
    private PowerGemmeObject ConvertFromPowerGemmeObjectData(PowerGemmeObjectData powerGemmeObjectData)
    {
        var powerGemmeObject = new PowerGemmeObject
        {
            GridGemme = ConvertFromGridData(powerGemmeObjectData.GridGemme),
            GemmeList = new List<Gemme>()
        };

        foreach (var gemmeData in powerGemmeObjectData.ListGemmeData)
        {
            powerGemmeObject.GemmeList.Add(ConvertFromGemmeData(gemmeData));
        }

        return powerGemmeObject;
    }
    /// <summary>
    /// Convert GemmeData to Gemme
    /// </summary>
    /// <param name="gemmeData"></param>
    /// <returns></returns>
    private Gemme ConvertFromGemmeData(GemmeData gemmeData)
    {
        return new Gemme
        {
            PositionX = gemmeData.PositionX,
            PositionZ = gemmeData.PositionZ,
            forme = gemmeData.forme,
            GemmeColorsName = gemmeData.GemmeColorsName,
            LVL = gemmeData.LVL,
            typeArme = gemmeData.typeArme,
            typeBoat = gemmeData.typeBoat,
            typeFilet = gemmeData.typeFilet
        };
    }
    #endregion
}