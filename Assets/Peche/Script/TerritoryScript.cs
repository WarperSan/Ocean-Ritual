using ScriptableObjects;
using UnityEngine;

public class TerritoryScript : MonoBehaviour
{
    [SerializeField]
    private Territory Territory;

    public Fish[] GetFishes() => this.Territory.Fishes;
    public GameObject[] GetEnemies() => this.Territory.Enemies;
}
