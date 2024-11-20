using System.Collections.Generic;
using UnityEngine;

public class SocleGenerator : MonoBehaviour
{
    #region Fields and Properties
    // Singleton instance
    public static SocleGenerator Instance { get; private set; }

    // Serialized fields for object references and settings
    

    [SerializeField] string SideName = "";
    [SerializeField] string voidSpaceName = "";
    [SerializeField] string SoclePatch = "";
    [SerializeField] public float spaceBetweenCube = 3f;
    [SerializeField] float SpacebetweenRectangleAndCube = 1f;

    // Dictionary to store loaded prefabs
    Dictionary<string, GameObject> DictionarySocle = new();
#endregion
    // Unity start method
    void Start()
    {
        if (Instance == null)
        {
            Instance = this; // Set the singleton instance
        }
        else
        {
            Debug.LogError("Multiple instances of SocleGenerator detected. Destroying the new instance.");
            Destroy(gameObject); // Destroy the new instance if one already exists
        }
        LoadSocleData(); // Load socle data
        
    }


    void Update() { }

    #region Socle Generation

    // Method to generate socle/grid
    public void GenerateSocle(GameObject ObjectSocle, GameObject Conteneur)
    {
       
        // Get the GemmeGrid component from the ObjectSocle
        GemmeGrid scriptGemmeGrid = ObjectSocle.GetComponent<GemmeGrid>();

        if (scriptGemmeGrid == null)
        {
            Debug.LogError("GemmeGrid script not found on the object.");
            return;
        }

        // Check if required prefabs are in the dictionary
        if (!DictionarySocle.ContainsKey(voidSpaceName) || !DictionarySocle.ContainsKey(SideName))
        {
            Debug.LogError("Prefabs not found in the dictionary.");
            return;
        }

        // Get the prefabs from the dictionary
        GameObject voidPrefab = DictionarySocle[voidSpaceName];
        GameObject sidePrefab = DictionarySocle[SideName];

        // Instantiate void spaces (grid cells)
        for (int i = 0; i < scriptGemmeGrid.width; i++)
        {
            for (int j = 0; j < scriptGemmeGrid.height; j++)
            {
                Vector3 position = new Vector3(i * spaceBetweenCube, 0, j * spaceBetweenCube);
                GameObject instance = Instantiate(voidPrefab, Conteneur.transform);
                LocationSocle scritpGemme = instance.GetComponent<LocationSocle>();
                if (scritpGemme != null)
                {
                    scritpGemme.x = i;
                    scritpGemme.z = j;
                }
                else
                {
                    Debug.Log("scritpGemme est null dans Socle générator fonction GenerateSocle");
                }
                instance.transform.localPosition += position; // Adjust position
                instance.transform.localScale += new Vector3(spaceBetweenCube - 2, 0, spaceBetweenCube - 2); // Adjust scale
            }
        }

        // Calculate half dimensions for positioning the sides
        float halfWidth = scriptGemmeGrid.width * spaceBetweenCube / 2.0f;
        float halfHeight = scriptGemmeGrid.height * spaceBetweenCube / 2.0f;

        // Instantiate side rectangles along the grid (vertical)
        for (int i = 0; i <= scriptGemmeGrid.width; i++)
        {
            Vector3 position = new Vector3(i * spaceBetweenCube - spaceBetweenCube / 2, SpacebetweenRectangleAndCube, halfHeight - spaceBetweenCube / 2);
            GameObject sideInstance = Instantiate(sidePrefab, Conteneur.transform);
            sideInstance.transform.localPosition += position; // Adjust position
            sideInstance.transform.localScale = new Vector3(1, 1, scriptGemmeGrid.height * spaceBetweenCube + spaceBetweenCube); // Adjust scale
        }

        // Instantiate side rectangles along the grid (horizontal)
        for (int j = 0; j <= scriptGemmeGrid.height; j++)
        {
            Vector3 position = new Vector3(halfWidth - spaceBetweenCube / 2, SpacebetweenRectangleAndCube, j * spaceBetweenCube - spaceBetweenCube / 2);
            GameObject sideInstance = Instantiate(sidePrefab, Conteneur.transform);
            sideInstance.transform.localPosition += position; // Adjust position
            sideInstance.transform.localRotation *= Quaternion.Euler(0, 90, 0); // Rotate the side
            sideInstance.transform.localScale = new Vector3(1, 1, scriptGemmeGrid.width * spaceBetweenCube + spaceBetweenCube); // Adjust scale
        }
    }

    #endregion

    #region Data Loading

    // Method to load socle data
    public void LoadSocleData()
    {
        DictionarySocle = DictionaryGenerator.DictionaryGameObjectGenerator(SoclePatch); // Load data into the dictionary
    }

    #endregion
}