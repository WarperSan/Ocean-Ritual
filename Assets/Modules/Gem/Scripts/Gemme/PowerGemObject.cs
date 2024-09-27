using static EnumGeneral;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PowerGemObject 
{
    #region Fields and Properties

    public GemmeGrid GridGemme;
    public GameObject SocleContainer;
     public GameObject GemContainer;


    public List<GemComponant> GemComponantList = new();
    [SerializeField] public List<Gem> GemmeList = new();
    #endregion

    public void GetGemToScriptList()
    {
        GemmeList.Clear();
        foreach (GemComponant item in GemComponantList)
        {
            GemmeList.Add(item.GemScript);
        }
    
    }

    #region Gemme Placement
    int AddSpace = 1;
    // Places gems in the grid and sets their position
    public void PlaceGem(List<GemComponant> ListGem)
    {
        foreach (GemComponant Gemmes in ListGem)
        {
            if(GridGemme.PlaceObject(Gemmes.GemScript.PositionX, Gemmes.GemScript.PositionZ, Gemmes.GemScript.forme.GetForme()))
            {
               
                GameObject theGemme = GeneratorGem.CreatGemmeObject(Gemmes.GemScript, GemContainer.transform);
               // theGemme.transform.position += new Vector3((Gemmes.GemScript.PositionX + AddSpace) * SocleGenerator.Instance.spaceBetweenCube, 0, (Gemmes.GemScript.PositionZ + AddSpace) * SocleGenerator.Instance.spaceBetweenCube);
            }
            
           
           
        }
    }

    #endregion

   

    #region Gemme Management

    // Receives a gem (implementation needed)
    public void ReceiveGemme(Gem oneGemme)
    {

    }

    // Deletes a gem (implementation needed)
    public void DeletedGemme()
    {

    }

    #endregion
}