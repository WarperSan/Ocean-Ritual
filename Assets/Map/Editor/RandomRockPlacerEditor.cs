using UnityEngine;
using UnityEditor;

namespace MapModule
{
    [InitializeOnLoad]
    public class RandomRockPlacerEditor
    {
        private const string ReferencePath = "Assets/Map/Prefabs/Rock.prefab";
        private const string RocksResourcesPath = "MapModule/Rocks";
        
        private static GameObject referencePrefab;
        private static GameObject[] rockPrefabs;

        static RandomRockPlacerEditor()
        {
            referencePrefab = AssetDatabase.LoadAssetAtPath<GameObject>(ReferencePath);
            
            if (referencePrefab != null)
            {
                //Debug.Log("Prefab de référence chargé avec succès : " + referencePrefab.name);
            }
            else
            {
                Debug.LogError("Échec du chargement du prefab de référence. Chemin incorrect : " + ReferencePath);
            }

            rockPrefabs = Resources.LoadAll<GameObject>(RocksResourcesPath);
            if (rockPrefabs.Length > 0)
            {
                //Debug.Log("Prefabs de rochers chargés avec succès. Nombre de prefabs : " + rockPrefabs.Length);
            }
            else
            {
                Debug.LogError("Aucun prefab trouvé dans le dossier Resources/Rock");
            }

            // Hook into the scene's drag-and-drop event
            SceneView.duringSceneGui += OnSceneGUI;
        }

        private static void OnSceneGUI(SceneView sceneView)
        {
            Event e = Event.current;

            if (e.type == EventType.DragPerform && DragAndDrop.objectReferences.Length > 0)
            {
                // Get the current drag and drop object
                Object obj = DragAndDrop.objectReferences[0];
                GameObject go = obj as GameObject;


                // Check if the object being dragged is the specific prefab
                if (go != null && PrefabUtility.GetPrefabAssetType(go) != PrefabAssetType.NotAPrefab && go == referencePrefab)
                {
            

                
                    GameObject randomRockPrefab = rockPrefabs[Random.Range(0, rockPrefabs.Length)];
                

                
                    Ray worldRay = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);
                    if (Physics.Raycast(worldRay, out RaycastHit hit))
                    {
                    
                        GameObject instantiatedRock = PrefabUtility.InstantiatePrefab(randomRockPrefab) as GameObject;
                        Vector3 position = hit.point;
                        position.y += 1; 
                        instantiatedRock.transform.position = position;
                        ApplyRandomTransform(instantiatedRock);
                        Undo.RegisterCreatedObjectUndo(instantiatedRock, "Create Random Rock");
                    }

                    // Marquer l'événement comme utilisé
                    DragAndDrop.AcceptDrag();
                    Event.current.Use();
                }
            }
        }

        private static void ApplyRandomTransform(GameObject obj)
        {
            Vector2 scaleRange = new Vector2(0.5f, 1.5f);
            Vector2 rotationRange = new Vector2(0, 360);

            // Apply random scale
            float randomScale = Random.Range(scaleRange.x, scaleRange.y);
            obj.transform.localScale = new Vector3(randomScale, randomScale, randomScale);

            // Apply random rotation
            float randomRotation = Random.Range(rotationRange.x, rotationRange.y);
            obj.transform.rotation = Quaternion.Euler(0, randomRotation, 0);
        }
    }
}