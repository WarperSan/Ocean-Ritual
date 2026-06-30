using BossesModule.Worm.Nodes;
using EntityModule;
using EntityModule.Entities;
using MapModule;
using System.Collections;
using UnityEngine;

namespace BossesModule.Worm
{
    [RequireComponent(typeof(WormTree))]
    public class WormEntity : BossEntity
    {
        public Transform ArenaOrigin => barrierParent;
        public float     ArenaRadius => barrierRange;

        #region Ice Storm

        [Header("Ice Storm")]
        [SerializeField]
        private ObjectPool iceStormPool;

        [SerializeField]
        private GameObject iceStorm_iciclePrefab;

        [SerializeField]
        private GameObject iceStorm_aoePrefab;

        public void StartIceStorm() => StartCoroutine(IceStormCoroutine());

        private IEnumerator IceStormCoroutine()
        {
            const float TIME_BETWEEN_SPAWN = 0.3f;
            const float SPAWN_HEIGHT = 340f;
            const float defaultAngle = 90f * Mathf.Deg2Rad;

            float time = IceStormNode.COOLDOWN;

            while (time > 0)
            {
                // Pick random location
                float rndRadius = Random.Range(0, ArenaRadius * 0.95f);
                Vector3 rndPos = UtilsModule.Random.RandomOnCircumference(rndRadius, ArenaOrigin.position);

                // Spawn Icicle
                GameObject icicle = iceStormPool.Get(iceStorm_iciclePrefab.name);
                float angle = 45f * Mathf.Deg2Rad;

                if (icicle.TryGetComponent(out Projectile projectile))
                {
                    projectile.ResetSelf();

                    projectile.Attribute(new Attack
                    {
                        Damage = 5,
                        Type = AttackType.ICE,
                        TargetType = ProjectileTarget.BOAT,
                    });
                }

                // Spawn AoE
                GameObject aoe = iceStormPool.Get(iceStorm_aoePrefab.name);

                // Place Objects
                icicle.transform.position = new Vector3(
                    rndPos.x,
                    Mathf.Sin(defaultAngle + angle) * SPAWN_HEIGHT,
                    rndPos.z - Mathf.Cos(defaultAngle + angle) * SPAWN_HEIGHT
                );
                icicle.transform.rotation = Quaternion.Euler(45, 0, 0);

                aoe.transform.position = new Vector3(
                    rndPos.x,
                    OceanManager.WATER_HEIGHT,
                    rndPos.z
                );

                // Activate objects
                icicle.SetActive(true);
                aoe.SetActive(true);

                if (aoe.TryGetComponent(out ParticleSystem particleSystem))
                    particleSystem.Play();

                yield return new WaitForSeconds(TIME_BETWEEN_SPAWN);

                time -= TIME_BETWEEN_SPAWN;
            }
        }

        #endregion

        #region Ice Wave

        [Header("Ice Wave")]
        [SerializeField]
        private ObjectPool iceWavePool;

        [SerializeField]
        private GameObject iceWave_iciclePrefab;

        public void StartIceWave() => StartCoroutine(IceWaveCoroutine());

        private IEnumerator IceWaveCoroutine()
        {
            const int SPAWN_QTY = 20;
            const float SPAWN_ARCH = 180f;
            const float SPAWN_RADIUS = 30f;

            float angleStart = transform.rotation.eulerAngles.y + SPAWN_ARCH / 2;
            float anglePerSpawn = SPAWN_ARCH / (SPAWN_QTY - 1);
            Vector3 spawnOrigin = transform.position;
            spawnOrigin.y = OceanManager.WATER_HEIGHT;

            for (int i = 0; i < SPAWN_QTY; i++)
            {
                GameObject icicle = iceWavePool.Get(iceWave_iciclePrefab.name);

                if (icicle.TryGetComponent(out Projectile projectile))
                {
                    projectile.ResetSelf();

                    projectile.Attribute(new Attack
                    {
                        Damage = 10,
                        Type = AttackType.ICE,
                        TargetType = ProjectileTarget.BOAT,
                    });
                }

                float angle = angleStart - i * anglePerSpawn;

                Vector3 spawnPosition = new Vector3(
                    Mathf.Sin(angle * Mathf.Deg2Rad) * SPAWN_RADIUS,
                    0,
                    Mathf.Cos(angle * Mathf.Deg2Rad) * SPAWN_RADIUS
                ) + spawnOrigin;

                icicle.transform.position = spawnPosition;

                Vector3 direction = (spawnOrigin - spawnPosition).normalized;
                icicle.transform.rotation = Quaternion.LookRotation(direction, Vector3.up);

                icicle.SetActive(true);

                yield return null;
                yield return null;
                yield return null;
            }
        }

        #endregion

        #region BossEntity

        /// <inheritdoc/>
        protected override void OnDeath(float overDamage)
        {
            StopAllCoroutines();

            iceStormPool.DisableAll(iceStorm_iciclePrefab.name);
            iceWavePool.DisableAll(iceWave_iciclePrefab.name);

            base.OnDeath(overDamage);
        }

        #endregion
    }
}