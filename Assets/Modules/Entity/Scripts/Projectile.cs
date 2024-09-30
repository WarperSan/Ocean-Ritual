using EntityModule.Conditions;
using System;
using UnityEngine;

namespace EntityModule
{
    [Flags]
    public enum ProjectileTarget
    {
        NONE = 0, // No entity
        ALL = -~0, // Every entity

        PLAYER = 1 << 0, // Only player

        ENEMY = 1 << 1, // Only enemies
        BOSS = 1 << 2, // Only bosses
        OPPONENTS = ENEMY | BOSS // Enemies and Bosses
    }

    /// <summary>
    /// Class that represents a projectile
    /// </summary>
    public abstract class Projectile : Entity
    {
        #region Attack

        private Attack attack = null;

        /// <summary>
        /// Attributes an attack to this projectile
        /// </summary>
        public void Attribute(Attack attack)
        {
            this.attack = attack;
            this.OnAttributed(this.attack);
        }

        /// <summary>
        /// Called when this projectile gets attributed an attack
        /// </summary>
        protected virtual void OnAttributed(Attack attack) { }

        #endregion

        #region Collision

        [Header("Collision")]
        [SerializeField]
        private Collider _collider;

        public static int BOSS_LAYER = -1;
        public static int ENEMY_LAYER = -1;
        public static int PLAYER_LAYER = -1;

        /// <inheritdoc/>
        private void OnTriggerEnter(Collider other)
        {
            // If hit non-entity, skip
            if (!other.TryGetComponent(out Entity entity))
                return;

            // If entity not targettable, skip
            if (!this.IsEntityTarget(entity))
                return;

            this.HitEntity(entity);
        }

        /// <summary>
        /// Checks if the given entity is targettable by this projectile
        /// </summary>
        protected virtual bool IsEntityTarget(Entity entity)
        {
            int layer = entity.gameObject.layer;

            // If targeting none
            if (this.attack.TargetType == ProjectileTarget.NONE)
                return false;

            // If targeting anyone
            if (this.attack.TargetType == ProjectileTarget.ALL)
                return true;

            // If hit a player, but not targeting player
            if (!this.attack.TargetType.HasFlag(ProjectileTarget.PLAYER) && layer == PLAYER_LAYER)
                return false;

            // If hit an enemy, but not targeting enemies
            if (!this.attack.TargetType.HasFlag(ProjectileTarget.ENEMY) && layer == ENEMY_LAYER)
                return false;

            // If hit a boss, but not targeting bosses
            if (!this.attack.TargetType.HasFlag(ProjectileTarget.BOSS) && layer == BOSS_LAYER)
                return false;

            // Layer matches the target type
            return true;
        }

        private void HitEntity(Entity entity)
        {
            // If attack invalid, skip
            if (this.attack == null)
            {
                Debug.LogWarning($"No attack was attributed when '{this.name}' hit the entity '{entity.name}'.");
                return;
            }

            this.OnPreApply(entity, this.attack);
            entity.UseAttack(this.attack);
            this.OnPostApply(entity, this.attack);
        }

        /// <summary>
        /// Called before this projectile applies its attack to the entity
        /// </summary>
        protected virtual void OnPreApply(Entity entity, Attack attack) { }

        /// <summary>
        /// Called after this projectile applied its attack to the entity
        /// </summary>
        protected virtual void OnPostApply(Entity entity, Attack attack) { }

        #endregion

        #region Movement

        /// <summary>
        /// Called to move this projectile
        /// </summary>
        protected virtual void OnMove(float elapsed) { }

        #endregion

        #region Conditions

        [Header("Conditions")]
        [SerializeField, Tooltip("Determines if all the conditions must be met in order to keep this projectile alive")]
        private bool mustMeetAllConditions = true;

        private ProjectileCondition[] conditions;

        /// <summary>
        /// Evaluates all the conditions for this projectile
        /// </summary>
        /// <param name="elapsed">Time since last call</param>
        /// <returns>Should the projectile stay alive?</returns>
        private bool EvaluateConditions(float elapsed)
        {
            foreach (ProjectileCondition item in conditions)
            {
                bool result = item.UpdateCondition(elapsed);

                if (!result && this.mustMeetAllConditions)
                    return false;
            }

            return true;
        }

        #endregion

        #region Reset

        /// <summary>
        /// Resets this projectile
        /// </summary>
        public void ResetSelf()
        {
            // Clear values
            this.attack = null;

            foreach (ProjectileCondition item in this.conditions)
                item.ResetCondition();

            this.OnReset();
        }

        /// <summary>
        /// Called when this projectile gets reset
        /// </summary>
        protected virtual void OnReset() { }

        #endregion

        #region Entity

        /// <inheritdoc/>
        public override bool TakeDamage => false;

        /// <inheritdoc/>
        protected override void OnDeath(float overDamage) => this.gameObject.SetActive(false);

        #endregion

        #region MonoBehaviour

        /// <inheritdoc/>
        private void Awake()
        {
#if UNITY_EDITOR
            if (!_collider.isTrigger)
            {
                _collider.isTrigger = true;
                Debug.LogWarning($"The projectile '{this.name}' has a collider that is not trigger. Please fix the collider.");
            }
#endif

            if (BOSS_LAYER == -1)
                BOSS_LAYER = LayerMask.NameToLayer("Boss");

            if (ENEMY_LAYER == -1)
                ENEMY_LAYER = LayerMask.NameToLayer("Enemy");

            if (PLAYER_LAYER == -1)
                PLAYER_LAYER = LayerMask.NameToLayer("Player");

            conditions = this.GetComponents<ProjectileCondition>();
        }

        /// <inheritdoc/>
        private void Update()
        {
            this.OnMove(Time.deltaTime);
            this.OnUpdate(Time.deltaTime);

            // Kill projectile if necessary
            if (!this.EvaluateConditions(Time.deltaTime))
                this.Death();
        }

        /// <summary>
        /// Called when this projectile gets updated
        /// </summary>
        protected virtual void OnUpdate(float elapsed) { }

        #endregion
    }
}