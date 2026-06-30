namespace ControllerModule.Interfaces.Player
{
    /// <summary>
    /// Defines the controllers that want to be notify when the player jumps
    /// </summary>
    public interface IJumpable
    {
        /// <summary>
        /// Called when the player jumps
        /// </summary>
        void OnJump();
    }
}