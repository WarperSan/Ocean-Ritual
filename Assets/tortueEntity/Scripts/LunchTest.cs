using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LunchTest : MonoBehaviour
{
    [SerializeField]float animationVitesse;
    [SerializeField] float animationTime = 5;
    bool returning = false;
    float timeLapse;
    Transform lunch;
    Vector3 initialPosition;
    // Start is called before the first frame update
    void Start()
    {
        lunch = transform;
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
            lunch.position += lunch.forward * moveDistance;

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
            Vector3 directionBack = initialPosition - lunch.position;
            if (directionBack.magnitude > returnDistance)
            {
                lunch.position += directionBack.normalized * returnDistance;
            }
            else
            {
                lunch.position = initialPosition; // Remet à la position initiale
                returning = false; // Terminé
            }
        }
    }
}
