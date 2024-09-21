using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public abstract class ItemData
{
    public int quantiterMax;
   public int quantiter;

    public static explicit operator ItemData(UnityEngine.Object v) => throw new NotImplementedException();
}
