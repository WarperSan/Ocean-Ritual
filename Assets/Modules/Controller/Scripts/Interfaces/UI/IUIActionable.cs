using ControllerModule.Controllers;
using UIModule;
using UnityEngine;

namespace ControllerModule.Interfaces.UI
{
    /// <summary>
    /// Defines a <see cref="UIComponent"/> that can receive input actions from the category 'UI'
    /// </summary>
    public interface IUIActionable : IActionable
    {
        public static InputMaster operator +(IUIActionable actionable, InputMaster input)
        {
            if (input == null || actionable == null)
                return input;

            // If not a UI Component, skip
            if (actionable is not UIComponent component)
            {
                Debug.LogWarning(string.Format(
                    "Tried to subscribe the object '{0}' to event from '{1}', but they are only available for classes that inherits '{2}'.",
                    actionable.GetType().Name,
                    nameof(IUIActionable),
                    nameof(UIComponent)
                ));
                return input;
            }

            // Tab movement
            if (component is ITabable tabable)
            {
                input.OnTabNext += tabable.OnTabNext;
                input.OnTabPrevious += tabable.OnTabPrevious;
            }

            return input;
        }

        public static InputMaster operator -(IUIActionable actionable, InputMaster input)
        {
            if (input == null || actionable == null)
                return input;

            // If not a UI Component, skip
            if (actionable is not UIComponent component)
                return input;

            // Tab movement
            if (component is ITabable tabable)
            {
                input.OnTabNext -= tabable.OnTabNext;
                input.OnTabPrevious -= tabable.OnTabPrevious;
            }

            return input;
        }
    }
}