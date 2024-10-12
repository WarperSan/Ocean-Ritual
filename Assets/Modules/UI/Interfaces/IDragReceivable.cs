using UnityEngine;

namespace UIModule.Interfaces
{
    /// <summary>
    /// Defines the objects that can receive another draggable object
    /// </summary>
    public interface IDragReceivable
    {
        public RectTransform Rect { get; }
    }
}