using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public abstract class ItemData
{
    public int quantityMax = 1;
    public int quantity = 1;
    public Sprite sprite;

    public static explicit operator ItemData(UnityEngine.Object v) => throw new NotImplementedException();
}
