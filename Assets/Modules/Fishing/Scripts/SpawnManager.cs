using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private float spawnRadius = 10f;

    Vector3 spawnPosition;
    Dictionary<GameObject, float> enemies;

    private void Start()
    {
        SpawnEnemy();
    }

    /// <summary>
    /// Spawns a random enemy at a random position
    /// </summary>
    /// <param name="enemies">Array of possible enemies to spawn</param>
    public void SpawnEnemy()
    {
        if (enemies != null)
        {
            GameObject enemyToSpawn = enemies.ElementAt(Random.Range(0, enemies.Count)).Key;
            Vector3 spawnPosition = UtilsModule.Random.RandomOnCircumference(spawnRadius, this.spawnPosition);
            Instantiate(enemyToSpawn, spawnPosition, Quaternion.identity);
        }
    }

    public void SetUp(Vector3 position, Dictionary<GameObject, float> enemies)
    {
        this.spawnPosition = position;
        this.enemies = enemies; 
    }
}
