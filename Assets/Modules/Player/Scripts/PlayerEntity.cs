using UnityEngine;

namespace EntityModule
{
    public class PlayerEntity : Entity
    {
        [SerializeField]
        private Transform respawnPoint;

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Death"))
                this.transform.position = respawnPoint.position + new Vector3(0, 2, 0);
        }

        #region Entity

        /// <inheritdoc/>
        public override bool TakeDamage => false;

        #endregion
    }
}

