using BossesModule.Golem;
using EntityModule;
using EntityModule.Entities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BossesModule.Worm
{
    [RequireComponent(typeof(WormTree))]
    public class WormEntity : EntityBehaviour
    {
        #region EntityBehavior

        protected override void OnStart()
        {
            base.OnStart();

            this.healthBar.maxValue = this.MaxHeath;
            this.healthBar.value = this.Health;
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

    }
}

