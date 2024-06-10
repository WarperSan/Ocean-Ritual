using static EnumGeneral;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PowerGemmeObject 
{
    #region Fields and Properties

    public GemmeGrid GridGemme;
    [SerializeField] public GameObject SocleConteneur;
    [SerializeField] private GameObject GemmeConteneur;


    [SerializeField] public List<GemmeComponant> GemmeList = new();

    #endregion



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