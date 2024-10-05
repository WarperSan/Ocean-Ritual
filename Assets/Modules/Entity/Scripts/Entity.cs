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
        [SerializeField, Tooltip("Maximum health for this entity")]
        protected float MaxHeath;

        protected float Health { get; private set; }

        /// <summary>
        /// Determines if this entity can take damage
        /// </summary>
        public virtual bool TakeDamage => true;

        /// <summary>
        /// Resets the health of this entity
        /// </summary>
        private void ResetHealth()
        {
            this.Health = this.MaxHeath;
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
            if (!this.TakeDamage)
                return;

            this.ModifyHeal(heal);
            this.Health = Mathf.Clamp(this.Health + heal.Amount, 0, this.MaxHeath);
        }

        /// <summary>
        /// Kills this entity
        /// </summary>
        public void Death()
        {
            float overDamage = 0;

            if (this.Health < 0)
                overDamage = Mathf.Abs(this.Health);

            this.OnDeath(overDamage);
        }

        /// <summary>
        /// Called when this entity dies
        /// </summary>
        /// <param name="overDamage">Extra damage dealt</param>
        protected virtual void OnDeath(float overDamage) => Destroy(this.gameObject);

        #endregion

        #region Receive Attack

        /// <summary>
        /// Damages this entity with the given attack
        /// </summary>
        public void UseAttack(Attack attack, Projectile source)
        {
            this.OnPreAttack(source);

            // If immuned to damage, skip
            if (!this.TakeDamage)
                return;

            this.ModifyAttack(attack);

            this.Health -= attack.Damage;

            // If not dead, skip
            if (this.Health > 0)
            {
                this.OnPostAttack(source);
                return;
            }

            // Cause death
            this.Death();
        }

        /// <summary>
        /// Modifies the amount of damage this entity will take from the given attack
        /// </summary>
        protected virtual void ModifyAttack(Attack attack) { }

        /// <summary>
        /// Called before this entity receives an attack
        /// </summary>
        protected virtual void OnPreAttack(Projectile source) { }

        /// <summary>
        /// Called after this entity receives an non-fatal attack
        /// </summary>
        protected virtual void OnPostAttack(Projectile source) { }

        #endregion

        #region MonoBehaviour

        /// <inheritdoc/>
        private void Start()
        {
            this.ResetHealth();
            this.OnStart();
        }

        /// <summary>
        /// Called when this entity starts
        /// </summary>
        protected virtual void OnStart() { }

        #endregion
    }
}