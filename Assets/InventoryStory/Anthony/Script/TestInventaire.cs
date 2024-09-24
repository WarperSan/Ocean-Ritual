using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static EnumGeneral;

public class TestInventaire : MonoBehaviour
{
    [SerializeField] Inventaire inventaireJoueur;

    [SerializeField] bool ajouterPoisson = false;
    [SerializeField] bool ajouterGemme = false;
    [SerializeField] bool clearInventaire = false;
    [SerializeField] bool faireLeTrie = false;
    [SerializeField] bool SwapPlace = false;
    [SerializeField] bool drop = false;
    [SerializeField] int index1;
    [SerializeField] int index2;
    [SerializeField] int indexDrop;
    [SerializeField] PoissonData poisson;
    [SerializeField] GemmeData gemme;
    [SerializeField] TypeOfSort typeDeTri;

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
                quantiterMax = poisson.quantiterMax,
                sprite = poisson.sprite,
            };

            Inventaire.Instance.AddItem(nouveauPoisson); // Utilise la nouvelle instance
        }

        if (ajouterGemme)
        {
            ajouterGemme = false; // Remplace ! par false pour ne pas inverser à chaque update

            // Crée une nouvelle instance de GemmeData
            GemmeData nouvelleGemme = new GemmeData()
            {
                GemmeColorsName = gemme.GemmeColorsName,
                LVL = gemme.LVL,
                quantiter = 1,
                quantiterMax = 1
            };

            Inventaire.Instance.AddItem(nouvelleGemme); // Utilise la nouvelle instance
        }
        if (clearInventaire)
        {
            clearInventaire = false;
            inventaireJoueur.NettoyerEmplacement();
        }
        if (faireLeTrie)
        {
            faireLeTrie = false;
            inventaireJoueur.TrierItemList(typeDeTri);
        }
        if (SwapPlace)
        {
            SwapPlace = false;
            inventaireJoueur.SwapPlace(index1, index2);
        }
        if (drop)
        {
            drop = false;
            inventaireJoueur.DropItem(indexDrop);
        }
    }
}