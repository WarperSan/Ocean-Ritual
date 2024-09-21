using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestInventaire : MonoBehaviour
{
    [SerializeField] Inventaire inventaireJoueur;

    [SerializeField] bool ajouterPoisson = false;
    [SerializeField] bool ajouterGemme = false;
    [SerializeField] PoissonData poisson;
    [SerializeField] GemmeData gemme;

    // Start is called before the first frame update
    void Start()
    {
        inventaireJoueur.InitiateListe();
    }

    // Update is called once per frame
    void Update()
    {
        if (ajouterPoisson)
        {
            ajouterPoisson = false; // Remplace ! par false pour ne pas inverser à chaque update

            // Crée une nouvelle instance de PoissonData
            PoissonData nouveauPoisson = new PoissonData()
            {
                nom = poisson.nom,
                quantiter = poisson.quantiter,
                quantiterMax = poisson.quantiterMax
            };

            inventaireJoueur.AddItem(nouveauPoisson); // Utilise la nouvelle instance
        }

        if (ajouterGemme)
        {
            ajouterGemme = false; // Remplace ! par false pour ne pas inverser à chaque update

            // Crée une nouvelle instance de GemmeData
            GemmeData nouvelleGemme = new GemmeData()
            {
                GemmeColorsName = gemme.GemmeColorsName,
                LVL = gemme.LVL,
                 quantiter =1,
                quantiterMax =1
            };

            inventaireJoueur.AddItem(nouvelleGemme); // Utilise la nouvelle instance
        }
    }
}
