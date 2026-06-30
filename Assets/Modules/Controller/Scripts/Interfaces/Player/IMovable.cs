using UnityEngine;

namespace ControllerModule.Interfaces.Player
{
    /// <summary>
    /// Defines the controllers that want to be notify when the player moves
    /// </summary>
    public interface IMovable
    {
        /// <summary>
        /// Called when the player moves
        /// </summary>
        void OnMove(Vector2 direction);
    }
}