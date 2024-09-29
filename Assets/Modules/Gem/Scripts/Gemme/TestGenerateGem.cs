using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestGenerateGem : MonoBehaviour
{
    [SerializeField] int lvlGem1;
    [SerializeField] int lvlGem2;
    [SerializeField] int lvlFinal;
    [SerializeField] bool test =false;
    [SerializeField] GemData GemData = new ();
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (test)
        {
            test = false;
            int lvl = GemHelperBlacksmith.fusionGemTab(lvlGem1, lvlGem2);
            lvlGem1 = lvl;
            lvlGem2 = lvl+1;
            GemData = GeneratorGem.GenerateRandomGemme(lvl, GemData);
        }
    }
}
