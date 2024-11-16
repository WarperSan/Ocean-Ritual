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
            float time = IceStormNode.COOLDOWN;

            while (time > 0)
            {
                // Pick random location
                float rndRadius = Random.Range(0, this.ArenaRadius * 0.95f);
                Vector3 rndPos = UtilsModule.Random.RandomOnCircumference(rndRadius, this.ArenaOrigin.position);

                // Spawn Icicle
                GameObject icicle = this.iceStormPool.Get(this.iceStorm_iciclePrefab.name);

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
                    340,
                    rndPos.z
                );

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
    }
}

