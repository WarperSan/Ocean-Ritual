using EntityModule;
using UnityEngine;

namespace BossesModule.Golem
{
    public class GolemAnimationEvents : MonoBehaviour
    {
        public GolemTree tree;

        #region Throw

        [Header("Throw")]
        [SerializeField, Tooltip("Prefab to throw")]
        private GameObject throwProjectile;

        [SerializeField, Tooltip("Initial position of the projectile")]
        private Transform throwSource;

        [SerializeField, Tooltip("Force applied to the projectile upon launch")]
        private Vector3 throwForce;

        public Transform throwTarget;

        public void ExecuteThrow()
        {
            // If source is invalid, skip
            if (this.throwSource == null)
                return;

            // If target is invalid, skip
            if (this.throwTarget == null)
                return;

            // var obj = ObjectPools.ObjectPool.GetObject(this.throwProjectile);
            GameObject obj = this.tree.throwPool.Get(this.throwProjectile.name);

            if (obj == null)
                return;

            obj.SetActive(true);
            obj.transform.position = this.throwSource.position;

            if (obj.TryGetComponent(out Rigidbody rb))
                rb.velocity = GetLaunch(obj, this.throwTarget.position, 10);

            if (obj.TryGetComponent(out Projectile projectile))
            {
                projectile.ResetSelf();
                projectile.Attribute(new Attack()
                {
                    Damage = 20,
                    Type = AttackType.FIRE,
                    TargetType = ProjectileTarget.ALL
                });
            }
        }

        public void ThrowEnded() => this.tree.ThrowEnded();

        #endregion

        #region Spawn

        public void SetSpawning()
        {
            if (!this.TryGetComponent(out Animator animator))
                return;

            animator.SetBool("isSpawning", false);
        }

        #endregion

        #region Projectiles

        private static GameObject GetProjectile(GameObject prefab)
        {
            // If prefab invalid
            if (prefab == null)
                return null;

            // OBJECT POOL MAGIC
            GameObject newProjectile = Instantiate(prefab);
            newProjectile.SetActive(true);

            if (newProjectile.TryGetComponent(out LavaProjectile lavaProjectile))
            {
                lavaProjectile.ResetSelf();
                lavaProjectile.Attribute(new EntityModule.Attack()
                {
                    Damage = 10,
                    Type = EntityModule.AttackType.FIRE,
                    TargetType = EntityModule.ProjectileTarget.PLAYER
                });
            }

            return newProjectile;
        }

        private static Vector3 GetLaunch(GameObject projectile, Vector3 target, float height, float? gravity = null)
        {
            // Get base gravity
            gravity ??= Physics.gravity.y;

            Vector3 displacement = target - projectile.transform.position;

            if (displacement.y > height)
                displacement.y = height;

            float time = Mathf.Sqrt(-2 * height / gravity.Value) + Mathf.Sqrt(2 * (displacement.y - height) / gravity.Value);

            return new Vector3(
                displacement.x / time,
                Mathf.Sqrt(-2 * gravity.Value * height) * -Mathf.Sign(gravity.Value),
                displacement.z / time
            );
        }

        #endregion
    }
}