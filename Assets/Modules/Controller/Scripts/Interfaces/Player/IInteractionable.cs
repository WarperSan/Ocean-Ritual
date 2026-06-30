namespace ControllerModule.Interfaces.Player
{
    /// <summary>
    /// Defines the controllers that want to be notify when the player interacts
    /// </summary>
    public interface IInteractionable
    {
        /// <summary>
        /// Called when the player presses the 'Interact' button
        /// </summary>
        void OnInteract();
    }
}