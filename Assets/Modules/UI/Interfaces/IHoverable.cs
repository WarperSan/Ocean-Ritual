namespace UIModule.Interfaces
{
    /// <summary>
    /// Defines an object that can show data in a hover
    /// </summary>
    public interface IHoverable
    {
        /// <summary>
        /// Fetches the data to show in the hover
        /// </summary>
        ItemData GetData();
    }
}