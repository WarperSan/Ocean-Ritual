using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private float spawnRadius = 10f;
    [SerializeField] private float spawnInterval = 10f;

    Vector3 spawnPosition;
    Dictionary<GameObject, float> enemies;
    List<GameObject> spawnedEnemies = new();

    private void Start()
    {
        StartCoroutine(SpawnEnemiesAtIntervals());
    }

    #region Spawn

    /// <summary>
    /// Spawns a random enemy at a random position
    /// </summary>
    public void SpawnEnemy()
    {
        if (enemies != null && enemies.Count > 0)
        {
            GameObject enemyToSpawn = enemies.ElementAt(Random.Range(0, enemies.Count)).Key;
            Vector3 spawnPosition = UtilsModule.Random.RandomOnCircumference(spawnRadius, this.spawnPosition);
            GameObject spawnedEnemy = Instantiate(enemyToSpawn, spawnPosition, Quaternion.identity);

            spawnedEnemies.Add(spawnedEnemy);
        }
    }

    /// <summary>
    /// Despawns all enemies
    /// </summary>
    public void DespawnEnemies()
    {
        foreach (GameObject enemy in spawnedEnemies)
        {
            if (enemy != null)
                Destroy(enemy);
        }
        spawnedEnemies.Clear();
    }

    /// <summary>
    /// Spawns enemy at a set interval
    /// </summary>
    /// <returns>Time to wait before spawning enemy</returns>
    private IEnumerator SpawnEnemiesAtIntervals()
    {
        while (true)
        {
            SpawnEnemy();
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    #endregion

    #region Setup

    public void SetUp(Vector3 position, Dictionary<GameObject, float> enemies)
    {
        this.spawnPosition = position;
        this.enemies = enemies;
    }

    #endregion 
}
