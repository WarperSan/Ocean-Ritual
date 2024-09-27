using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static EnumGeneral;

public   class GeneratorGem: MonoBehaviour
    
{

    #region StatPropritite

    private static List<int> LuckLevels = new List<int> { 20, 10, 5 };


    #endregion


    #region Fields and Properties
    // Singleton instance

    static bool dataLoad = false;
    static string GemmePath = "Gemme/AllGemme"; // Path to the gem prefabs
    static string SampleGemmePath = "Gemme/SampleGemme"; // Path to the sample gem prefab
   // static string GemmeName = "Red"; // Default gem name
    //static float Spacebetween = 1f; // Space between gemmes
    static Dictionary<string, GameObject> DictionaryGemme = new(); // Dictionary to store gem prefabs
    static private GameObject SampleGemme; // Sample gem prefab
    //static int lvlTest = 3; // Test level
    static int height = 1; // Height of the gem

    // Start is called before the first frame update
    #endregion

    #region Load Data
    // Function to load gem data from resources
    public static void LoadGemmeData()
    {
        DictionaryGemme = DictionaryGenerator.DictionaryGameObjectGenerator(GemmePath);

        GameObject[] SampleObjects = Resources.LoadAll<GameObject>(SampleGemmePath);
       
        if (SampleObjects.Length != 0)
            SampleGemme = SampleObjects[0];
        
    }
    #endregion


    #region Gemme Creation

    // Function to create a random gem
    //public static void CreatGemmeRandomFunction(int lvlTests, string GemmeNames, Transform Conteneur)
    //{
    //    if (!dataLoad)
    //    {
    //        LoadGemmeData();
    //        dataLoad = true;
    //    }
    //    CreatGemmeObject(GenerateRandomGemme(lvlTests, GemmeNames), Conteneur);
    //}

   

    // Function to generate a random gem
    public static GemmeData GenerateRandomGemme(int LVL)
    {


        GemmeData gemmeScript = new GemmeData();

        if (gemmeScript != null)
        {
            gemmeScript.quantityMax = 1;
            gemmeScript.quantity = 1;
            gemmeScript.Shape = GenerateForme(LVL);
            gemmeScript.GemmeColorsName = GeneratesRandomlColors();
            gemmeScript.LVL = LVL;
            gemmeScript.typeWeapon = GeneratesRandomlvlStat<TypeWeapon>(LVL);
            gemmeScript.typeBoat = GeneratesRandomlvlStat<TypeBoat>(LVL);
            gemmeScript.typeNet = GeneratesRandomlvlStat<TypeNet>(LVL);
            return gemmeScript;
        }
        else
        {
            Debug.LogError("Script Gem Not instanciate");
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
    public static List<TypeQuantite<T>> GeneratesRandomlvlStat<T>(int LVL) where T : Enum
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
    public static string GeneratesRandomlColors()
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
    // Function to generate the shape of the gem based on the level
    private static FormBool GenerateForme(int LVL)
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

        bool[,] form = new bool[size, size];
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
            form[pos.Item1, pos.Item2] = true;
            trueCount++;
        }

        return new FormBool(form, size, size);
    }

    // Function to create a gem object in the scene
    public static GameObject? CreatGemmeObject(Gem GemmeScript, Transform Conteneur)
    {
        if (!dataLoad)
        {
            LoadGemmeData();
            dataLoad = true;
        }

        
        GameObject instantiatedGemme = Instantiate(SampleGemme, Conteneur);
    
        if (instantiatedGemme != null)
            instantiatedGemme.GetComponent<Gemcomponent>().GemScript = GemmeScript;
        if (CreateMaterialGemme(instantiatedGemme))
            return instantiatedGemme;
        else
            return null;
    }



    // Function to create the material for the gem object
    public static bool CreateMaterialGemme(GameObject instantiatedGemme)
    {
        if (!dataLoad)
        {
            LoadGemmeData();
            dataLoad = true;
        }

        Gemcomponent scriptGemmecomponent = instantiatedGemme.GetComponent<Gemcomponent>();
        Gem gemmeScript = scriptGemmecomponent.GemScript;

        if (gemmeScript == null || !DictionaryGemme.ContainsKey(gemmeScript.GemColorsName))
        {
            Debug.LogError("Gem script is null or Gem color not found in dictionary.");
            return false;
        }

        GameObject prefabToInstantiate = DictionaryGemme[gemmeScript.GemColorsName];
        FormBool form = gemmeScript.form;
        bool[,] boolArray = form.GetForme();

        float space = SocleGenerator.Instance.spaceBetweenCube;

        // 1. Calcul de la taille totale du Grid
        float totalWidth = form.width * space;
        float totalHeight = form.height * space;

        // 2. Calcul de l'offset pour centrer instantiatedGemme
        Vector3 positionOffset = new Vector3(gemmeScript.PositionX * space  , 0, gemmeScript.PositionZ * space );

        // 3. Ajuster la position d'instantiatedGemme (enlever si tu ne veux pas que le centre soit impacté)
        instantiatedGemme.transform.localPosition = positionOffset;
      
        // Calcul du centre de la Shape
        int centreX = Mathf.FloorToInt(form.width / 2.0f);
        int centreY = Mathf.FloorToInt(form.height / 2.0f);

        for (int i = 0; i < form.height; i++)
        {
            for (int j = 0; j < form.width; j++)
            {
                if (boolArray[i, j])
                {
                    // Positionner chaque cube par rapport au centre de la Shape
                    Vector3 localPosition = new Vector3((i - centreY) * space, height, (j - centreX) * space);
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