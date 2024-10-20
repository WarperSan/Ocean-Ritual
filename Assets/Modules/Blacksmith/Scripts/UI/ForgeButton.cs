using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine;
using UnityEngine.EventSystems;

public class ForgeButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] Fill fillScript;
    [SerializeField] float amount;
    [SerializeField] float timeBetweenFill = 1f;  // Temps entre chaque ajout de remplissage
    private float deltaTime = 0f;                 // Le temps écoulé depuis le dernier remplissage
    public bool isButtonHeld = false;            // Indique si le bouton est maintenu enfoncé

    void Update()
    {
        if (isButtonHeld)
        {
            deltaTime += Time.deltaTime;

            // Vérifie si le temps écoulé dépasse le temps requis pour ajouter du remplissage
            if (deltaTime >= timeBetweenFill)
            {
                AddFill();   // Appelle la méthode pour ajouter du remplissage
                deltaTime = 0f;  // Réinitialise le compteur de temps
            }
        }
    }

    // Méthode appelée lorsque le bouton est maintenu enfoncé
    public void OnPointerDown(PointerEventData eventData)
    {
        isButtonHeld = true;
    }

    // Méthode appelée lorsque le bouton est relâché
    public void OnPointerUp(PointerEventData eventData)
    {
        isButtonHeld = false;
        deltaTime = 0f;  // Réinitialise le temps quand le bouton est relâché
    }

    // Ajoute du remplissage à l'objet lié
    public void AddFill()
    {
        fillScript.AddingFill(amount);
    }
}
