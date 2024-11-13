using UnityEngine;

namespace EntityModule
{
    public class PlayerEntity : Entity
    {
        [SerializeField]
        private Transform respawnPoint;
        [SerializeField] string music;
        [SerializeField] string wavesaAmbience;

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Death"))
                this.transform.position = respawnPoint.position + new Vector3(0, 2, 0);

            
        }
        private void Start()
        {
            Debug.Log(SoundManager.Instance.name);
            //SoundManager.Instance.PlaySound(music,SoundType.Music,true);
            SoundManager.Instance.PlaySound(wavesaAmbience, SoundType.Ambient, true);
        }
        
        #region Entity

        /// <inheritdoc/>
        public override bool TakeDamage => false;

        #endregion
    }
}

