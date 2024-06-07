using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneratorGemme : MonoBehaviour
{
    // Singleton instance
    public static GeneratorGemme Instance { get; private set; }

    [SerializeField] string GemmePath = "Gemme/AllGemme"; // Path to the gemme prefabs
    [SerializeField] string SampleGemmePath = "Gemme/SampleGemme"; // Path to the sample gemme prefab
    [SerializeField] string GemmeName = "Red"; // Default gemme name
    [SerializeField] float Spacebetween = 1f; // Space between gemmes
    Dictionary<string, GameObject> DictionaryGemme = new(); // Dictionary to store gemme prefabs
    private GameObject SampleGemme; // Sample gemme prefab
    [SerializeField] int lvlTest = 3; // Test level
    [SerializeField] int Hauteurgemme = 1; // Height of the gemme

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
    #region Load Data
    // Function to load gemme data from resources
    public void LoadGemmeData()
    {
        DictionaryGemme = DictionaryGenerator.DictionaryGameObjectGenerator(GemmePath);

        GameObject[] SampleObjects = Resources.LoadAll<GameObject>(SampleGemmePath);
        if (SampleObjects.Length != 0)
            SampleGemme = SampleObjects[0];
    }
    #endregion


    #region Gemme Creation

    // Function to create a random gemme
    public void CreatGemmeRandomFunction(int lvlTests, string GemmeNames)
    {
        CreatGemmeObject(GenerateRandomGemme(lvlTests, GemmeNames), this.transform);
    }

   

    // Function to generate a random gemme
    public Gemme? GenerateRandomGemme(int LVL, string ColorName)
    {
        if (SampleGemme == null)
        {
            Debug.LogError($"Sample gemme prefab not found at path: {SampleGemmePath}");
            return null;
        }

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

    // Function to generate the shape of the gemme based on the level
    private FormeBool GenerateForme(int LVL)
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
    public GameObject? CreatGemmeObject(Gemme GemmeScript, Transform Conteneur)
    {
        GameObject instantiatedGemme = Instantiate(SampleGemme, Conteneur);
        if (instantiatedGemme != null)
            instantiatedGemme.GetComponent<GemmeComponant>().GemmeScript = GemmeScript;
        if (CreateMaterialGemme(instantiatedGemme))
            return instantiatedGemme;
        else
            return null;
    }

    // Overloaded function to create a gemme object in the scene with additional parameter
    public GameObject? CreatGemmeObject(Gemme GemmeScript, Transform Conteneur, int A)
    {
        GameObject instantiatedGemme = Instantiate(SampleGemme, Conteneur);
        if (instantiatedGemme != null)
            instantiatedGemme.GetComponent<GemmeComponant>().GemmeScript = GemmeScript;
        if (CreateMaterialGemme(instantiatedGemme))
            return instantiatedGemme;
        else
            return null;
    }

    // Function to create the material for the gemme object
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

                    Vector3 localPosition = new Vector3((i - centreY) * Space, Hauteurgemme, (j - centreX) * Space);
                    GameObject GemmeCube = Instantiate(prefabToInstantiate, instantiatedGemme.transform);
                    GemmeCube.transform.localPosition = localPosition;
                    GemmeCube.transform.localScale += new Vector3(Space - 2, 0, Space - 2);
                }
            }
        }
        return true;
    }

    #endregion

    // Update is called once per frame
    void Update()
    {

    }
}