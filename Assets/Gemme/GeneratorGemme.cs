using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneratorGemme : MonoBehaviour
{
    public static GeneratorGemme Instance { get; private set; }
    [SerializeField] string GemmePath = "Gemme/AllGemme";
    [SerializeField] string SampleGemmePath = "Gemme/SampleGemme";
    [SerializeField] string GemmeName = "Red";
    [SerializeField] float Spacebetween = 1f;
    Dictionary<string,GameObject> DictionaryGemme = new();
    private GameObject SampleGemme;
    [SerializeField] int lvlTest = 3;
    [SerializeField] int Hauteurgemme = 1;
    // Start is called before the first frame update
    void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogError("Multiple instances of SocleGenerator detected. Destroying the new instance.");
            Destroy(gameObject);
        }
        LoadGemmeData();
        // CreatGemmeRandomFunction(lvlTest, GemmeName);


    }
    public void CreatGemmeRandomFunction(int lvlTests,string GemmeNames)
    {
         CreatGemmeObject(GenerateRandomGemme(lvlTests, GemmeNames),this.transform);
    }
    public void LoadGemmeData()
    {
        DictionaryGemme = DictionaryGenerator.DictionaryGameObjectGenerator(GemmePath);
       
        GameObject[] SampleObjects = Resources.LoadAll<GameObject>(SampleGemmePath);
        if (SampleObjects.Length != 0)
            SampleGemme = SampleObjects[0];



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
            gemmeScript.PositionX = 3;
            gemmeScript.PositionZ = 3;
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


    public GameObject? CreatGemmeObject(Gemme GemmeScript,Transform Conteneur)
    {

        
        GameObject instantiatedGemme = Instantiate(SampleGemme,  Conteneur);
        if(instantiatedGemme != null) 
        instantiatedGemme.GetComponent<GemmeComponant>().GemmeScript = GemmeScript;
      if( CreateMaterialGemme(instantiatedGemme))
        return instantiatedGemme;
      else 
            return null;

    }
    public GameObject? CreatGemmeObject(Gemme GemmeScript, Transform Conteneur,int A)
    {


        GameObject instantiatedGemme = Instantiate(SampleGemme, Conteneur);
        if (instantiatedGemme != null)
            instantiatedGemme.GetComponent<GemmeComponant>().GemmeScript = GemmeScript;
        if (CreateMaterialGemme(instantiatedGemme))

            return instantiatedGemme;
        else
            return null;

    }
    public bool CreateMaterialGemme(GameObject instantiatedGemme)
    {
        GemmeComponant scriptGemmeComponant = instantiatedGemme.GetComponent<GemmeComponant>();
        Gemme gemmeScript = scriptGemmeComponant.GemmeScript;

        if (gemmeScript == null || !DictionaryGemme.ContainsKey(gemmeScript.GemmeColorsName))
        {
            Debug.LogError("Gemme script is null or Gemme color not found in dictionary.");
            return false;
        }

        GameObject prefabToInstantiate = DictionaryGemme[gemmeScript.GemmeColorsName];
        FormeBool forme = gemmeScript.forme;
        bool[,] boolArray = forme.GetForme();

        int centreX = forme.width / 2;
        int centreY = forme.height / 2;

        for (int i = 0; i < forme.height; i++)
        {
            for (int j = 0; j < forme.width; j++)
            {
                if (boolArray[i, j])
                {
                    float Space = SocleGenerator.Instance.spaceBetweenCube;

                    // Position calculée relative au parent (instantiatedGemme)
                    Vector3 localPosition = new Vector3((i - centreY) * Space, Hauteurgemme, (j - centreX) * Space);
                  //  Debug.Log("Local Position: " + localPosition);

                    // Instanciation avec la position relative et le parent
                    GameObject GemmeCube = Instantiate(prefabToInstantiate, instantiatedGemme.transform);
                    GemmeCube.transform.localPosition = localPosition; // Utilisez localPosition pour placer l'objet correctement par rapport au parent
                    GemmeCube.transform.localScale += new Vector3(Space - 2, 0, Space - 2);
                 //   Debug.Log("World Position: " + GemmeCube.transform.position);
                }
            }
        }
        return true;
    }


}
