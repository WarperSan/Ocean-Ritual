using System.Collections;
using System.Collections.Generic;
using UIModule.Components;
using UnityEngine;

namespace EntityModule.Entities
{
    public abstract class BossEntity : EntityBehaviour
    {

        #region Health Bar

        [Header("Health Bar")]
        [SerializeField] private HealthBar healthBar;

        private void UpdateHealthBar() => this.healthBar.UpdateBar(this);

        private void ShowHealthBar() => this.healthBar.Show();
        private void HideHealthBar() => this.healthBar.Hide();

        #endregion

        #region EntityBehavior

        private void Update()
        {
            // si combat commencer, updatetree
            this.UpdateTree();
            // sinon spawn arene, show healthbar
        }

        protected override void OnStart()
        {
            base.OnStart();
            healthBar.InitializeBar(this);
            
        }

        protected override void OnPostAttack(Projectile source) => UpdateHealthBar();

        #endregion

        
    }
}

