namespace ControllerModule.Interfaces.UI
{
    /// <summary>
    /// Defines the components that want to be notify when the player presses ESC
    /// </summary>
    public interface IEscapable
    {
        /// <summary>
        /// Called when the player requests to close the UI
        /// </summary>
        void OnEscape();
    }
}