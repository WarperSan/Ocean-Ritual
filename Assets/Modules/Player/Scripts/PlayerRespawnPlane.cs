using UnityEngine;

namespace EntityModule
{
    public class PlayerRespawnPlane : MonoBehaviour
    {
        [SerializeField]
        private Transform respawnPoint;

        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Player"))
                return;

            other.transform.position = respawnPoint.position + new Vector3(-1, 2, 0);
        }
    }
}