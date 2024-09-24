using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] int NiveauDeEnnemie = 0;

    public int GetLevel()
    {
        return NiveauDeEnnemie;
    }
}
