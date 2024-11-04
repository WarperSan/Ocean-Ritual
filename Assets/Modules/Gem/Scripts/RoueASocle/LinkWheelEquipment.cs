using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    //max 8 min 1 dans la liste donc de l'index 0 à 7
    int stand = 0;
    public GameObject GetStand()
    {
        if (stand < listStand.Count && listStand.Count > 0)
        {
            return listStand[stand].gameObject;
        }
        Debug.Log("GetStand function liste null");
        return null;
    }

    public GameObject GetNextSocle()
    {
        // Décrémente Incrémente stand et boucle au début si la fin est atteinte
        if (listStand.Count > 0)
        {
            stand = (stand - 1 + listStand.Count) % listStand.Count;
            return listStand[stand].gameObject;
        }
        Debug.Log("GetNextSocle function liste vide");
        return null;
    }

    public GameObject GetPreviewSocle()
    {
        // Incrémente stand et boucle à la fin si le début est atteint
        if (listStand.Count > 0)
        {
            stand = (stand + 1) % listStand.Count;
            return listStand[stand].gameObject;
        }
        Debug.Log("GetPreviewSocle function liste vide");
        return null;
    }
    public void TransitionCam()
    {

    }

}
