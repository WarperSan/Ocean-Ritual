using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarsManager : MonoBehaviour
{

    [SerializeField] GameObject Bronze;
    [SerializeField] GameObject Argent;
    [SerializeField] GameObject Or;
    [SerializeField] GameObject Empty;
    [SerializeField] GameObject StarsContainer;
    [SerializeField] bool updateData = false;
    [SerializeField] int LVL = 1;
    [SerializeField] int totalStars = 16;
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
        // On vide d'abord le conteneur pour �viter les doublons
        foreach (Transform child in StarsContainer.transform)
        {
            Destroy(child.gameObject);
        }

        // Calcul du nombre de chaque �toile
        int numOr = lvl / 10;            // Chaque �toile Or repr�sente 10 niveaux
        int remainingAfterOr = lvl % 10;

        int numArgent = remainingAfterOr / 5; // Chaque �toile Argent repr�sente 5 niveaux
        int remainingAfterArgent = remainingAfterOr % 5;

        int numBronze = remainingAfterArgent; // Chaque �toile Bronze repr�sente 1 niveau

        // Cr�ation des �toiles Or
        for (int i = 0; i < numOr; i++)
            Instantiate(Or, StarsContainer.transform);

        // Cr�ation des �toiles Argent
        for (int i = 0; i < numArgent; i++)
            Instantiate(Argent, StarsContainer.transform);

        // Cr�ation des �toiles Bronze
        for (int i = 0; i < numBronze; i++)
            Instantiate(Bronze, StarsContainer.transform);

        for (int i = numBronze + numArgent + numOr; i < totalStars; i++)
            Instantiate(Empty, StarsContainer.transform);
    }
}
