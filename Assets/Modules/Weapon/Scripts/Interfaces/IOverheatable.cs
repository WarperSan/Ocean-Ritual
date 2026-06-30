namespace WeaponModule.Interfaces
{
    /// <summary>
    /// Defines a weapon that can overheat
    /// </summary>
    public interface IOverheatable
    {
        /// <summary>
        /// Called when this weapon overheats
        /// </summary>
        void OnOverheat();

        /// <summary>
        /// Updates this weapon's overheat indicator
        /// </summary>
        void UpdateOverheatIndicator();

        /// <summary>
        /// Determines if this weapon is overheated
        /// </summary>
        bool IsOverheated();
    }
}