using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LienRoueEquipement : MonoBehaviour
{

    [SerializeField] ComponantGBN equipement;
    [SerializeField] List<GameObject> PointDePlacement;
    [SerializeField] float GrosseurSocle = 1;
    [SerializeField] float rotation = 0;
    private bool faireSocle = true;
    // Start is called before the first frame update
    void Start()
    {
        if (faireSocle)
        {
            faireSocle = !faireSocle;
            placerSocle();
        }
  
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void placerSocle()
    {
        List<ComponantPowerGemmeObject> listSocle = equipement.GBNScript.SocleListe;
        List<GameObject> pointDePlacementSocle = new List<GameObject>();
        equipement.GenereSocle();
       
        foreach (ComponantPowerGemmeObject socle in listSocle)
        {
            pointDePlacementSocle.Add(socle.gameObject);
            pointDePlacementSocle[^1].transform.position = PointDePlacement[ pointDePlacementSocle.Count-1].transform.position;
            pointDePlacementSocle[^1].transform.SetParent(PointDePlacement[pointDePlacementSocle.Count - 1].transform);
            pointDePlacementSocle[^1].transform.localScale = pointDePlacementSocle[^1].transform.localScale * GrosseurSocle;
            pointDePlacementSocle[^1].transform.rotation = Quaternion.Euler(rotation, 0, 0);
            rotation += 45;
        }
   
       

    }

}
