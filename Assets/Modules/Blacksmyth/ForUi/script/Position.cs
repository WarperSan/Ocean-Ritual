using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Position : MonoBehaviour
{
    [SerializeField] int X;
    [SerializeField] int Y;
    [SerializeField] ChangeColorBasedOnBool scriptChangeColor;
    public void SetPoition(int x, int y)
    {
        X = x;
        Y = y;
    }
    public void GivePosition()
    {
        if (TestBlackSmith.Instance.canAddNewCase)
        {
           if (!TestBlackSmith.Instance.CasseOnlytrue)
            {
                scriptChangeColor.SwapState();
                TestBlackSmith.Instance.ChangeValueGemme(X, Y, scriptChangeColor.isActive);
            }
          else if(!scriptChangeColor.isActive)
            {
                scriptChangeColor.SwapState();
                TestBlackSmith.Instance.ChangeValueGemme(X, Y, scriptChangeColor.isActive);
            }
           
        }
        else if (!TestBlackSmith.Instance.canAddNewCase && scriptChangeColor.isActive)
        {
            if (!TestBlackSmith.Instance.CasseOnlytrue)
            {

                scriptChangeColor.SwapState();
                TestBlackSmith.Instance.ChangeValueGemme(X, Y, scriptChangeColor.isActive);
            }
        }
    }
}
