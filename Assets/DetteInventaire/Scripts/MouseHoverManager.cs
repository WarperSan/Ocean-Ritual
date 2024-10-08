using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseHoverManager : MonoBehaviour
{
    [SerializeField] GemData Gem;
    [SerializeField] FishData Fish;
    [SerializeField] GestionInformation HoverItem;
    [SerializeField] bool testGem = false;
    [SerializeField] bool testfish = false;
    [SerializeField] GameObject ActifInventorie;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (ActifInventorie.active)
        {
            //if ()
            //{
            //   Inventory.Instance.GetInventoryItem(itemIndex) ,
            //}
           
        }
        if (testGem)
        {
            testGem = !testGem;
            GestionInformation.Instance.GetNewInformationGem(Gem);

        }
        if (testfish)
        {
            testfish = !testfish;
            GestionInformation.Instance.GetNewInformationFish(Fish);

        }
    }
}
