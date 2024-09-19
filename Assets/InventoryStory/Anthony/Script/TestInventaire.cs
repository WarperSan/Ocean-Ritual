using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestInventaire : MonoBehaviour
{

    [SerializeField] Inventaire inventaireJoueur;
    // Start is called before the first frame update
    void Start()
    {
        inventaireJoueur.InitiateListe();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
