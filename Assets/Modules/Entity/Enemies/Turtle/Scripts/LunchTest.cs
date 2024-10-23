using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LunchTest : MonoBehaviour
{
    [SerializeField]float animationVitesse;
    [SerializeField] float animationTime = 5;
    [SerializeField] Transform lunch;
    bool returning = false;
    float timeLapse;
    Transform aa;
    Vector3 initialPosition;
    // Start is called before the first frame update
    void Start()
    {
        aa = transform;
        initialPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        timeLapse += Time.deltaTime;
        DoLunchAnimation();
    }
    public void DoLunchAnimation()
    {
        if (!returning)
        {
            // Avance en ligne droite
            float moveDistance = animationVitesse * Time.deltaTime;
            aa.position += aa.forward * moveDistance;

            // Vérifie si l'objet a atteint la fin de l'animation
            if (timeLapse >= animationTime)
            {
                returning = true; // Commence la phase de retour
                timeLapse = 0f; // Réinitialise le timer pour le retour
            }
        }
        else
        {
            // Retour avec effet de rebond
            float returnDistance = animationVitesse * Time.deltaTime;

            // On calcule la position cible
            Vector3 directionBack = initialPosition - aa.position;
            if (directionBack.magnitude > returnDistance)
            {
                aa.position += directionBack.normalized * returnDistance;
            }
            else
            {
                aa.position = initialPosition; // Remet à la position initiale
                returning = false; // Terminé
            }
        }
    }
    // Ceci sera appelé dans l'éditeur Unity pour visualiser le raycast.
    void OnDrawGizmos()
    {
        if (lunch != null)
        {
            // Couleur du rayon
            Gizmos.color = Color.red;

            // Dessine une ligne (rayon) depuis la position de lunch dans la direction de lunch.forward
            Gizmos.DrawRay(lunch.position, lunch.forward * 5f);  // Le 5f représente la longueur du rayon

            // Optionnel : Dessine une sphère à la fin du rayon pour mieux voir la direction
            Gizmos.DrawSphere(lunch.position + lunch.forward * 5f, 0.1f);
        }
    }

}
