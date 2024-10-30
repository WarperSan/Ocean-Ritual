namespace EntityModule
{
    /// <summary>
    /// Class that represents an Attaque
    /// </summary>
    public sealed class Attack
    {
        /// <summary>
        /// Amount of damage this Attaque deals
        /// </summary>
        public float Damage;

        /// <summary>
        /// Type of this Attaque
        /// </summary>
        public AttackType Type = AttackType.NORMAL;

        /// <summary>
        /// Targets of this Attaque
        /// </summary>
        public ProjectileTarget TargetType = ProjectileTarget.ALL;
    }

    /// <summary>Type of the Attaque</summary>
    public enum AttackType
    {
        NORMAL,
        FIRE,
        ICE,
        MAGIC
    }
}