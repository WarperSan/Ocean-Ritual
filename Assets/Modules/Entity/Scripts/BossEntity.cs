using ExtensionsModule;
using System;
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
        private float detectionRange = 100;

        [SerializeField, Min(0)]
        private float barrierRange = 125;

        [SerializeField, Min(1)]
        private int barrierCount = 20;

        [SerializeField]
        private GameObject barrierPrefab;

        private Transform barrierParent;

        private IEnumerator SpawnArena()
        {
            barrierParent = new GameObject()
            {
                name = this.name + " BARRIER",
            }.transform;
            barrierParent.transform.position = this.transform.position;

            for (int i = 0; i < this.barrierCount; i++)
            {
                GameObject piece = Instantiate(this.barrierPrefab, barrierParent);
                piece.transform.localPosition = new Vector3(
                    Mathf.Cos(Mathf.Deg2Rad * 360f * i / this.barrierCount),
                    0,
                    Mathf.Sin(Mathf.Deg2Rad * 360f * i / this.barrierCount)
                ) * this.barrierRange;
                piece.transform.localRotation = Quaternion.Euler(0, UnityEngine.Random.Range(0, 180), 0);

                yield return new WaitForSeconds(3f / this.barrierCount);
            }

            this.hasSpawned = true;
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

                this.animator.SetBool("isSpawning", true);
                this.ShowHealthBar();

                this.StartCoroutine(this.SpawnArena());

                return;
            }

            // si combat commencer, updatetree
            //this.UpdateTree();
            // sinon spawn arene, show healthbar
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

        protected override void OnDeath(float overDamage)
        {
            animator.SetTrigger("isDead");
            enabled = false;
            HideHealthBar();
            StartCoroutine(this.DespawnArena());
        }

        #endregion
    }
}

