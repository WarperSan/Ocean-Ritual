using ProjectilesModule.Interfaces;
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
            GameObject obj = GetProjectile(this.throwProjectile);

            if (obj == null)
                return;

            obj.transform.position = this.throwSource.position;

            if (obj.TryGetComponent(out Rigidbody rb))
                rb.velocity = GetLaunch(obj, this.throwTarget.position, 10);
        }

        public void ThrowEnded() => this.tree.ThrowEnded();

        #endregion

        #region Rage Throw

        [Header("Rage Throw")]
        [SerializeField]
        private GameObject[] rageThrowProjectiles;

        [SerializeField]
        private Transform[] rageThrowSources;

        public void ExecuteRageThrow()
        {
            // If source is invalid, skip
            if (this.rageThrowSources.Length == 0)
                return;

            // var obj = ObjectPools.ObjectPool.GetObject(this.rageThrowProjectiles[Random.Range(0, this.rageThrowProjectiles.Length)]);

            // // If projectile invalid, skip
            // if (obj == null)
            //     return;

            // obj.transform.position = this.rageThrowSources[Random.Range(0, this.rageThrowProjectiles.Length)].position;

            // if (obj.TryGetComponent(out Projectiles.Projectile projectile) && obj.TryGetComponent(out Rigidbody rb))
            // {
            //     var angle = Random.Range(0, 360) * Mathf.Deg2Rad;
            //     var distance = Random.Range(10, 50);
            //     rb.velocity = projectile.GetLaunch(this.transform.position + (new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle)) * distance), Random.Range(10, 50));
            //     var pos = Random.Range(0, 25) == 0
            //         ? this.throwTarget.position
            //         : this.transform.position + (new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle)) * distance);

            //     rb.velocity = projectile.GetLaunch(pos, Random.Range(10, 50));
            // }
        }

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

            newProjectile.GetComponent<IDespawnable>()?.ResetSelf();

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