using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface EnnemieGenerique
{
    // Propriétés
    float Life { get; set; }
    float Damage { get; set; }
    float AttackSpeed { get; set; }

    // Méthodes
    void Movement(); // Gérer le mouvement de l'ennemi
    void TakeHit(float damage); // Réagir lorsqu'il est touché
    void DoHit(); // Effectuer une attaque
}