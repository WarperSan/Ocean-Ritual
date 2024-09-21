namespace EntityModule
{
    /// <summary>
    /// Class that represents a heal
    /// </summary>
    public sealed class Heal
    {
        public float Amount;
        public HealType Type = HealType.ENVIRONMENT;
    }

    /// <summary>Type of the heal</summary>
    public enum HealType
    {
        FRIEND,
        FOE,
        ENVIRONMENT
    }
}