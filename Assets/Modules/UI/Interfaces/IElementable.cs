using ControllerModule.Interfaces.UI;
using UnityEngine;

namespace UIModule.Interfaces
{
    /// <summary>
    /// Defines what a custom UI element should have
    /// </summary>
    public interface IElementable : IUIActionable
    {
        RectTransform Rect { get; }
    }
}