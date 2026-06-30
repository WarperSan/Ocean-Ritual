namespace EntityModule
{
    /// <summary>
    /// Class that represents a heal
    /// </summary>
    public sealed class Heal
    {
        /// <summary>
        /// Amount of health this heal will recover
        /// </summary>
        public float Amount;

        /// <summary>
        /// Type of the heal
        /// </summary>
        public HealType Type = HealType.ENVIRONMENT;
    }

    /// <summary>Type of the heal</summary>
    public enum HealType
    {
        FRIEND,
        FOE,
        ENVIRONMENT,
    }
}