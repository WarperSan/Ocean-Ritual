namespace EntityModule
{
    /// <summary>
    /// Class that represents an attack
    /// </summary>
    public sealed class Attack
    {
        public float Damage;
        public AttackType Type = AttackType.NORMAL;
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