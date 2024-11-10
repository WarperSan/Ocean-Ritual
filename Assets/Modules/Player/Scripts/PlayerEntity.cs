using UnityEngine;

namespace EntityModule
{
    public class PlayerEntity : Entity
    {
        [SerializeField]
        private Transform respawnPoint;
        [SerializeField] string music;
        

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Death"))
                this.transform.position = respawnPoint.position + new Vector3(0, 2, 0);

            
        }
        private void Start()
        {
            Debug.Log(SoundManager.Instance.name);
            SoundManager.Instance.PlaySound(music,SoundType.Music,true);
        }
        
        #region Entity

        /// <inheritdoc/>
        protected override void OnPostAttack(Projectile source)
        {
            Vector3 direction = this.transform.position - source.transform.position;
            direction.y = 0;
            direction.Normalize();
            direction *= 500;
            direction.y = 300;

            //knockback?
            //this.GetComponent<Rigidbody>().AddForce(direction);
        }

        /// <inheritdoc/>
        protected override void OnDeath(float overDamage)
        {
            //tue le joueur

            //soit afficher un menu game over
            //soit faire respawn le joueur direct apr�s un certain temps
        }

        #endregion
    }
}

