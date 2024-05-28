using ScriptableObjects;
using System.Collections.Generic;
using UnityEngine;

public class TerritoryScript : MonoBehaviour
{
    [SerializeField]
    private Territory Territory;

    [SerializeField] List<GameObject> ListEnnemies;

    public Fish[] GetFishes() => this.Territory.Fishes;

    // M�thode pour obtenir une copie de la liste des ennemis
    public List<GameObject> ObtenirListeEnnemie()
    {
        return new List<GameObject>(ListEnnemies);
    }
}
