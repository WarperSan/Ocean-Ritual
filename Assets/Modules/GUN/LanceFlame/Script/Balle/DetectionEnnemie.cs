using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectionEnnemie : MonoBehaviour
{
    [SerializeField] BalleLF balle;
    void OnTriggerEnter(Collider other)
    {
      
        // Vérifie si l'objet touché appartient au layer Ennemi
        if (other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            // Appelle la logique d'impact avec l'ennemi
            balle.EnnemiHit(other);

           
            
        }
    }
}
