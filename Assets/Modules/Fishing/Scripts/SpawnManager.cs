using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private float spawnRadius = 10f; 
    [SerializeField] private Vector3 spawnCenter = Vector3.zero; 

    /// <summary>
    /// Spawns a random enemy at a random position
    /// </summary>
    /// <param name="enemies">Array of possible enemies to spawn</param>
    public void SpawnEnemies(GameObject[] enemies)
    {
        if (enemies != null || enemies.Length != 0)
        {
            GameObject enemyToSpawn = enemies[Random.Range(0, enemies.Length)];
            Vector3 spawnPosition = UtilsModule.Random.RandomOnCircumference(spawnRadius, spawnCenter);
            Instantiate(enemyToSpawn, spawnPosition, Quaternion.identity);
        }
    }
}
