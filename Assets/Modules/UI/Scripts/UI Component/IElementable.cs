using UnityEngine;

namespace UIModule
{
    /// <summary>
    /// Defines what a custom UI element should have
    /// </summary>
    public interface IElementable
    {
        public RectTransform Rect { get; }
    }
}