using static EnumGeneral;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PowerGemmeObject 
{
    #region Fields and Properties

    public GemmeGrid GridGemme;
    public GameObject SocleConteneur;
     public GameObject GemmeConteneur;


    public List<GemmeComponant> GemmeComponantList = new();
    [SerializeField] public List<Gemme> GemmeList = new();
    #endregion

    public void GetGemmeToScriptList()
    {
        GemmeList.Clear();
        foreach (GemmeComponant item in GemmeComponantList)
        {
            GemmeList.Add(item.GemmeScript);
        }
        Debug.Log(GemmeList.Count);
    }

    #region Gemme Placement

    // Places gems in the grid and sets their position
    public void PlacerGemme(List<GemmeComponant> ListGemme)
    {
        foreach (GemmeComponant Gemmes in ListGemme)
        {
            GridGemme.PlaceObject(Gemmes.GemmeScript.PositionX, Gemmes.GemmeScript.PositionZ, Gemmes.GemmeScript.forme.GetForme());
            GameObject theGemme = GeneratorGemme.Instance.CreatGemmeObject(Gemmes.GemmeScript, GemmeConteneur.transform);
            theGemme.transform.position += new Vector3(Gemmes.GemmeScript.PositionX * SocleGenerator.Instance.spaceBetweenCube, 0, Gemmes.GemmeScript.PositionZ * SocleGenerator.Instance.spaceBetweenCube);
        }
    }

    #endregion

   

    #region Gemme Management

    // Receives a gem (implementation needed)
    public void ReceiveGemme(Gemme oneGemme)
    {

    }

    // Deletes a gem (implementation needed)
    public void DeletedGemme()
    {

    }

    #endregion
}