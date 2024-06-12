using Save;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static EnumGeneral;

public class GBNSaveLoad : SaveBehaviour
{
    [SerializeField] private ListeGBNData dataGBN = new();
    [SerializeField] private List<ComponantGBN> ListGBN = new();


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

    private GBNData ConvertToGBNData(GBN gbn)
    {
        
        var gbnData = new GBNData
        {
            Name = gbn.Name,
            typeDeSocle = gbn.typeDeSocle,
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
    private GridtData ConvertToGridData(GemmeGrid gridGemme)
    {
        return new GridtData
        {
            width = gridGemme.width,
            height = gridGemme.height,
            tableau = gridGemme.tableau
        };
    }
    private GemmeGrid ConvertFromGridData(GridtData gridData)
    {
        return new GemmeGrid
        {
            width = gridData.width,
            height = gridData.height,
            tableau =gridData.tableau
        };
    }
    private GBN ConvertFromGBNData(GBNData gbnData)
    {
        var gbn = new GBN
        {
            Name = gbnData.Name,
            typeDeSocle = gbnData.typeDeSocle,
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
}