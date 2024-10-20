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
    static string GemmePath = "Gemme/AllGemme"; // Path to the gems prefabs
    static string SampleGemmePath = "Gemme/SampleGemme"; // Path to the sample gems prefab
   // static string GemmeName = "Red"; // Default gems name
    //static float Spacebetween = 1f; // Space between gemmes
    static Dictionary<string, GameObject> DictionaryGemme = new(); // Dictionary to store gems prefabs
    static private GameObject SampleGemme; // Sample gems prefab
    //static int lvlTest = 3; // Test level
    static int height = 1; // Height of the gems

    // Start is called before the first frame update
    #endregion

    #region Load Data
    // Function to load gems data from resources
    public static void LoadGemmeData()
    {
        DictionaryGemme = DictionaryGenerator.DictionaryGameObjectGenerator(GemmePath);

        GameObject[] SampleObjects = Resources.LoadAll<GameObject>(SampleGemmePath);
       
        if (SampleObjects.Length != 0)
            SampleGemme = SampleObjects[0];
        
    }
    #endregion


    #region Gemme Creation

    // Function to create a random gems
    //public static void CreatGemmeRandomFunction(int lvlTests, string GemmeNames, Transform Conteneur)
    //{
    //    if (!dataLoad)
    //    {
    //        LoadGemmeData();
    //        dataLoad = true;
    //    }
    //    CreatGemmeObject(GenerateRandomGemme(lvlTests, GemmeNames), Conteneur);
    //}

   

    // Function to generate a random gems
    public static GemData GenerateRandomGemme(int LVL)
    {


        GemData gemmeScript = new GemData();

        if (gemmeScript != null)
        {
            gemmeScript.quantityMax = 1;
            gemmeScript.quantity = 1;
            gemmeScript.Shape = GenerateForme(LVL);
            gemmeScript.GemColorsName = GeneratesRandomlColors();
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

    //si on appele cela inclu que a gemme est fusionné
    public static GemData GenerateRandomGemme(int LVL, GemData gemHeritage )
    {


        GemData gemmeScript = new GemData();

        if (gemmeScript != null)
        {
            if(LVL>= gemHeritage.LVL)
            {
                gemmeScript.quantityMax = 1;
                gemmeScript.quantity = 1;
                gemmeScript.Shape = GenerateForme(gemHeritage.Shape);
                gemmeScript.GemColorsName = gemHeritage.GemColorsName;
                gemmeScript.LVL = LVL;
                gemmeScript.typeWeapon = GeneratesRandomlvlStat<TypeWeapon>(LVL);
                gemmeScript.typeBoat = GeneratesRandomlvlStat<TypeBoat>(LVL);
                gemmeScript.typeNet = GeneratesRandomlvlStat<TypeNet>(LVL);
                return gemmeScript;
            }
            else
            {
                gemmeScript.quantityMax = 1;
                gemmeScript.quantity = 1;
                gemmeScript.Shape =gemHeritage.Shape;
                gemmeScript.GemColorsName = gemHeritage.GemColorsName;
                gemmeScript.LVL = LVL;
                gemmeScript.typeWeapon = GeneratesRandomlvlStat<TypeWeapon>(LVL);
                gemmeScript.typeBoat = GeneratesRandomlvlStat<TypeBoat>(LVL);
                gemmeScript.typeNet = GeneratesRandomlvlStat<TypeNet>(LVL);
                return gemmeScript;
            }
           
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




    // Méthode générique qui génère une liste de TypeQuantity<T> où chaque quantité est égale au niveau LVL
    public static List<TypeQuantity<T>> GeneratesRandomlvlStat<T>(int LVL) where T : Enum
    {
        // Obtenir tous les types disponibles dans l'énumération T
        T[] enumValues = (T[])Enum.GetValues(typeof(T));

        // Créer une liste pour stocker les résultats
        List<TypeQuantity<T>> resultList = new List<TypeQuantity<T>>();

        // Boucle pour ajouter chaque type avec la quantité LVL
        foreach (T enumValue in enumValues)
        {
            // Ajouter la paire (type, quantité) dans la liste, avec la quantité égale à LVL
            resultList.Add(new TypeQuantity<T>(enumValue, GenerateStats( LVL)));
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
    // Function to generate the shape of the gems based on the level
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
    private static FormBool GenerateForme(FormBool gemForm)
    {
        // Extraction des données de la forme existante
        List<bool> existingFormFlat = gemForm.flatForme;
        int rows = gemForm.height;
        int cols = gemForm.width;
        int trueCount = 0;

        // Compter les cases 'true' et vérifier si des espaces 'false' sont disponibles
        List<int> falsePositions = new List<int>();
        for (int i = 0; i < existingFormFlat.Count; i++)
        {
            if (existingFormFlat[i])
            {
                trueCount++;
            }
            else
            {
                falsePositions.Add(i);
            }
        }

     
       

        // Si l'on a de l'espace disponible, activer une case aléatoire qui est actuellement 'false'
        if (falsePositions.Count > 0)
        {
            System.Random rand = new System.Random();
            int randomIndex = falsePositions[rand.Next(falsePositions.Count)];
            existingFormFlat[randomIndex] = true;
        }
        else
        {
            // Si plus d'espace, agrandir la forme
            int newSize = Mathf.CeilToInt(Mathf.Sqrt(trueCount));
            if (newSize % 2 == 0)
            {
                newSize += 1;
            }

            while ((newSize * newSize - trueCount) < 4)
            {
                newSize += 2;
            }

            // Créer une nouvelle grille de la nouvelle taille
            List<bool> newForm = new List<bool>(newSize * newSize);
            for (int i = 0; i < newSize * newSize; i++)
            {
                newForm.Add(false);
            }

            // Copier l'ancienne forme centrée dans la nouvelle liste
            int rowOffset = (newSize - rows) / 2;
            int colOffset = (newSize - cols) / 2;

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    newForm[(i + rowOffset) * newSize + (j + colOffset)] = existingFormFlat[i * cols + j];
                }
            }


          

            // Ajouter un 'true' à une position aléatoire dans la nouvelle forme agrandie
            List<int> newPositions = new List<int>();
            for (int i = 0; i < newForm.Count; i++)
            {
                if (!newForm[i])
                {
                    newPositions.Add(i);
                }
            }

            System.Random rand = new System.Random();
            int randomNewPosIndex = newPositions[rand.Next(newPositions.Count)];
            newForm[randomNewPosIndex] = true;

            return new FormBool(newForm, newSize, newSize);
        }

        // Si l'on a activé un index aléatoire, renvoyer la forme modifiée
        return new FormBool(existingFormFlat, cols, rows);
    }

    

   


    // Function to create a gems object in the scene
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



    // Function to create the material for the gems object
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