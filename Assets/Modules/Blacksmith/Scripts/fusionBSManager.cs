using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fusionBSManager : MonoBehaviour
{
    [SerializeField] Fill fill;
    [SerializeField] GameObject button;
    [SerializeField] FusionCase left;
    [SerializeField] FusionCase right;
    [SerializeField] FusionCase mid;
    [SerializeField] Sprite sprite;
    bool fuse = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(left.gems !=null && right.gems != null)
        {
            if (left.gems.LVL != 0 && right.gems.LVL != 0)
            {
                button.SetActive(true);
            }
            else
            {
                button.SetActive(false);
            }
        }
        
        else
        {
            button.SetActive(false);
        }
       
        if (fill.CanFuse)
        {
            if(!fuse)
            {
                int lvl = GemHelper.fusionGemTab(left.gems.LVL, right.gems.LVL);
                GemData TheGemme = GeneratorGem.GenerateRandomGemme(lvl, left.gems);
                TheGemme.sprite = sprite;
                mid.ReceiveGemFromFusion(TheGemme);
                fuse= true;
                left.Resete();
                right.Resete();
            }
            
            
        }
       
        
    }
}
