using UnityEngine;
using UnityEditor;

[InitializeOnLoad]
public class RandomRockPlacerEditor : Editor
{
    private static GameObject rockPrefab;

    static RandomRockPlacerEditor()
    {
        // Replace "YourPrefabName" with the name of your prefab
        rockPrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Path/To/YourPrefab.prefab");

        // Hook into the scene's drag-and-drop event
        SceneView.duringSceneGui += OnSceneGUI;
    }

    private static void OnSceneGUI(SceneView sceneView)
    {
        Event e = Event.current;
        if ((e.type == EventType.DragPerform || e.type == EventType.DragUpdated) && DragAndDrop.objectReferences.Length > 0)
        {
            // Get the current drag and drop object
            Object obj = DragAndDrop.objectReferences[0];
            GameObject go = obj as GameObject;

            // Check if the object being dragged is the specific prefab
            if (go != null && PrefabUtility.GetPrefabAssetType(go) != PrefabAssetType.NotAPrefab && go == rockPrefab)
            {
                ApplyRandomTransform(go);
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