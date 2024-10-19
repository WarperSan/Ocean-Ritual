namespace ControllerModule.Controllers.Interfaces
{
    /// <summary>
    /// Defines the objects that want to be notify when the player fires
    /// </summary>
    public interface IFirable
    {
        /// <summary>
        /// Called when the player presses the 'Fire' button
        /// </summary>
        public void OnFireStart();

        /// <summary>
        /// Called when the player releases the 'Fire' button
        /// </summary>
        public void OnFireEnd();
    }
}