using UnityEngine;

namespace ControllerModule.Controllers.Interfaces
{
    /// <summary>
    /// Defines the controllers that want to be notify when the player moves
    /// </summary>
    public interface IMovable
    {
        /// <summary>
        /// Called when the player moves
        /// </summary>
        public void OnMove(Vector2 direction);
    }
}