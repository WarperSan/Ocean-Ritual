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
        foreach (var gbnData in dataGBN.ListdataGBN)
        {
            ComponantGBN componantGBN = new ComponantGBN();
            componantGBN.GBNScript = ConvertFromGBNData(gbnData);
            ListGBN.Add(componantGBN);
        }
    }

    protected override void OnSave(ref SaveData data)
    {
        // Convert ListGBN to dataGBN
        dataGBN.ListdataGBN = new List<GBNData>();
        Debug.Log(" il y a " + ListGBN.Count + " object dans la Lastion ListGbN");
        foreach (ComponantGBN componantGBN in ListGBN)
        {
            dataGBN.ListdataGBN.Add(ConvertToGBNData(componantGBN.GBNScript));
        }
        data.test = 3;
        data.gbnData = dataGBN;
    }

    private GBNData ConvertToGBNData(GBN gbn)
    {
        Debug.Log(gbn.Name);
        var gbnData = new GBNData
        {
            Name = gbn.Name,
            typeDeSocle = gbn.typeDeSocle,
            typeArme = gbn.typeArme,
            typeBoat = gbn.typeBoat,
            typeFilet = gbn.typeFilet,
            ListdataGBN = new List<PowerGemmeObjectData>()
        };
        Debug.Log(gbn.PowerGemmeObjectListe.Count);
        foreach (PowerGemmeObject powerGemmeObject in gbn.PowerGemmeObjectListe)
        {
            Debug.Log("passe ici");
            gbnData.ListdataGBN.Add(ConvertToPowerGemmeObjectData(powerGemmeObject));
        }

        return gbnData;
    }

    private PowerGemmeObjectData ConvertToPowerGemmeObjectData(PowerGemmeObject powerGemmeObject)
    {
        var powerGemmeObjectData = new PowerGemmeObjectData
        {
            GridGemme = powerGemmeObject.GridGemme,
            ListdataGBN = new List<GemmeData>()
        };

        foreach (var gemme in powerGemmeObject.GemmeList)
        {
            powerGemmeObjectData.ListdataGBN.Add(ConvertToGemmeData(gemme));
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

        foreach (var powerGemmeObjectData in gbnData.ListdataGBN)
        {
            gbn.PowerGemmeObjectListe.Add(ConvertFromPowerGemmeObjectData(powerGemmeObjectData));
        }

        return gbn;
    }

    private PowerGemmeObject ConvertFromPowerGemmeObjectData(PowerGemmeObjectData powerGemmeObjectData)
    {
        var powerGemmeObject = new PowerGemmeObject
        {
            GridGemme = powerGemmeObjectData.GridGemme,
            GemmeList = new List<Gemme>()
        };

        foreach (var gemmeData in powerGemmeObjectData.ListdataGBN)
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