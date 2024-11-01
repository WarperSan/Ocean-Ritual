using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinkWheelEquipment : MonoBehaviour
{

    [SerializeField] componentGBN equipment;
    [SerializeField] List<GameObject> PlacementPoint;
    [SerializeField] List<componentPowerGemObject> listStand = new();
    [SerializeField] float SizeBase = 1;
    [SerializeField] float rotation = 0;
    private bool DoSocle = true;
    // Start is called before the first frame update
    void Start()
    {
        if (DoSocle)
        {
            DoSocle = !DoSocle;
            PlaceStand();
        }
  
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlaceStand()
    {
        listStand = equipment.GBNScript.SocleListe;
        List<GameObject> PlacementpointBase = new List<GameObject>();
        equipment.GenerationSocle();
       
        foreach (componentPowerGemObject socle in listStand)
        {
            PlacementpointBase.Add(socle.gameObject);
            PlacementpointBase[^1].transform.position = PlacementPoint[ PlacementpointBase.Count-1].transform.position;
            PlacementpointBase[^1].transform.SetParent(PlacementPoint[PlacementpointBase.Count - 1].transform);
            PlacementpointBase[^1].transform.localScale = PlacementpointBase[^1].transform.localScale * SizeBase;
            PlacementpointBase[^1].transform.rotation = Quaternion.Euler(rotation, 0, 0);
            rotation += 45;
        }


    }
    public GameObject GetStand(int stand = 0)
    {
        if(stand< listStand.Count)
        {
            return listStand[stand].gameObject;
        }
        Debug.Log("GetStand function liste null");
       return null;
    }
    public void TransitionCam()
    {

    }

}
