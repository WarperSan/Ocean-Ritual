using System.Collections.Generic;
using UnityEngine;

public class LinkWheelEquipment : MonoBehaviour
{
    [SerializeField]
    private SelectionWheel selectionWheel;

    [SerializeField]
    private componentGBN equipment;

    [SerializeField]
    private List<GameObject> PlacementPoint;

    [SerializeField]
    private List<componentPowerGemObject> listStand = new();

    [SerializeField]
    private List<GameObject> PlacementpointBase = new();

    [SerializeField]
    private GameObject Wheel;

    [SerializeField]
    private MouseWheelManager mouseWheelManager;

    [SerializeField]
    private float SizeBase = 1;

    [SerializeField]
    private float rotation;

    [SerializeField]
    private bool remove;

    [SerializeField]
    private bool DoSocle = true;

    [SerializeField]
    private int skippedSocle;

    // Start is called before the first frame update
    private void Start()
    {
        if (DoSocle)
        {
            DoSocle = !DoSocle;
            PlaceStand();
        }
    }

    // Update is called once per frame
    private void Update()
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

    public void ResetStandFromUpgrade()
    {
        if (equipment == null)
            equipment = selectionWheel.listeForgeable[0].GetComponent<componentGBN>();
        ResetStand(equipment);
    }

    public void ResetStand(componentGBN equipement)
    {
        // Debug.Log("allo resetStand");
        RemoveStand();
        getNewEquipement(equipement);
        PlaceStand();
    }

    public void getNewEquipement(componentGBN equipement) => equipment = equipement;

    public void PlaceStand()
    {
        Wheel.transform.rotation = Quaternion.identity;
        listStand = equipment.GBNScript.SocleListe;
        PlacementpointBase = new List<GameObject>();
        equipment.GenerationSocle();

        foreach (componentPowerGemObject socle in listStand)
        {
            PlacementpointBase.Add(socle.gameObject);
            PlacementpointBase[^1].transform.position = PlacementPoint[PlacementpointBase.Count - 1].transform.position;
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

    //max 8 min 1 dans la liste donc de l'index 0 � 7
    private int stand;

    public GameObject GetStand()
    {
        if (stand < listStand.Count && listStand.Count > 0)
            return listStand[stand].gameObject;

        Debug.Log("GetStand function liste null");
        return null;
    }

    public bool IsAlone() => listStand.Count == 1;

    public GameObject GetNextSocle()
    {
        // V�rifie si la liste contient des socles
        if (listStand.Count > 0)
        {
            // D�cr�mente stand et boucle au d�but si la fin est atteinte
            int previousStand = stand;
            stand = (stand - 1 + listStand.Count) % listStand.Count;

            // Calcule le nombre d'emplacements "saut�s" uniquement si le saut s'effectue (lors du retour au d�but)
            if (previousStand == 0 && stand == listStand.Count - 1)
            {
                skippedSocle = 8 - listStand.Count;
                Debug.Log("Skipped slots (GetNextSocle): " + skippedSocle);
            }
            else
                skippedSocle = 0;

            return listStand[stand].gameObject;
        }

        Debug.Log("GetNextSocle function liste vide");
        return null;
    }

    public GameObject GetPreviewSocle()
    {
        // V�rifie si la liste contient des socles
        if (listStand.Count > 0)
        {
            // Incr�mente stand et boucle � la fin si le d�but est atteint
            int previousStand = stand;
            stand = (stand + 1) % listStand.Count;

            // Calcule le nombre d'emplacements "saut�s" uniquement si le saut s'effectue (lors du retour � la fin)
            if (previousStand == listStand.Count - 1 && stand == 0)
            {
                skippedSocle = 8 - listStand.Count;
                Debug.Log("Skipped slots (GetPreviewSocle): " + skippedSocle);
            }
            else
                skippedSocle = 0;
            return listStand[stand].gameObject;
        }

        Debug.Log("GetPreviewSocle function liste vide");
        return null;
    }

    public int GetSkippedSocle() => skippedSocle;

    public void TransitionCam()
    {
    }
}