using ControllerModule.Controllers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public abstract class Arme : Controller
{


    public abstract float GetDamage();// donne a la balle le nombre de d�gat qu'elle fait
    public abstract void OverHeat();// r�action du a la surchaufe
    public abstract void OverHeatIndicator();//Indicateur de surchaufe
    public abstract void Reload();//rechargement
    public abstract void MainShoot();// tire principale
    public abstract void ChangeMode();// Changerde mode si il y a lieu
    public abstract void SecondaryShoot(); //tire secondaire si il y a lieu

    public abstract void Rotation();// g�re la rotation de l'arme

    public abstract void InstantiateAmmo();//Fonction appeler pour tire une balle

    public abstract void GetEffect();//Fonction pour passe les effet sp�ciaux
    public abstract void SetBulletProriety(GameObject balle);// g�re les stat de la balle
}
