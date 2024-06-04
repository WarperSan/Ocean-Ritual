using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SocleGenerator : MonoBehaviour
{
    [SerializeField] GameObject ObjectToSocle;

    [SerializeField] string SideName = "";
    [SerializeField] string voidSpaceName ="";
    [SerializeField] string SoclePatch = "";
    [SerializeField] float spaceBetweenCube = 1f;
    [SerializeField] float SpacebetweenRectangleAndCube = 1f;
     Dictionary<string, GameObject> DictionarySocle = new();

    // Start is called before the first frame update
    void Start()
    {
        LoadSocleData();
        GenerateSocle(ObjectToSocle);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void GenerateSocle(GameObject ObjectSocle)
    {
        GemmeGrid scriptGemmeGrid = ObjectSocle.GetComponent<GemmeGrid>();
        if (scriptGemmeGrid == null)
        {
            Debug.LogError("GemmeGrid script not found on the object.");
            return;
        }

        if (!DictionarySocle.ContainsKey(voidSpaceName) || !DictionarySocle.ContainsKey(SideName))
        {
            Debug.LogError("Prefabs not found in the dictionary.");
            return;
        }

        GameObject voidPrefab = DictionarySocle[voidSpaceName];
        GameObject sidePrefab = DictionarySocle[SideName];

        // Instanciation des cubes avec l'espacement spécifié
        for (int i = 0; i < scriptGemmeGrid.width; i++)
        {
            for (int j = 0; j < scriptGemmeGrid.height; j++)
            {
                Vector3 position = new Vector3(i * spaceBetweenCube, 0, j * spaceBetweenCube);
                Instantiate(voidPrefab, position, Quaternion.identity, ObjectSocle.transform);
            }
        }

        float halfWidth = scriptGemmeGrid.width * spaceBetweenCube / 2.0f;
        float halfHeight = scriptGemmeGrid.height * spaceBetweenCube / 2.0f;

        // Instanciation des rectangles pour créer une grille, légèrement décalés par rapport aux cubes
        for (int i = 0; i <= scriptGemmeGrid.width; i++)
        {
            Vector3 position = new Vector3(i * spaceBetweenCube - spaceBetweenCube / 2, SpacebetweenRectangleAndCube, halfHeight - spaceBetweenCube / 2);
            GameObject sideInstance = Instantiate(sidePrefab, position, Quaternion.identity, ObjectSocle.transform);
            sideInstance.transform.localScale = new Vector3(1, 1, scriptGemmeGrid.height * spaceBetweenCube + spaceBetweenCube);
        }

        for (int j = 0; j <= scriptGemmeGrid.height; j++)
        {
            Vector3 position = new Vector3(halfWidth - spaceBetweenCube / 2, SpacebetweenRectangleAndCube, j * spaceBetweenCube - spaceBetweenCube / 2);
            GameObject sideInstance = Instantiate(sidePrefab, position, Quaternion.Euler(0, 90, 0), ObjectSocle.transform);
            sideInstance.transform.localScale = new Vector3(1, 1, scriptGemmeGrid.width * spaceBetweenCube + spaceBetweenCube);
        }
    }

    public void LoadSocleData()
    {
        DictionarySocle = DictionaryGenerator.DictionaryGameObjectGenerator(SoclePatch);
    }
}
