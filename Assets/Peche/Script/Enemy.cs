using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] string nom = "nomTemporaire";
    [SerializeField] int NiveauDeEnnemie = 0;

    public int GetLevel()
    {
        return NiveauDeEnnemie;
    }
}
