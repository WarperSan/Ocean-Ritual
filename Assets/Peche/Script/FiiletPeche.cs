using ScriptableObjects;
using Singletons;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class FiiletPeche : MonoBehaviour
{
    [SerializeField] float ZoneDePeche = 20;
    [SerializeField] GameObject SpawneraMob;
    [SerializeField] GameObject GestionnairePoisson;
    List<TerritoryScript> ListeTerritoire;

    public void DetectionTerritoir()
    {
        ListeTerritoire = new List<TerritoryScript>();
        // Lance une sph�re de rayon ZoneDePeche autour de la position de FiiletPeche
        Collider[] colliders = Physics.OverlapSphere(transform.position, ZoneDePeche);
        int compteur = 0;
        // Parcourt tous les colliders d�tect�s
        foreach (Collider collider in colliders)
        {
            // V�rifie si le collider appartient � un objet avec le script Territoire
            TerritoryScript territoire = collider.GetComponent<TerritoryScript>();
            if (territoire != null)
            {
                ListeTerritoire.Add(territoire);
                //Debug.Log($"Territoire d�tect� !    {compteur}");
                compteur++;
            }
        }

        // Debug.Log(ListeTerritoire.Count);
        DonnerEnnemie(ListeDesEnnemis());
        PoissonGestion.UpdateFishes(GetFishes(ListeTerritoire));

        gameObject.SetActive(false);
    }
    private void DonnerEnnemie(List<GameObject> listeEnnemie)
    {
        SpawnerEnnemi spawn = SpawneraMob.GetComponent<SpawnerEnnemi>();
        if (listeEnnemie.Count != 0 && listeEnnemie != null)
        {

            spawn.RecevoirListeEnnemie(listeEnnemie);
        }
        else
            spawn.RecevoirListeEnnemie(new List<GameObject>());
    }
    private List<GameObject> ListeDesEnnemis()
    {
        List<GameObject> ennemisResultat = new List<GameObject>();

        foreach (TerritoryScript territoire in ListeTerritoire)
        {
            List<GameObject> ennemisTerritoire = territoire.ObtenirListeEnnemie();

            foreach (GameObject ennemi in ennemisTerritoire)
            {
                // V�rifie si l'ennemi est d�j� dans la liste r�sultante
                if (!ennemisResultat.Contains(ennemi))
                {
                    ennemisResultat.Add(ennemi);
                }
            }
        }

        return ennemisResultat;
    }

    #region Fish

    /// <summary>
    /// Finds all the unique fishes in the given territories
    /// </summary>
    /// <param name="territories">Territories to check</param>
    /// <returns>Fishes found</returns>
    private static Fish[] GetFishes(IEnumerable<TerritoryScript> territories)
    {
        HashSet<Fish> fishes = new();

        // Finds all the unique fishes
        foreach (TerritoryScript territory in territories)
        {
            foreach (Fish item in territory.GetFishes())
                fishes.Add(item);
        }

        // Copy to array
        var result = new Fish[fishes.Count];
        fishes.CopyTo(result);

        return result;
    }

    #endregion
}