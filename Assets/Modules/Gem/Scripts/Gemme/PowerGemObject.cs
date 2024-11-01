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


    public List<Gemcomponent> GemcomponentList = new();
    [SerializeField] public List<Gem> GemmeList = new();
    [SerializeField] Gem TemporaryGem;
    #endregion

    public void GetGemToScriptList()
    {
        GemmeList.Clear();
        foreach (Gemcomponent item in GemcomponentList)
        {
            GemmeList.Add(item.GemScript);
        }
    
    }
    public void ReceiveGemData(GemData gemData)
    {
        TemporaryGem = new(gemData);
       
    }
   
    public void ReceiveGemData(Gem gemData)
    {
        TemporaryGem = gemData;
    }
    #region Gemme Placement
    //   int AddSpace = 1;
    // Places gems in the grid and sets their position
    public void PlaceGem(List<Gemcomponent> ListGem)
    {
        foreach (Gemcomponent Gemmes in ListGem)
        {
            if(GridGemme.PlaceObject(Gemmes.GemScript.PositionX, Gemmes.GemScript.PositionZ, Gemmes.GemScript.form.GetForme()))
            {
               
                GameObject theGemme = GeneratorGem.CreatGemmeObject(Gemmes.GemScript, GemContainer.transform);
               // theGemme.transform.position += new Vector3((Gemmes.GemScript.PositionX + AddSpace) * SocleGenerator.Instance.spaceBetweenCube, 0, (Gemmes.GemScript.PositionZ + AddSpace) * SocleGenerator.Instance.spaceBetweenCube);
            }
            
           
           
        }
    }

    #endregion

    
   public bool TryPlaceTemporaryGem(int x,int z)
    {
        //   Debug.Log(TemporaryGem.PositionX);
        //Debug.Log(TemporaryGem.PositionZ);
        if (GridGemme.TryPlaceObjectOnGrid(x,z, TemporaryGem))
        {
           // Debug.Log(TemporaryGem.PositionX);
          // Debug.Log(TemporaryGem.PositionZ);
            GemmeList.Add(TemporaryGem);
            //TemporaryGem = null;
          // Debug.Log(GemmeList.Count);


            return true;
        }
      //  Debug.Log("false TryPlaceTemporaryGem");
        return false;
    }
    public void  GivePositionRef(Gem gem)
    {
        gem.PositionX =  TemporaryGem.PositionX  ;
        gem.PositionZ =  TemporaryGem.PositionZ ;
    }

    #region Gemme Management

    // Receives a gems (implementation needed)
    public void ReceiveGemme(Gem oneGemme)
    {

    }

    // Deletes a gems (implementation needed)
    public void DeletedGemme()
    {

    }

    #endregion
}