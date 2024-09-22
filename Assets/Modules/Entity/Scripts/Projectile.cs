using UnityEngine;

namespace EntityModule
{
    public class Projectile : MonoBehaviour
    {
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
        }

        #region Attack

        private Attack attack;

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

        /// <inheritdoc/>
        private void OnTriggerEnter(Collider other)
        {
            // If hit non-entity, skip
            if (!other.TryGetComponent(out Entity entity))
                return;

            this.HitEntity(entity);
        }

        private void HitEntity(Entity entity)
        {
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
    }
}