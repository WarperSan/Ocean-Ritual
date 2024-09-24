using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface BalleGenerique 
{
    // Propriétés
    float Vitesse { get; set; }
    bool DisparaitApresHit { get; set; }
    bool Grossissement { get; set; }
    float ValeurGrossissement { get; set; }
    float Range { get; set; }
    bool DirectionForward { get; set; }
    bool Rotation { get; set; }
    float RotationTodo { get; set; } // Valeur pour la rotation à faire
    bool DisparaitHitObstacle { get; set; }
    bool CoupCritique { get; set; }
    bool EffetSpecial { get; set; }

    // Méthode
    void EnnemiHit(Collider other); // Fonction appelée lors de l'impact avec un ennemi
    void Deplacement(); // la Façon quelle ce déplace
  
}
