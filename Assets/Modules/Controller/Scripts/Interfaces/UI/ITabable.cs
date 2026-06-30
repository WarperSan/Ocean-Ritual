namespace ControllerModule.Interfaces.UI
{
    /// <summary>
    /// Defines the components that want to be notify when the player presses TAB
    /// </summary>
    public interface ITabable
    {
        /// <summary>
        /// Called when the player asks to go to the next tab
        /// </summary>
        void OnTabNext();

        /// <summary>
        /// Called when the player asks to go to the previous tag
        /// </summary>
        void OnTabPrevious();
    }
}