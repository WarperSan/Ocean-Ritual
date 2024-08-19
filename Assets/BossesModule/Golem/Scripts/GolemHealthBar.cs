using ProjectilesModule;
using ProjectilesModule.Interfaces;
using UnityEngine;
using UnityEngine.UI;

namespace BossesModule.Golem
{
    public class GolemHealthBar : MonoBehaviour, ICollidable
    {
        [SerializeField]
        private Slider healthBar;

        [SerializeField]
        private int maxHealth = 100;
        private int health;

        /// <inheritdoc/>
        private void Start()
        {
            this.health = this.maxHealth;
            this.healthBar.maxValue = this.maxHealth;
            this.healthBar.value = this.health;
        }

        private void TakeDamage(int damage)
        {
            this.health -= damage;
            this.healthBar.value = this.health;

            if (this.health <= 0)
            {
                Debug.Log("GOLEM DIED"); // <---- ICI ANTHONY!!!!!!!!!!!!!!!!!!!!!!!!!!!!
            }
        }

        #region ICollidable

        /// <inheritdoc/>
        public void OnCollision(Projectile source)
        {
            this.TakeDamage(5);

            source.Despawn();
        }

        #endregion
    }
}