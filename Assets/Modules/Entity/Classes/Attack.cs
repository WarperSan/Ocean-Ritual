namespace EntityModule
{
    /// <summary>
    /// Class that represents an attack
    /// </summary>
    public sealed class Attack
    {
        /// <summary>
        /// Amount of damage this attack deals
        /// </summary>
        public float Damage;

        /// <summary>
        /// Type of this attack
        /// </summary>
        public AttackType Type = AttackType.NORMAL;

        /// <summary>
        /// Targets of this attack
        /// </summary>
        public ProjectileTarget TargetType = ProjectileTarget.ALL;
    }

    /// <summary>Type of the attack</summary>
    public enum AttackType
    {
        NORMAL,
        FIRE,
        ICE,
        MAGIC
    }
}