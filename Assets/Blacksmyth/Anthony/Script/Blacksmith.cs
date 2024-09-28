using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blacksmith : MonoBehaviour
{
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
   public void GetAllUpgrade()
    {

    }
}
