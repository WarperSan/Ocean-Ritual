using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static EnumGeneral;

public   class GeneratorGemme: MonoBehaviour
    
{

    #region StatPropritite

    private static List<int> LuckLevels = new List<int> { 20, 10, 5 };


    #endregion


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

    // Start is called before the first frame update
    #endregion

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
    //public static void CreatGemmeRandomFunction(int lvlTests, string GemmeNames, Transform Conteneur)
    //{
    //    if (!dataLoad)
    //    {
    //        LoadGemmeData();
    //        dataLoad = true;
    //    }
    //    CreatGemmeObject(GenerateRandomGemme(lvlTests, GemmeNames), Conteneur);
    //}

   

    // Function to generate a random gemme
    public static GemmeData GenerateRandomGemme(int LVL)
    {


        GemmeData gemmeScript = new GemmeData();

        if (gemmeScript != null)
        {
            gemmeScript.quantityMax = 1;
            gemmeScript.quantity = 1;
            gemmeScript.Shape = GenerateForme(LVL);
            gemmeScript.GemmeColorsName = GeneratsRandomlColors();
            gemmeScript.LVL = LVL;
            gemmeScript.typeWeapon = GeneratsRandomlvlStat<TypeWeapon>(LVL);
            gemmeScript.typeBoat = GeneratsRandomlvlStat<TypeBoat>(LVL);
            gemmeScript.typeNet = GeneratsRandomlvlStat<TypeNet>(LVL);
            return gemmeScript;
        }
        else
        {
            Debug.LogError("Script Gemme Not instanciate");
        }

        return null;
    }
    public static int GenerateStats(int LVL)
    {
        

        int stat = LVL;

        for (int i = 0;i < LuckLevels.Count; i++)
        {
            int luck = LuckLevels[i];
            int chance = UnityEngine.Random.Range(1, 101);

            if (chance <= luck)
            {
                stat += LVL;
            }
            else
            {
                break;
            }
        }

        return stat;
    }




    // Méthode générique qui génère une liste de TypeQuantite<T> où chaque quantité est égale au niveau LVL
    public static List<TypeQuantite<T>> GeneratsRandomlvlStat<T>(int LVL) where T : Enum
    {
        // Obtenir tous les types disponibles dans l'énumération T
        T[] enumValues = (T[])Enum.GetValues(typeof(T));

        // Créer une liste pour stocker les résultats
        List<TypeQuantite<T>> resultList = new List<TypeQuantite<T>>();

        // Boucle pour ajouter chaque type avec la quantité LVL
        foreach (T enumValue in enumValues)
        {
            // Ajouter la paire (type, quantité) dans la liste, avec la quantité égale à LVL
            resultList.Add(new TypeQuantite<T>(enumValue, GenerateStats( LVL)));
        }

        return resultList;
    }
    public static string GeneratsRandomlColors()
    {
        // Récupérer tous les noms de l'énumération ColorsName
        Array colors = Enum.GetValues(typeof(EnumGeneral.ColorsName));

        // Créer une instance de Random pour sélectionner un élément aléatoire
        System.Random random = new System.Random();

        // Sélectionner un index aléatoire dans la liste des couleurs
        int randomIndex = random.Next(colors.Length);

        // Retourner le name de la couleur sélectionnée aléatoirement
        return colors.GetValue(randomIndex).ToString();
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
      
        // Calcul du centre de la Shape
        int centreX = Mathf.FloorToInt(forme.width / 2.0f);
        int centreY = Mathf.FloorToInt(forme.height / 2.0f);

        for (int i = 0; i < forme.height; i++)
        {
            for (int j = 0; j < forme.width; j++)
            {
                if (boolArray[i, j])
                {
                    // Positionner chaque cube par rapport au centre de la Shape
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