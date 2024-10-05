using EntityModule;
using EntityModule.Entities;
using UnityEngine;
using UnityEngine.UI;

namespace BossesModule.Golem
{
    [RequireComponent(typeof(GolemTree))]
    public class GolemEntity : EntityBehaviour
    {
        #region EntityBehaviour

        protected override void OnStart()
        {
            base.OnStart();

            this.healthBar.maxValue = this.MaxHeath;
            this.healthBar.value = this.Health;
        }

        /// <inheritdoc/>
        private void Update()
        {
            this.UpdateTree();
        }

        #endregion

        #region Health Bar

        [Header("Health Bar")]
        [SerializeField]
        private Slider healthBar;

        /// <inheritdoc/>
        protected override void OnPostAttack(Projectile source) 
        {
            this.healthBar.value = this.Health;
        }

        #endregion

        #region Quest

        /// <inheritdoc/>
        protected override void OnDeath(float overDamage)
        {
            this.healthBar.value = 0;
            QuestManager.SomeoneDeath(this.name);
        }

        #endregion
    }
}