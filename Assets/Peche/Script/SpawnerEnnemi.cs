using Extensions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class SpawnerEnnemi : MonoBehaviour
{
    [SerializeField] List<GameObject> ennemy;
    [SerializeField] GameObject ConteneurEnnemie;
    //[SerializeField] GameObject GestionnaireDePecheDelimitation;
   // GestionDelimitation ScriptGestionPecheDelimitation;
    [SerializeField] float DelayDepart = 0.01f;
    [SerializeField] float tempsEntreVague = 0.01f;
    [SerializeField] float TauxDeTransferPourcentage = 1f;
    [SerializeField] float TauxDeRetentionDutransfer = 0.3f;
    private int compteurDepartTransfer = 0;
    [SerializeField] GameObject Drapeau;
    [SerializeField] float spawnRadius = 5f; // Rayon de spawn autour du drapeau
    int compteurEnnemie = 0;
    [SerializeField] 
    private ProbabilityForLevel[] probabilities;
    [SerializeField] List<int> stats = new();

    private void Start()
    {
        //ScriptGestionPecheDelimitation = GestionnaireDePecheDelimitation.GetComponent<GestionDelimitation>();
        stats.Add(0);
        stats.Add(0);
        stats.Add(0);
        stats.Add(0);
    }

    void Update()
    {
        if (!GestionDelimitation.instance.FinDeLaPeche)
        {
            if (ennemy.Count != 0)
            {
                if (DelayDepart <= 0)
                {
                    SpawnEnnemy2();
                    DelayDepart = 0.01f; // R�initialiser le d�lai apr�s chaque spawn
                }
                else
                {
                    DelayDepart -= Time.deltaTime;
                }
                AugmenterProbabiliter();
            }
        }
    }

    private void SpawnEnnemy2()
    {
        // S�lectionne une position al�atoire autour du drapeau
        Vector3 randomCirclePos = RandomCircle(Drapeau.transform.position, spawnRadius);

        // G�n�rer un nombre al�atoire entre 1 et 100
        float randomFloat = Random.Range(0f, 100f);

        // S�lectionner un niveau en fonction des probabilit�s
        int selectedLevel = SelectLevel(randomFloat);

        // Trouver un ennemi avec le niveau s�lectionn�
        GameObject ennemiPrefab = FindEnemyByLevel(selectedLevel);

        // Instancie l'ennemi � la position al�atoire

        GameObject ennemiInstance = ObjectPool.objectPool.GetObjectRandomRayon(ennemiPrefab, randomCirclePos, ConteneurEnnemie);

        // Active l'ennemi
        ennemiInstance.SetActive(true);

        // Donne la destination � l'ennemi (le drapeau)
        PoursuivreJoueur mettreDestination = ennemiInstance.GetComponent<PoursuivreJoueur>();
        if (mettreDestination != null)
        {
            mettreDestination.DonnerProie(Drapeau);
        }
    }

    // M�thode pour obtenir une position al�atoire autour du point central dans un cercle
    private Vector3 RandomCircle(Vector3 center, float radius)
    {
        float angle = Random.Range(0f, 360f);
        float randomX = center.x + radius * Mathf.Sin(angle * Mathf.Deg2Rad);
        float randomZ = center.z + radius * Mathf.Cos(angle * Mathf.Deg2Rad);
        return new Vector3(randomX, center.y, randomZ);
    }

    // Dessiner le gizmo pour afficher le rayon de spawn
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(Drapeau.transform.position, spawnRadius);
    }

    public void RecevoirListeEnnemie(List<GameObject> listeEnnemie)
    {
        ennemy = listeEnnemie;
        
            MettreAJourListeEnnemie();
        
    
        compteurDepartTransfer = 0;
    }

    public void MettreAJourListeEnnemie()
    {
        if (ennemy.Count == 0)
        {
            probabilities = System.Array.Empty<ProbabilityForLevel>();
            return;
        }

        List<Ennemie> listeEnnemie = new List<Ennemie>();
        foreach (GameObject unEnemie in ennemy)
        {
            Ennemie en = unEnemie.GetComponent<Ennemie>();
            listeEnnemie.Add(en);
        }
    

        probabilities = GestionProbabiliter.ProbabilitiesForLevels(listeEnnemie.GetUniques(e => e.AvoirNiveau()));
    }

    private int SelectLevel(float randomValue)
    {
        float cumulativeProbability = 0f;
        foreach (ProbabilityForLevel prob in probabilities)
        {
            cumulativeProbability += prob.Probability;
            if (randomValue <= cumulativeProbability)
                return prob.Level;
        }
        Debug.Log("valeur" + randomValue);
        // Si aucun niveau n'est s�lectionn�, retourner le dernier niveau disponible
        return 0;
    }

    private GameObject FindEnemyByLevel(int level)
    {
       // Debug.Log(level);
        stats[level - 1] = stats[level - 1] + 1;
        List<GameObject> enemiesWithLevel = new List<GameObject>();

        // Parcourir la liste des ennemis pour trouver ceux avec le niveau sp�cifi�
        foreach (GameObject enemyPrefab in ennemy)
        {
            Ennemie ennemiComponent = enemyPrefab.GetComponent<Ennemie>();
            if (ennemiComponent != null && ennemiComponent.AvoirNiveau() == level)
            {
                enemiesWithLevel.Add(enemyPrefab);
            }
        }

        // Si des ennemis avec le niveau sp�cifi� ont �t� trouv�s
        if (enemiesWithLevel.Count > 0)
        {
            // S�lectionner al�atoirement un ennemi parmi ceux avec le niveau sp�cifi�
            int randomIndex = Random.Range(0, enemiesWithLevel.Count);
            return enemiesWithLevel[randomIndex];
        }
        // Aucun ennemi avec le niveau sp�cifi� n'a �t� trouv�
        return null;
    }

    private void AugmenterProbabiliter()
    {
        // Calculer le transfert initial de probabilit�
        float initialTransferRate = Time.deltaTime * TauxDeTransferPourcentage;
        int compteurbasse = 0;
        // Transf�rer progressivement les probabilit�s du premier au dernier niveau
        for (int i = compteurDepartTransfer; i < probabilities.Length - 1; i++)
        {
   
            // Calculer le montant de probabilit� � transf�rer (30% de la probabilit� restante)
     
            float transferAmount =initialTransferRate * Mathf.Pow(TauxDeRetentionDutransfer, compteurbasse);
            transferAmount = Mathf.Min(transferAmount, probabilities[i].Probability);
           // Debug.Log(transferAmount);
           // Debug.Log("le I est  =  a    "+i +  " et tranfer   =  "  + transferAmount);
            // Transf�rer la probabilit� du niveau actuel au niveau suivant
            probabilities[i].Probability -= transferAmount;
            probabilities[i + 1].Probability += transferAmount;

            if (probabilities[i].Probability <= 0)
            {
                compteurDepartTransfer += 1;
            }


                // Arr�ter si le taux de transfert est devenu trop faible
                if (transferAmount <= 0.001f)
            {
                break;
            }
            compteurbasse++;
        }
    }
}