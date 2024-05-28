using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PoursuivreJoueur : MonoBehaviour
{
    GameObject Proie;
    NavMeshAgent agent;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    public void DonnerProie(GameObject proie)
    {
        Proie = proie;
    }

    void Update()
    {
        // Vector3? target = GestionDelimitation.AvoirProie();

        // if (target.HasValue)
        //     agent.destination = target.Value;
    }
}
