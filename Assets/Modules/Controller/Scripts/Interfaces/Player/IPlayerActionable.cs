using ControllerModule.Controllers;
using UnityEngine;

namespace ControllerModule.Interfaces.Player
{
    /// <summary>
    /// Defines a <see cref="Controller"/> that can receive input actions from the category 'Player'
    /// </summary>
    public interface IPlayerActionable : IActionable
    {
        public static InputMaster operator +(IPlayerActionable actionable, InputMaster input)
        {
            if (input == null || actionable == null)
                return input;

            // If not a controller, skip
            if (actionable is not Controller controller)
            {
                Debug.LogWarning(string.Format(
                    "Tried to subscribe the object '{0}' to event from '{1}', but they are only available for classes that inherits '{2}'.",
                    actionable.GetType().Name,
                    nameof(IPlayerActionable),
                    nameof(Controller)
                ));
                return input;
            }

            // Mouse movement
            input.OnLook += controller.OnLook;

            // Player movement
            if (controller is IMovable movable)
                input.OnMove += movable.OnMove;

            // Player click
            if (actionable is IFirable firable)
            {
                input.OnFireStart += firable.OnFireStart;
                input.OnFireEnd += firable.OnFireEnd;
            }

            // Player jump
            if (actionable is IJumpable jumpable)
                input.OnJump += jumpable.OnJump;

            return input;
        }

        public static InputMaster operator -(IPlayerActionable actionable, InputMaster input)
        {
            if (input == null || actionable == null)
                return input;

            // If not a controller, skip
            if (actionable is not Controller controller)
                return input;

            // Mouse movement
            input.OnLook -= controller.OnLook;

            // Player movement
            if (controller is IMovable movable)
                input.OnMove -= movable.OnMove;

            // Player click
            if (actionable is IFirable firable)
            {
                input.OnFireStart -= firable.OnFireStart;
                input.OnFireEnd -= firable.OnFireEnd;
            }

            // Player jump
            if (actionable is IJumpable jumpable)
                input.OnJump -= jumpable.OnJump;

            return input;
        }
    }
}