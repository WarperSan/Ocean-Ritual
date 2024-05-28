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
       
            agent.destination = GestionDelimitation.AvoirProie();


    }

   
}
