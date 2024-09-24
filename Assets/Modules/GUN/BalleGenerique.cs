using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface BalleGenerique 
{
    // Propri�t�s
    float Vitesse { get; set; }
    bool DisparaitApresHit { get; set; }
    bool Grossissement { get; set; }
    float ValeurGrossissement { get; set; }
    float Range { get; set; }
    bool DirectionForward { get; set; }
    bool Rotation { get; set; }
    float RotationTodo { get; set; } // Valeur pour la rotation � faire
    bool DisparaitHitObstacle { get; set; }
    bool CoupCritique { get; set; }
    bool EffetSpecial { get; set; }

    // M�thode
    void EnnemiHit(Collider other); // Fonction appel�e lors de l'impact avec un ennemi
    void Deplacement(); // la Fa�on quelle ce d�place
  
}
