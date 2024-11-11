using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LinkWheelEquipment : MonoBehaviour
{

    [SerializeField] componentGBN equipment;
    [SerializeField] List<GameObject> PlacementPoint;
    [SerializeField] List<componentPowerGemObject> listStand = new();
    [SerializeField] List<GameObject> PlacementpointBase = new List<GameObject>();
    [SerializeField] GameObject Wheel;
    [SerializeField] MouseWheelManager mouseWheelManager;
    [SerializeField] float SizeBase = 1;
    [SerializeField] float rotation = 0;
    [SerializeField] bool remove = false;
    [SerializeField] bool DoSocle = true;
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
        if (DoSocle)
        {
            DoSocle = !DoSocle;
            PlaceStand();
        }
        if (remove)
        {
            remove = !remove;
            RemoveStand();
        }
    }
    public void ResetStand(componentGBN equipement)
    {
        Debug.Log("allo resetStand");
        RemoveStand();
        getNewEquipement(equipement);
        PlaceStand();

    }
    public void getNewEquipement(componentGBN equipement)
    {
        this.equipment= equipement;
    }

    public void PlaceStand()
    {

        Wheel.transform.rotation = Quaternion.identity;
        listStand = equipment.GBNScript.SocleListe;
       PlacementpointBase = new List<GameObject>();
        equipment.GenerationSocle();
       
        foreach (componentPowerGemObject socle in listStand)
        {
            PlacementpointBase.Add(socle.gameObject);
            PlacementpointBase[^1].transform.position = PlacementPoint[ PlacementpointBase.Count-1].transform.position;
            PlacementpointBase[^1].transform.SetParent(PlacementPoint[PlacementpointBase.Count - 1].transform);
            PlacementpointBase[^1].transform.localScale = new Vector3(SizeBase, SizeBase, SizeBase);
            PlacementpointBase[^1].transform.rotation = Quaternion.Euler(rotation, 0, 0);
            rotation += 45;
        }

        stand = 0;
        mouseWheelManager.resetSrinkObject();
    }

    public void RemoveStand()
    {
         rotation = 0;
        foreach (GameObject socle in PlacementpointBase)
        {
            socle.transform.SetParent(equipment.transform);
            componentPowerGemObject scriptComponant = socle.GetComponent<componentPowerGemObject>();
            scriptComponant.PowerGemObjectScript.RemoveStand();
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
