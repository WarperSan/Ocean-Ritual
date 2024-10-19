namespace ControllerModule.Controllers.Interfaces
{
    /// <summary>
    /// Defines the objects that want to be notify when the player jumps
    /// </summary>
    public interface IJumpable
    {
        /// <summary>
        /// Called when the player jumps
        /// </summary>
        public void OnJump();
    }
}