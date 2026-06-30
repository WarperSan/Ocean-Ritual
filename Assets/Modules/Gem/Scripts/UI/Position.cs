using BlacksmithModule;
using UnityEngine;

namespace GemModule.UI
{
    public class Position : MonoBehaviour
    {
        [SerializeField]
        private int X;

        [SerializeField]
        private int Y;

        [SerializeField]
        private ChangeColorBasedOnBool scriptChangeColor;

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
                else if (!scriptChangeColor.isActive)
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
}