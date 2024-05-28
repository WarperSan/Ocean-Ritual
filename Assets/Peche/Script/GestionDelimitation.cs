using Fishing;
using ScriptableObjects;
using Singletons;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GestionDelimitation : MonoBehaviour
{
    [SerializeField] GameObject Joueur;
    [SerializeField] GameObject Drapeau;
    [SerializeField] GameObject ConteneurEnnemie;
    [SerializeField] GameObject PoissonGestion;
    Vector3 PositionInitial;
    //Distance pour le premier cercle
    [SerializeField] float rayonZonePeche = 5f;
    //Distance pour le deuxi�me cercle
    [SerializeField] float rayonLimiteChasse = 10f;
    //Distance pour le trois�me cercle
    [SerializeField] float rayonFinChasse = 15f;
    public static GestionDelimitation instance;
     public bool FinDeLaPeche = false;
     bool PoursuivreJoueur = false;

    // Start is called before the first frame update

    public   void DebutLimitation()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            if (instance != this)
            Destroy(gameObject); // Ensure only one instance of this script exists
        }
        instance.FinDeLaPeche = false;
        instance.FinDeLaPeche = false;
        // V�rifier que les rayons sont dans l'ordre correct
        VerifierRayons();

        // Sauvegarder la position initiale du drapeau
        if (Drapeau != null)
        {
            PositionInitial = Drapeau.transform.position;
        }
        else
        {
            Debug.LogError("Joueur n'est pas assign�.");
        }
    }
 

    private void VerifierRayons()
    {
        bool valid = true;

        if (rayonZonePeche >= rayonLimiteChasse)
        {
            Debug.LogError("Le rayon de la zone de p�che doit �tre plus petit que le rayon de la limite de chasse.");
            rayonZonePeche = 5f;
            valid = false;
        }

        if (rayonLimiteChasse >= rayonFinChasse)
        {
            Debug.LogError("Le rayon de la limite de chasse doit �tre plus petit que le rayon de la fin de la chasse.");
            rayonLimiteChasse = 10f;
            valid = false;
        }

        if (!valid)
        {
            Debug.LogError("Des valeurs par d�faut ont �t� d�finies pour les rayons incorrects.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (FinDeLaPeche)
        {
            GestionFinDeLaPeche();
        }
        else if (Joueur != null)
        {
            VerifierZones();
        }
    }
    private void GestionFinDeLaPeche()
    {
        // V�rifier que ConteneurEnnemie est assign�
        if (ConteneurEnnemie != null)
        {
            // Obtenir tous les enfants sous forme de Transforms
            Transform[] listeEnnemie = ConteneurEnnemie.GetComponentsInChildren<Transform>();

            // Parcourir chaque enfant et le d�truire
            foreach (Transform ennemi in listeEnnemie)
            {
                // Ne pas supprimer le transform de ConteneurEnnemie lui-m�me
                if (ennemi != ConteneurEnnemie.transform)
                {
                    Destroy(ennemi.gameObject);
                }
                ObjectPool.objectPool.ResetPool();
            }
        }
        else
        {
            Debug.LogError("ConteneurEnnemie n'est pas assign�.");
        }
        if (Drapeau != null)
        {
            FishingBuoy  ScritDrapeau  = Drapeau.GetComponent<FishingBuoy>();
            // le if a savoir si le joueur repart avec ou sans son butin de poison
            if (true)
            {
                List<Fish> listePoison = ScritDrapeau.ObtainInventory();
            }
            //sinon il n'a rien
            else
            {

            }
            
        }
        
        if (PoissonGestion!= null)
        {
            PoissonGestion.GetComponent<PoissonGestion>().enabled = false;
        }
        
    }

    private void VerifierZones()
    {
        float distance = Vector3.Distance(Joueur.transform.position, PositionInitial);

        if (distance <= rayonZonePeche)
        {
            Debug.Log("Le joueur est dans la zone de p�che.");
            instance.PoursuivreJoueur = true;
        }
        else if (distance <= rayonLimiteChasse)
        {
            Debug.Log("Le joueur est dans la limite de chasse.");
            instance.PoursuivreJoueur = true;
        }
        else if (distance <= rayonFinChasse)
        {
            Debug.Log("Le joueur est dans la zone de pause/paix.");
            instance.PoursuivreJoueur = false;
        }
        else
        {
            Debug.Log("Le joueur a d�pass� la fin de la chasse.");
            FinDeLaPeche = true;
            instance.PoursuivreJoueur = false;
            var fishes = Drapeau.GetComponent<FishingBuoy>().ObtainInventory();
            Debug.Log(fishes.Count);

            foreach (var item in fishes)
                Debug.Log(item.DisplayName);
        }
    }


    // M�thode statique pour obtenir la r�f�rence � Joueur pour les ennemie cela permet de dicter quoi chaser
    //permet futurment d,Impl�menter un leurre(id�e)
    public static Vector3 AvoirProie()
    {
        if (instance != null)
        {
            if (instance.PoursuivreJoueur)
            {
                return instance.Joueur.transform.position;
            }
            else
            {
                return instance.PositionInitial;
            }

        }
        return new Vector3(0, 0, 0);
    }
   


    //aide temporaire au debogage
    private void OnDrawGizmos()
    {
        if (Joueur != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(PositionInitial, rayonZonePeche);

            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(PositionInitial, rayonLimiteChasse);

            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(PositionInitial, rayonFinChasse);
        }
    }

}