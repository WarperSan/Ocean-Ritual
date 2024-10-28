using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarsManager : MonoBehaviour
{

    [SerializeField] GameObject Bronze;
    [SerializeField] GameObject Argent;
    [SerializeField] GameObject Or;
    [SerializeField] GameObject StarsContainer;
    [SerializeField] bool updateData = false;
    [SerializeField] int LVL = 1;
    [SerializeField] int MaxLVL = 100;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (updateData)
        {
            CreatUiStars(LVL);
        }
    }
    public void CreatUiStars(int lvl)
    {
        if (lvl > MaxLVL)
        {
            Debug.Log("niveau trop grand  de la gemme");
            return;
        }
        // On vide d'abord le conteneur pour éviter les doublons
        foreach (Transform child in StarsContainer.transform)
        {
            Destroy(child.gameObject);
        }

        // Calcul du nombre de chaque étoile
        int numOr = lvl / 10;            // Chaque étoile Or représente 10 niveaux
        int remainingAfterOr = lvl % 10;

        int numArgent = remainingAfterOr / 5; // Chaque étoile Argent représente 5 niveaux
        int remainingAfterArgent = remainingAfterOr % 5;

        int numBronze = remainingAfterArgent; // Chaque étoile Bronze représente 1 niveau

        // Création des étoiles Or
        for (int i = 0; i < numOr; i++)
        {
            Instantiate(Or, StarsContainer.transform);
        }

        // Création des étoiles Argent
        for (int i = 0; i < numArgent; i++)
        {
            Instantiate(Argent, StarsContainer.transform);
        }

        // Création des étoiles Bronze
        for (int i = 0; i < numBronze; i++)
        {
            Instantiate(Bronze, StarsContainer.transform);
        }
    }
}
