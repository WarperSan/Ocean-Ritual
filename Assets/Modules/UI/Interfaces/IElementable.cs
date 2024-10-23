using UnityEngine;

namespace UIModule.Interfaces
{
    /// <summary>
    /// Defines what a custom UI element should have
    /// </summary>
    public interface IElementable
    {
        public RectTransform Rect { get; }
    }
}