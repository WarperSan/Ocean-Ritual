    using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static EnumGeneral;

public class Blacksmith : MonoBehaviour
{
    [SerializeField] public List<Forgeable> forgeableItems = new ();

    [SerializeField] public List<UpgradeStats> ListStat;
    private static Blacksmith instance;

    public static Blacksmith Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<Blacksmith>();
                if (instance == null)
                {
                    GameObject obj = new GameObject("Blacksmith");
                    instance = obj.AddComponent<Blacksmith>();
                }
            }
            return instance;
        }
    }

    private void Start()
    {
        getAllUpgradeableItem();
        GetAllUpgrade();
       UiBSGBN.Instance.CreateUiGBNUpgrade(ListStat);
    }


    public void GetAllUpgrade()
    {
        ListStat.Clear(); // Assurez-vous de vider la liste avant d'ajouter de nouveaux éléments

        // Parcours de chaque forgeable item
        foreach (var item in forgeableItems)
        {
            
                // Récupère les données d'amélioration
                UpgradeStats upgradeData = item.GetStatToUpgradeAndCost();

                // Ajoute les données à ListStat
                ListStat.Add(upgradeData);
            
        }
    }

    public void getAllUpgradeableItem()
    {
        IEnumerable<Forgeable> list = FindObjectsOfType<MonoBehaviour>().OfType<Forgeable>();
        forgeableItems =   new List<Forgeable>(list);
    }
}
