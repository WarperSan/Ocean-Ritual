using MapModule;
using UnityEngine;

namespace EntityModule
{
    public class PlayerRespawnPlane : MonoBehaviour
    {
        [SerializeField]
        private Transform player;

        [SerializeField]
        private Transform respawnPoint;

        private void Update()
        {
            if (player.transform.position.y <= OceanManager.WATER_HEIGHT - 2)
            {
                player.transform.position = respawnPoint.position + new Vector3(-1, 2, 0);
            }
        }
    }
}
