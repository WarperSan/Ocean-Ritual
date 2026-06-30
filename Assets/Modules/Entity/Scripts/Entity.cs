using UnityEngine;

namespace EntityModule
{
    /// <summary>
    /// Class that represents an entity
    /// </summary>
    public abstract class Entity : MonoBehaviour
    {
        #region Health

        [Header("Health")]
        [SerializeField]
        [Tooltip("Maximum health for this entity")]
        protected float _MaxHeath;

        public float MaxHealth => _MaxHeath;

        [field: SerializeField]
        [Tooltip("Maximum health to heal for this entity")]
        protected float HealthTotal { get; private set; }

        public float Health { get; private set; }

        /// <summary>
        /// Is this entity dead?
        /// </summary>
        public bool IsDead { get; private set; }

        /// <summary>
        /// Determines if this entity can take damage
        /// </summary>
        public virtual bool TakeDamage => true;

        /// <summary>
        /// Resets the health of this entity
        /// </summary>
        protected void ResetHealth()
        {
            IsDead = false;
            Health = _MaxHeath;
        }

        /// <summary>
        /// Modifies the amount of health this entity will recover from the given heal
        /// </summary>
        protected virtual void ModifyHeal(Heal heal) { }

        /// <summary>
        /// Heals this entity with the given heal
        /// </summary>
        /// <param name="heal"></param>
        public void UseHeal(Heal heal)
        {
            // If immuned to damage, skip
            if (!TakeDamage)
                return;

            ModifyHeal(heal);
            Health = Mathf.Clamp(Health + heal.Amount, 0, _MaxHeath);
        }

        /// <summary>
        /// Kills this entity
        /// </summary>
        public void Death()
        {
            float overDamage = 0;

            if (Health < 0)
                overDamage = Mathf.Abs(Health);

            OnDeath(overDamage);
            IsDead = true;
        }

        /// <summary>
        /// Called when this entity dies
        /// </summary>
        /// <param name="overDamage">Extra damage dealt</param>
        protected virtual void OnDeath(float overDamage) => Destroy(gameObject);

        #endregion

        #region Receive Attack

        /// <summary>
        /// Damages this entity with the given Attaque
        /// </summary>
        public void UseAttack(Attack attack, Projectile source)
        {
            OnPreAttack(source);

            // If immuned to damage, skip
            if (!TakeDamage)
                return;

            ModifyAttack(attack);

            Health -= attack.Damage;

            // If not dead, skip
            if (Health > 0)
            {
                OnPostAttack(source);
                return;
            }

            // Cause death
            Death();
        }

        /// <summary>
        /// Modifies the amount of damage this entity will take from the given Attaque
        /// </summary>
        protected virtual void ModifyAttack(Attack attack) { }

        /// <summary>
        /// Called before this entity receives an Attaque
        /// </summary>
        protected virtual void OnPreAttack(Projectile source) { }

        /// <summary>
        /// Called after this entity receives an non-fatal Attaque
        /// </summary>
        protected virtual void OnPostAttack(Projectile source = null) { }

        #endregion

        #region MonoBehaviour

        /// <inheritdoc/>
        private void Start()
        {
            ResetHealth();
            OnStart();
        }

        /// <summary>
        /// Called when this entity starts
        /// </summary>
        protected virtual void OnStart() { }

        #endregion
    }
}