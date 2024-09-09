using ExtensionsModule;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UtilsModule.Random;

namespace FishingModule
{
    public class FishingEnemyManager : MonoBehaviour
    {
        #region Enemies

        private Enemy[] enemies;

        /// <summary>
        /// Updates the probabilities for the given enemies
        /// </summary>
        public void UpdateEnemies(Enemy[] enemies)
        {
            // Reset
            this.transferOffset = 0;
            this.delay = this.spawnDelay;

            // Update odds
            IEnumerable<int> levels = enemies.GetUniques(e => e.GetLevel());
            this.probabilities = ProbabilitiesForLevels(levels);

            // Update enemies
            this.enemies = enemies;
        }

        /// <summary>
        /// Finds an enemy with the given level
        /// </summary>
        /// <param name="level">Level to have</param>
        /// <returns>Enemy found</returns>
        private Enemy FindEnemyByLevel(int level)
        {
            // Find all with given level
            Enemy[] enemiesWithLevel = this.enemies.Where(e => e.GetLevel() == level).ToArray();

            return enemiesWithLevel.Random(out _);
        }

        #endregion
        
        #region Probabilities

        private ProbabilityForLevel[] probabilities;

        private float transferRate = 1f;
        private float retentionRate = 0.3f;
        private int transferOffset = 0;

        /// <summary>
        /// Increases the probabilities over time
        /// </summary>
        /// <param name="elapsed">Time passed since the last frame</param>
        private void IncreaseProbabilities(float elapsed)
        {
            // If probabilities invalid, skip
            if (this.probabilities == null)
                return;

            float transferRate = elapsed * this.transferRate;
            int counter = 0;

            for (int i = this.transferOffset; i < this.probabilities.Length - 1; i++)
            {
                float transferAmount = Mathf.Min(
                    transferRate * Mathf.Pow(retentionRate, counter), 
                    this.probabilities[i].Probability
                );

                // Transfer amount between levels
                this.probabilities[i].Probability -= transferAmount;
                this.probabilities[i + 1].Probability += transferAmount;

                // If current level reached 0, offset by 1
                if (this.probabilities[i].Probability <= 0)
                    this.transferOffset++;

                // If amount too low, exit
                if (transferAmount <= 0.001f)
                    break;

                counter++;
            }
        }

        /// <summary>
        /// Sets the rates for this manager
        /// </summary>
        public void SetRates(float transferRate, float retentionRate)
        {
            this.transferRate = transferRate;
            this.retentionRate = retentionRate;
        }

        #endregion

        #region Spawn

        private float spawnDelay = 0.01f;
        private float spawnRadius = 5f;
        private float delay;

        /// <summary>
        /// Updates the spawn of the enemies
        /// </summary>
        /// <param name="elapsed">Time passed since the last frame</param>
        private void UpdateSpawn(float elapsed)
        {
            this.IncreaseProbabilities(elapsed);

            if (this.delay > 0)
            {
                this.delay -= elapsed;
                return;
            }

            this.SpawnEnnemy();
            this.delay = this.spawnDelay;
        }

        /// <summary>
        /// Tries to spawn an enemy of a random level
        /// </summary>
        private void SpawnEnnemy()
        {
            // Fetch an enemy of a random level 
            int selectedLevel = this.probabilities.GetRandomLevel();
            Enemy enemy = this.FindEnemyByLevel(selectedLevel);

            // If no enemy found, skip
            if (enemy == null)
                return;

            // Missing: Spawn enemy
        }

        /// <returns>Position to spawn the enemy</returns>
        private Vector3 GetSpawnPos() => RandomOnCircumference(this.spawnRadius, this.GetOrigin());

        /// <returns>Origin of the spawn radius</returns>
        private Vector3 GetOrigin() => this.transform.position;

        /// <summary>
        /// Sets the spawn settings of this manager
        /// </summary>
        public void SetSpawnSettings(float delay, float radius)
        {
            this.spawnDelay = delay;
            this.delay = delay;
            this.spawnRadius = radius;
        }

        #endregion

        #region MonoBehaviour

        /// <inheritdoc/>
        private void Update() => this.UpdateSpawn(Time.deltaTime);

        #endregion

        #region Gizmos
#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            UnityEditor.Handles.color = Color.cyan;
            UnityEditor.Handles.DrawWireDisc(this.GetOrigin(), Vector3.up, this.spawnRadius, 5);
        }
#endif
        #endregion
    }
}