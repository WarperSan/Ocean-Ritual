using ExtensionsModule;
using System.Collections;
using UIModule.Components;
using UnityEngine;

namespace EntityModule.Entities
{
    public abstract class BossEntity : EntityBehaviour
    {
        #region Animations

        [Header("Animations")]
        [SerializeField]
        private Animator animator;

        #endregion

        #region Health Bar

        [Header("Health Bar")]
        [SerializeField] private HealthBar healthBar;

        private void UpdateHealthBar() => this.healthBar.UpdateBar(this);

        private void ShowHealthBar() => this.healthBar.Show();
        private void HideHealthBar() => this.healthBar.Hide();

        #endregion

        #region Arena

        [Header("Arena")]
        [SerializeField, Min(0)]
        protected float detectionRange = 100;

        [SerializeField, Min(0)]
        protected float barrierRange = 125;

        [SerializeField, Min(1)]
        protected int barrierCount = 20;

        [SerializeField]
        protected GameObject barrierPrefab;

        protected Transform barrierParent;

        private IEnumerator SpawnArena()
        {
            barrierParent = new GameObject()
            {
                name = this.name + " BARRIER",
            }.transform;
            barrierParent.transform.position = this.transform.position;

            int centerIndex = this.barrierCount / 2;

            for (int offset = 0; offset <= centerIndex; offset++)
            {
                if (centerIndex + offset < this.barrierCount)
                {
                    SpawnBarrierPiece(centerIndex + offset);
                    yield return new WaitForSeconds(1.5f / this.barrierCount);
                }

                if (centerIndex - offset >= 0 && offset != 0) 
                {
                    SpawnBarrierPiece(centerIndex - offset);
                    yield return new WaitForSeconds(1.5f / this.barrierCount);
                }
            }

            this.hasSpawned = true;
        }

        private void SpawnBarrierPiece(int index)
        {
            GameObject piece = Instantiate(this.barrierPrefab, barrierParent);
            piece.transform.localPosition = new Vector3(
                Mathf.Cos(Mathf.Deg2Rad * 360f * index / this.barrierCount),
                0,
                Mathf.Sin(Mathf.Deg2Rad * 360f * index / this.barrierCount)
            ) * this.barrierRange;
            piece.transform.localRotation = Quaternion.Euler(0, UnityEngine.Random.Range(0, 180), 0);
        }

        private IEnumerator DespawnArena()
        {
            for (int i = barrierParent.childCount - 1; i >= 0; i--)
            {
                if (barrierParent.GetChild(i).TryGetComponent(out Animator animator))
                    animator.SetTrigger("hide");
                yield return new WaitForSeconds(3f / this.barrierCount);
            }
            yield return new WaitForSeconds(5f);
            Destroy(barrierParent.gameObject);
            barrierParent = null;
        }

        #endregion

        #region EntityBehavior

        private bool hasStarted = false;
        private bool hasSpawned = false;
        [SerializeField] AudioClip bossMusic;
        private void Update()
        {
            // If spawned, update tree
            if (this.hasSpawned)
            {
                this.UpdateTree();
                return;
            }

            Transform target = TargetGeneral.Instance.BoatTarget;

            // If target not found, skip
            if (target == null)
                return;

            if (this.transform.Distance(target) > this.detectionRange)
                return;

            if (!this.hasStarted)
            {
                this.hasStarted = true;
                TargetGeneral.Instance.Target = target;
                SoundManager.Instance.PlaySound(bossMusic,SoundType.Music,0.8f,true);
                this.animator.SetBool("isSpawning", true);
                this.ShowHealthBar();

                this.StartCoroutine(this.SpawnArena());

                return;
            }
        }

        /// <inheritdoc/>
        protected override void OnStart()
        {
            base.OnStart();
            healthBar.InitializeBar(this);
        }

        /// <inheritdoc/>
        protected override void OnPostAttack(Projectile source)
        {
            animator.SetTrigger("isHit");
            UpdateHealthBar();
        }

        /// <inheritdoc/>
        protected override void OnDeath(float overDamage)
        {
            animator.SetTrigger("isDead");
            enabled = false;
            HideHealthBar();
            StartCoroutine(this.DespawnArena());
            SoundManager.Instance.StopSound(SoundType.Music);
        }

        #endregion
    }
}

