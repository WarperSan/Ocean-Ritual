using UnityEngine;
using UnityEngine.UI;

namespace ExtensionsModule
{
    public static class GraphicExtension
    {
        /// <summary>
        /// Sets the alpha value of this graphic
        /// </summary>
        public static void SetAlpha(this Graphic graphic, float alpha)
        {
            Color color = graphic.color;
            color.a = Mathf.Clamp01(alpha);
            graphic.color = color;
        }
    }
}