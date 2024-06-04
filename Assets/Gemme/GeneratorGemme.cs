using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneratorGemme : MonoBehaviour
{
    [SerializeField] string GemmePath = "Gemme/AllGemme";
    [SerializeField] string SampleGemmePath = "Gemme/SampleGemme";
    [SerializeField] string GemmeName = "Red";
    [SerializeField] float Spacebetween = 1f;
    Dictionary<string,GameObject> DictionaryGemme = new();
    private GameObject SampleGemme;
    [SerializeField] int lvlTest = 3;
    // Start is called before the first frame update
    void Start()
    {
        LoadGemmeData();
        CreatGemmeObject(GenerateRandomGemme(lvlTest, GemmeName));
      
    }
    public void LoadGemmeData()
    {
        // Charger tous les GameObjects à partir du dossier spécifié
        GameObject[] loadedObjects = Resources.LoadAll<GameObject>(GemmePath);
        GameObject[] SampleObjects = Resources.LoadAll<GameObject>(SampleGemmePath);
        if (SampleObjects.Length != 0)
            SampleGemme = SampleObjects[0];



        foreach (GameObject obj in loadedObjects)
        {


            // Vérifier si le dictionnaire ne contient pas déjà ce nom
            if (!DictionaryGemme.ContainsKey(obj.name))
            {
                // Ajouter l'objet au dictionnaire
                DictionaryGemme.Add(obj.name, obj);
            }
        }
        // Afficher les noms des objets dans le dictionnaire
        foreach (KeyValuePair<string, GameObject> kvp in DictionaryGemme)
        {
           // Debug.Log(kvp.Key);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }


    public Gemme? GenerateRandomGemme(int LVL,string ColorName)
    {
        // Charger l'objet à partir de SampleGemmePath
       
        if (SampleGemme == null)
        {
            Debug.LogError($"Sample gemme prefab not found at path: {SampleGemmePath}");
            return null;
        }

        // Instancier l'objet dans SampleGemmePath
        

        // Obtenir le composant Gemme du prefab instancié
        Gemme gemmeScript =new Gemme();

        if (gemmeScript != null)
        {
            gemmeScript.forme = GenerateForme(LVL);
            gemmeScript.GemmeColorsName = ColorName;
            gemmeScript.LVL = LVL;
            return gemmeScript;

        }
        else
        {
            Debug.LogError("Script Gemme Not instanciate");
        }

        return null;
    }


    private FormeBool GenerateForme(int LVL)
    {
        // Calculer la taille minimale du tableau (doit être un carré impair)
        int size = Mathf.CeilToInt(Mathf.Sqrt(LVL));
        if (size % 2 == 0)
        {
            size += 1;
        }

        // Augmenter la taille si nécessaire pour avoir suffisamment de cases non vraies
        while ((size * size - LVL) < 4)
        {
            size += 2;
        }

        // Créer et remplir le tableau
        bool[,] forme = new bool[size, size];
        int trueCount = 0;
        List<(int, int)> positions = new List<(int, int)>();

        // Remplir une liste de toutes les positions possibles
        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                positions.Add((i, j));
            }
        }

        // Mélanger les positions pour une sélection aléatoire
        System.Random rand = new System.Random();
        for (int i = 0; i < positions.Count; i++)
        {
            var temp = positions[i];
            int randomIndex = rand.Next(i, positions.Count);
            positions[i] = positions[randomIndex];
            positions[randomIndex] = temp;
        }

        // Assigner des true aléatoirement jusqu'à atteindre le LVL
        for (int i = 0; i < LVL; i++)
        {
            var pos = positions[i];
            forme[pos.Item1, pos.Item2] = true;
            trueCount++;
        }

        return new FormeBool(forme, size, size);
    }


    public void CreatGemmeObject(Gemme GemmeScript)
    {

        
        GameObject instantiatedGemme = Instantiate(SampleGemme, transform.position, transform.rotation);
        if(instantiatedGemme != null) 
        instantiatedGemme.GetComponent<GemmeComponant>().GemmeScript = GemmeScript;
        CreateMaterialGemme(instantiatedGemme);
        //return instantiatedGemme;

    }
    public void CreateMaterialGemme(GameObject instantiatedGemme)
    {
        GemmeComponant scriptGemmeComponant = instantiatedGemme.GetComponent<GemmeComponant>();
        Gemme gemmeScript = scriptGemmeComponant.GemmeScript;

        if (gemmeScript == null || !DictionaryGemme.ContainsKey(gemmeScript.GemmeColorsName))
        {
            Debug.LogError("Gemme script is null or Gemme color not found in dictionary.");
            return;
        }

        GameObject prefabToInstantiate = DictionaryGemme[gemmeScript.GemmeColorsName];
        FormeBool forme = gemmeScript.forme;

        for (int i = 0; i < forme.height; i++)
        {
            for (int j = 0; j < forme.width; j++)
            {
                if (forme.GetForme()[i, j])
                {
                    Vector3 position = new Vector3(i * Spacebetween, 0, j * Spacebetween);
                    Instantiate(prefabToInstantiate, position, Quaternion.identity, instantiatedGemme.transform);
                }
            }
        }
    }


}
