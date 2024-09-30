using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Position : MonoBehaviour
{
    [SerializeField] int X;
    [SerializeField] int Y;

    public void SetPoition(int x, int y)
    {
        X=x;
        Y=y;
    }
    public (int x, int y ) GivePosition()
    {
        return (X, Y);
    }
}
