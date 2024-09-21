using System;
using System.Collections;
using System.Collections.Generic;

using System.Linq.Expressions;
using UnityEngine;
using static EnumGeneral;
using System.Linq;
[System.Serializable]
public class Inventaire : MonoBehaviour
{
    private int nombreDePlaceInventaire =10;
  [SerializeField]  public List<ItemData> ItemList = new ();
    [SerializeField] List<PoissonData> poissons;
    [SerializeField] List<GemmeData> gemmes;
    bool InventaireOuvert = false;
    // Start is called before the first frame update
    void Start()
    {
        AjouterPoissonsDeTest();
        AjouterGemmesDeTest();
        UpdateSousListe();
    }
    void AjouterGemmesDeTest()
    {
        string[] gemmeColors = { "Gemme Rouge", "Gemme Bleue", "Gemme Verte" };

        for (int i = 0; i < 6; i++)
        {
            GemmeData gemme = new GemmeData
            {
                GemmeColorsName = gemmeColors[i % gemmeColors.Length], // Alternance des couleurs Rouge, Bleue, Verte
                LVL = i + 1 // Niveau croissant
            };
            ItemList.Add(gemme);
        }
    }
    void AjouterPoissonsDeTest()
    {
        // Poisson A
        for (int i = 0; i < 3; i++)
        {
            PoissonData poissonA = new PoissonData
            {
                nom = "Poisson A",
                quantiterMax = 5,
                quantiter = 5 // Quantité égale à la quantité maximale
            };
            ItemList.Add(poissonA);
        }

        // Poisson B
        for (int i = 0; i < 3; i++)
        {
            PoissonData poissonB = new PoissonData
            {
                nom = "Poisson B",
                quantiterMax = 8,
                quantiter = 8 // Quantité égale à la quantité maximale
            };
            ItemList.Add(poissonB);
        }

        // Poisson C
        for (int i = 0; i < 3; i++)
        {
            PoissonData poissonC = new PoissonData
            {
                nom = "Poisson C",
                quantiterMax = 10,
                quantiter = 10 // Quantité égale à la quantité maximale
            };
            ItemList.Add(poissonC);
        }
    }
    public void UpdateSousListe()
    {
        poissons = ItemList.OfType<PoissonData>().ToList();
        gemmes = ItemList.OfType<GemmeData>().ToList();
    }
    public void InitiateListe()
    {
        ItemList = new List<ItemData>();
    }
    public void UpgradeInventory(int AddingStockage)
    {
        if (AddingStockage < 0)
        {
            Debug.LogWarning($"AddingStockage is less than 0   :   {AddingStockage}     : upgrade fail");
        }
        else
        {
            nombreDePlaceInventaire += AddingStockage;
        }
       
    }


    public (List<int>, List<int>) ItemExistantDansListe(ItemData item)
    {
        List<int> ListeIndexDisponible = new(); // Index où l'objet peut être placé (vide)
        List<int> ListeIndexItemIdentique = new(); // Index d'objets identiques (même nom/type)

        // Parcourt l'inventaire pour trouver des emplacements disponibles ou identiques
        for (int i = 0; i < ItemList.Count; i++)
        {
            if (ItemList[i] == null)
            {
                // Ajoute les emplacements vides
                ListeIndexDisponible.Add(i);
            }
            else
            {
                // Si l'objet est un PoissonData
                if (item is PoissonData poisson && ItemList[i] is PoissonData poissonInList)
                {
                    // Vérifie que le nom est identique et que la quantité max n'est pas atteinte
                    if (poissonInList.nom == poisson.nom && poissonInList.quantiter < poissonInList.quantiterMax)
                    {
                        ListeIndexItemIdentique.Add(i);
                    }
                }
                // Si l'objet est une GemmeData, on ignore les emplacements contenant d'autres objets
                else if (item is GemmeData)
                {
                    // Ne fais rien pour les Gemmes, on ne veut que les emplacements vides
                }
            }
        }

        return (ListeIndexItemIdentique, ListeIndexDisponible);
    }

    private void AjoutEmplacement()
    {
        ItemList.Add(null);
    }
    public void NettoyerEmplacement()
    {
        // Traverse la liste à l'envers
        for (int i = ItemList.Count - 1; i >= 0; i--)
        {
            // Si on trouve un élément non null, on garde un seul null à la fin et on arrête
            if (ItemList[i] != null)
            {
                // S'il y a déjà un null à la fin, on le garde, sinon on en ajoute un
                if (i == ItemList.Count - 1 || ItemList[^1] != null)
                {
                    AjoutEmplacement();// On garde un emplacement vide (null) à la fin
                }
                break;
            }

            // Supprime les éléments null s'ils sont inutiles à la fin de la liste
            ItemList.RemoveAt(i);
        }
        UpdateSousListe();
    }

    public void SwapPlace(int index1, int index2)
    {
        // Vérifie que les index sont valides
        if (index1 < 0 || index1 >= ItemList.Count || index2 < 0 || index2 >= ItemList.Count)
        {
            Debug.LogError("Les index fournis ne sont pas valides.");
            return;
        }

        // Effectue l'échange des éléments
        ItemData temp = ItemList[index1];  
        ItemList[index1] = ItemList[index2];  
        ItemList[index2] = temp;
        UpdateSousListe();
    }
    public void DropItem(int index)
    {
        // Vérifie si l'index est dans les limites de la liste
        if (index >= 0 && index < ItemList.Count)
        {
            ItemList.RemoveAt(index);
           
        }
        else
        {
            // Avertit que l'index est invalide
            Debug.LogWarning($"Index invalide : {index}. Aucune suppression effectuée.");
        }
        UpdateSousListe();
    }
    public ItemData GetItem(int index)
    {
     return  ItemList[index];

    }
    public void AddItem(ItemData item)
    {
        // Récupère les emplacements d'objets similaires et disponibles
        var (ListeIndexItemIdentique, ListeIndexDisponible) = ItemExistantDansListe(item);
        Debug.Log("Liste des indices d'objets identiques : " + string.Join(", ", ListeIndexItemIdentique));
        Debug.Log("Liste des indices d'emplacements disponibles : " + string.Join(", ", ListeIndexDisponible));
        int quantiteRestante = item.quantiter;

        // 1. Ajoute aux emplacements d'objets identiques si possible
        foreach (int index in ListeIndexItemIdentique)
        {
            var itemInList = ItemList[index];
            int placeDisponible = itemInList.quantiterMax - itemInList.quantiter;

            if (quantiteRestante <= placeDisponible)
            {
                itemInList.quantiter += quantiteRestante;
                quantiteRestante = 0;
                break;
            }
            else
            {
                itemInList.quantiter = itemInList.quantiterMax;
                quantiteRestante -= placeDisponible;
            }
        }

        // 2. Si la quantité restante > 0, ajoute dans les emplacements disponibles
        foreach (int index in ListeIndexDisponible)
        {
            if (quantiteRestante == 0)
            {
                break;
            }

            ItemList[index] = item;
            if (quantiteRestante <= item.quantiterMax)
            {
                ItemList[index].quantiter = quantiteRestante;
                quantiteRestante = 0;
            }
            else
            {
                ItemList[index].quantiter = item.quantiterMax;
                quantiteRestante -= item.quantiterMax;
            }
        }

        // 3. Si encore de la quantité à placer, crée un nouvel emplacement
        int iterationLimit = 100; // Limite maximale d'itérations pour éviter les boucles infinies
        int iterationCount = 0;   // Compteur d'itérations

        while (quantiteRestante > 0)
        {
            AjoutEmplacement();
            int dernierIndex = ItemList.Count - 1;
            ItemList[dernierIndex] = item;

            if (quantiteRestante <= item.quantiterMax)
            {
                ItemList[dernierIndex].quantiter = quantiteRestante;
                quantiteRestante = 0;
            }
            else
            {
                ItemList[dernierIndex].quantiter = item.quantiterMax;
                quantiteRestante -= item.quantiterMax;
            }

            // Incrémentation du compteur d'itérations
            iterationCount++;

            // Si la limite d'itérations est atteinte, on sort de la boucle
            if (iterationCount >= iterationLimit)
            {
                Debug.LogError("Boucle infinie détectée, la boucle a été interrompue après " + iterationLimit + " itérations.");
                break; // Sort de la boucle pour éviter de bloquer le programme
            }
        }
        UpdateSousListe();
    }


    public List<ItemData> GetInventaire()
    {
        return new List<ItemData>(ItemList); // Crée une nouvelle liste en copiant l'ancienne
    }


    public void UpdateItemListeUI()     
    {
      
    }
    public void TrierItemList(TypeOfSort SortType)
    {
        switch (SortType)
        {
            case TypeOfSort.Nom:
                TrierNom();
                Console.WriteLine("Tri par nom de poisson les gemme apres");
                break;

            case TypeOfSort.Type:
                TrierParType();
                Console.WriteLine("Tri par type poisson ou gemme");
                break;

            case TypeOfSort.Quantite:
                TrierQuantiter();
                Console.WriteLine("Tri par quantité de poisson");
                break;

            case TypeOfSort.Fusion:
                FusionAuto();
                Console.WriteLine(" faire la fusion");
                break;

            case TypeOfSort.Niveau:
                TrierNiveau();
                Console.WriteLine(" trie par niveau de gemme poisson après");
                break;

            default:
               
                Console.WriteLine("Type de tri non reconnu");
                break;
        }
        UpdateSousListe();
    }
    public void TrierNom()
    {
        // Séparer les poissons et les gemmes
        List<PoissonData> poissons = ItemList.OfType<PoissonData>().ToList();
        List<GemmeData> gemmes = ItemList.OfType<GemmeData>().ToList();

        // Trier les poissons par nom (supposant que PoissonData a un champ 'nom')
        poissons = poissons.OrderBy(p => p.nom).ToList();

        // Réorganiser l'inventaire avec poissons d'abord, puis les gemmes
        ItemList = new List<ItemData>();
        ItemList.AddRange(poissons);
        ItemList.AddRange(gemmes);

        Debug.Log("Liste triée par nom de poisson.");
    }

    public void TrierParNiveau()
    {
        // Séparer les poissons et les gemmes
        List<PoissonData> poissons = ItemList.OfType<PoissonData>().ToList();
        List<GemmeData> gemmes = ItemList.OfType<GemmeData>().ToList();

        // Trier les gemmes par niveau
        gemmes = gemmes.OrderByDescending(g => g.LVL).ToList();

        // Réorganiser l'inventaire avec les gemmes d'abord, puis les poissons
        ItemList = new List<ItemData>();
        ItemList.AddRange(gemmes);
        ItemList.AddRange(poissons);

        Debug.Log("Liste triée par niveau de gemmes.");
    }

    public void TrierParType()
    {
        // Séparer les poissons et les gemmes
        List<PoissonData> poissons = ItemList.OfType<PoissonData>().ToList();
        List<GemmeData> gemmes = ItemList.OfType<GemmeData>().ToList();

        // Réorganiser l'inventaire avec les poissons d'abord, puis les gemmes
        ItemList = new List<ItemData>();
        ItemList.AddRange(poissons);
        ItemList.AddRange(gemmes);

        Debug.Log("Liste triée par type (poissons puis gemmes).");
    }

    public void TrierQuantiter()
    {
        // Trier les objets par quantité (qu'ils soient des poissons ou des gemmes)
        ItemList = ItemList.OrderByDescending(item => item.quantiter).ToList();

        Debug.Log("Liste triée par quantité.");
    }

    public void TrierNiveau()
    {
        // Séparer les poissons et les gemmes
        List<GemmeData> gemmes = ItemList.OfType<GemmeData>().ToList();
        List<PoissonData> poissons = ItemList.OfType<PoissonData>().ToList();

        // Trier uniquement les gemmes par niveau
        gemmes = gemmes.OrderByDescending(g => g.LVL).ToList();

        // Réorganiser l'inventaire avec les gemmes d'abord, puis les poissons
        ItemList = new List<ItemData>();
        ItemList.AddRange(gemmes);
        ItemList.AddRange(poissons);

        Debug.Log("Liste triée par niveau de gemmes.");
    }

    public void FusionAuto()
    {
        // On garde les gemmes intactes
        List<ItemData> gemmes = ItemList.Where(item => item is GemmeData).ToList();

        // On filtre les poissons avec quantité > 0
        List<PoissonData> poissons = ItemList.OfType<PoissonData>()
                                             .Where(poisson => poisson.quantiter > 0)
                                             .ToList();

        // On crée un dictionnaire pour compter et fusionner les poissons par nom
        Dictionary<string, (int quantiteTotale, int quantiterMax)> fusionPoissons = new Dictionary<string, (int, int)>();

        foreach (var poisson in poissons)
        {
            if (!fusionPoissons.ContainsKey(poisson.nom))
            {
                // On stocke la quantité totale et le quantiterMax
                fusionPoissons[poisson.nom] = (poisson.quantiter, poisson.quantiterMax);
            }
            else
            {
                // On ajoute la quantité au total déjà enregistré
                fusionPoissons[poisson.nom] = (fusionPoissons[poisson.nom].quantiteTotale + poisson.quantiter, poisson.quantiterMax);
            }
        }

        // Nouvelle liste des poissons fusionnés
        List<PoissonData> poissonsFusionnes = new List<PoissonData>();

        foreach (var entry in fusionPoissons)
        {
            string nomPoisson = entry.Key;
            int quantiteTotale = entry.Value.quantiteTotale;
            int quantiterMax = entry.Value.quantiterMax;

            // On répartit les poissons en respectant la quantité maximale propre à chaque poisson
            while (quantiteTotale > 0)
            {
                PoissonData nouveauPoisson = new PoissonData
                {
                    nom = nomPoisson,
                    quantiter = Math.Min(quantiterMax, quantiteTotale), // Utilisation de la valeur quantiterMax propre à ce poisson
                    quantiterMax = quantiterMax
                };
                poissonsFusionnes.Add(nouveauPoisson);
                quantiteTotale -= nouveauPoisson.quantiter;
            }
        }

        // Maintenant on replace tout dans ItemList
        // En gardant d'abord les poissons fusionnés, puis les gemmes
        ItemList = poissonsFusionnes.Cast<ItemData>()
                                    .Concat(gemmes)
                                    .ToList();
    }



}
