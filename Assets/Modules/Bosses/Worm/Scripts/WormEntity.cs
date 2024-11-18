using BossesModule.Worm.Nodes;
using EntityModule;
using EntityModule.Entities;
using MapModule;
using System.Collections;
using TMPro.EditorUtilities;
using UnityEngine;

namespace BossesModule.Worm
{
    [RequireComponent(typeof(WormTree))]
    public class WormEntity : BossEntity
    {
        public Transform ArenaOrigin => this.barrierParent;
        public float ArenaRadius => this.barrierRange;

        #region Ice Storm

        [Header("Ice Storm")]
        [SerializeField]
        private ObjectPool iceStormPool;

        [SerializeField]
        private GameObject iceStorm_iciclePrefab;

        [SerializeField]
        private GameObject iceStorm_aoePrefab;

        public void StartIceStorm() => this.StartCoroutine(this.IceStormCoroutine());

        private IEnumerator IceStormCoroutine()
        {
            const float TIME_BETWEEN_SPAWN = 0.3f;
            const float SPAWN_HEIGHT = 340f;
            const float defaultAngle = 90f * Mathf.Deg2Rad;

            float time = IceStormNode.COOLDOWN;

            while (time > 0)
            {
                // Pick random location
                float rndRadius = Random.Range(0, this.ArenaRadius * 0.95f);
                Vector3 rndPos = UtilsModule.Random.RandomOnCircumference(rndRadius, this.ArenaOrigin.position);

                // Spawn Icicle
                GameObject icicle = this.iceStormPool.Get(this.iceStorm_iciclePrefab.name);
                float angle = 45f * Mathf.Deg2Rad;


                if (icicle.TryGetComponent(out Projectile projectile))
                {
                    projectile.ResetSelf();
                    projectile.Attribute(new Attack()
                    {
                        Damage = 1,
                        Type = AttackType.ICE,
                        TargetType = ProjectileTarget.BOAT
                    });
                }

                // Spawn AoE
                GameObject aoe = this.iceStormPool.Get(this.iceStorm_aoePrefab.name);

                // Place Objects
                icicle.transform.position = new Vector3(
                    rndPos.x,
                    Mathf.Sin(defaultAngle + angle) * SPAWN_HEIGHT,
                    rndPos.z - (Mathf.Cos(defaultAngle + angle) * SPAWN_HEIGHT)
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

        Coroutine iceWaveCoroutine;

        public void StartIceWave() => iceWaveCoroutine = this.StartCoroutine(this.IceWaveCoroutine());
        public void EndIceWave() => this.StopCoroutine(iceWaveCoroutine);

        private IEnumerator IceWaveCoroutine()
        {
            const int SPAWN_QTY = 20;
            const float SPAWN_ARCH = 180f;
            const float SPAWN_RADIUS = 10f;

            float time = IceWaveNode.COOLDOWN;
            float timeBetweenSpawn = time / SPAWN_QTY;

            while (time > 0)
            {
                // Spawn Icicle
                GameObject icicle = this.iceWavePool.Get(this.iceWave_iciclePrefab.name);
                float angle = 45f * Mathf.Deg2Rad;

                if (icicle.TryGetComponent(out Projectile projectile))
                {
                    projectile.ResetSelf();
                    projectile.Attribute(new Attack()
                    {
                        Damage = 1,
                        Type = AttackType.ICE,
                        TargetType = ProjectileTarget.BOAT
                    });
                }

                // Place Objects
                icicle.transform.position = new Vector3(
                    Mathf.Cos(angle) * SPAWN_RADIUS + this.transform.position.x,
                    OceanManager.WATER_HEIGHT,
                    Mathf.Sin(angle) * SPAWN_RADIUS + this.transform.position.z
                );

                // Activate objects
                icicle.SetActive(true);

                yield return new WaitForSeconds(timeBetweenSpawn);

                time -= timeBetweenSpawn;
            }
        }

        #endregion
    }
}

