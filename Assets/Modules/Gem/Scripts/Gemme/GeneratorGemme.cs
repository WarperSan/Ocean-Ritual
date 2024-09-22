using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public   class GeneratorGemme: MonoBehaviour
    
{
    #region Fields and Properties
    // Singleton instance
    
    static bool dataLoad = false;
    static string GemmePath = "Gemme/AllGemme"; // Path to the gemme prefabs
    static string SampleGemmePath = "Gemme/SampleGemme"; // Path to the sample gemme prefab
    static string GemmeName = "Red"; // Default gemme name
    static float Spacebetween = 1f; // Space between gemmes
    static Dictionary<string, GameObject> DictionaryGemme = new(); // Dictionary to store gemme prefabs
    static private GameObject SampleGemme; // Sample gemme prefab
    static int lvlTest = 3; // Test level
    static int Hauteurgemme = 1; // Height of the gemme
    #endregion
    // Start is called before the first frame update
    
    
    #region Load Data
    // Function to load gemme data from resources
    public static void LoadGemmeData()
    {
        DictionaryGemme = DictionaryGenerator.DictionaryGameObjectGenerator(GemmePath);

        GameObject[] SampleObjects = Resources.LoadAll<GameObject>(SampleGemmePath);
       
        if (SampleObjects.Length != 0)
            SampleGemme = SampleObjects[0];
        
    }
    #endregion


    #region Gemme Creation

    // Function to create a random gemme
    public static void CreatGemmeRandomFunction(int lvlTests, string GemmeNames, Transform Conteneur)
    {
        if (!dataLoad)
        {
            LoadGemmeData();
            dataLoad = true;
        }
        CreatGemmeObject(GenerateRandomGemme(lvlTests, GemmeNames), Conteneur);
    }

   

    // Function to generate a random gemme
    public static Gemme? GenerateRandomGemme(int LVL, string ColorName)
    {
       

        Gemme gemmeScript = new Gemme();

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
    public static void GenerateStat(int LVL)
    {

    }
    // Function to generate the shape of the gemme based on the level
    private static FormeBool GenerateForme(int LVL)
    {
        int size = Mathf.CeilToInt(Mathf.Sqrt(LVL));
        if (size % 2 == 0)
        {
            size += 1;
        }

        while ((size * size - LVL) < 4)
        {
            size += 2;
        }

        bool[,] forme = new bool[size, size];
        int trueCount = 0;
        List<(int, int)> positions = new List<(int, int)>();

        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                positions.Add((i, j));
            }
        }

        System.Random rand = new System.Random();
        for (int i = 0; i < positions.Count; i++)
        {
            var temp = positions[i];
            int randomIndex = rand.Next(i, positions.Count);
            positions[i] = positions[randomIndex];
            positions[randomIndex] = temp;
        }

        for (int i = 0; i < LVL; i++)
        {
            var pos = positions[i];
            forme[pos.Item1, pos.Item2] = true;
            trueCount++;
        }

        return new FormeBool(forme, size, size);
    }

    // Function to create a gemme object in the scene
    public static GameObject? CreatGemmeObject(Gemme GemmeScript, Transform Conteneur)
    {
        if (!dataLoad)
        {
            LoadGemmeData();
            dataLoad = true;
        }
       

        GameObject instantiatedGemme = Instantiate(SampleGemme, Conteneur);
    
        if (instantiatedGemme != null)
            instantiatedGemme.GetComponent<GemmeComponant>().GemmeScript = GemmeScript;
        if (CreateMaterialGemme(instantiatedGemme))
            return instantiatedGemme;
        else
            return null;
    }



    // Function to create the material for the gemme object
    public static bool CreateMaterialGemme(GameObject instantiatedGemme)
    {
        if (!dataLoad)
        {
            LoadGemmeData();
            dataLoad = true;
        }

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

        float space = SocleGenerator.Instance.spaceBetweenCube;

        // 1. Calcul de la taille totale du tableau
        float totalWidth = forme.width * space;
        float totalHeight = forme.height * space;

        // 2. Calcul de l'offset pour centrer instantiatedGemme
        Vector3 positionOffset = new Vector3(gemmeScript.PositionX * space  , 0, gemmeScript.PositionZ * space );

        // 3. Ajuster la position d'instantiatedGemme (enlever si tu ne veux pas que le centre soit impacté)
        instantiatedGemme.transform.localPosition = positionOffset;
      
        // Calcul du centre de la forme
        int centreX = Mathf.FloorToInt(forme.width / 2.0f);
        int centreY = Mathf.FloorToInt(forme.height / 2.0f);

        for (int i = 0; i < forme.height; i++)
        {
            for (int j = 0; j < forme.width; j++)
            {
                if (boolArray[i, j])
                {
                    // Positionner chaque cube par rapport au centre de la forme
                    Vector3 localPosition = new Vector3((i - centreY) * space, Hauteurgemme, (j - centreX) * space);
                    GameObject gemmeCube = Instantiate(prefabToInstantiate, instantiatedGemme.transform);
                    gemmeCube.transform.localPosition = localPosition;
                    gemmeCube.transform.localScale += new Vector3(space - 2, 0, space - 2);
                }
            }
        }

        return true;
    }

    #endregion


}