using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseHoverManager : MonoBehaviour
{
    [SerializeField] GemData Gem;
    [SerializeField] FishData Fish;
    [SerializeField] GestionInformation HoverItem;
    [SerializeField] bool test  = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (test)
        {
            test = !test;
            GestionInformation.Instance.GetNewInformationGem(Gem);

        }
    }
}
