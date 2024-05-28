using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Ennemie : MonoBehaviour
{
    [SerializeField] string nom = "nomTemporaire";
    [SerializeField] int NiveauDeEnnemie = 0;

    public int AvoirNiveau()
    {
        return NiveauDeEnnemie;
    }
}
