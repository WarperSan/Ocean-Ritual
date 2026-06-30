namespace ControllerModule.Interfaces.Player
{
    /// <summary>
    /// Defines the controllers that want to be notify when the player fires
    /// </summary>
    public interface IFirable
    {
        /// <summary>
        /// Called when the player presses the 'Fire' button
        /// </summary>
        void OnFireStart();

        /// <summary>
        /// Called when the player releases the 'Fire' button
        /// </summary>
        void OnFireEnd();
    }
}