using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public abstract class ItemData
{
    public int quantiterMax = 1;
    public int quantiter = 1;

    public Sprite sprite;

    public static explicit operator ItemData(UnityEngine.Object v) => throw new NotImplementedException();
}
