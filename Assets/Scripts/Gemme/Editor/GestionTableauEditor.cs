using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(GemmeGrid))]
public class GestionTableauEditor : UnityEditor.Editor
{
    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        GemmeGrid gemmeGrid = (GemmeGrid)target;

        gemmeGrid.width = EditorGUILayout.IntField("Width", gemmeGrid.width);
        gemmeGrid.height = EditorGUILayout.IntField("Height", gemmeGrid.height);

        if (gemmeGrid.tableau == null || gemmeGrid.tableau.GetLength(0) != gemmeGrid.width || gemmeGrid.tableau.GetLength(1) != gemmeGrid.height)
        {
            gemmeGrid.InitializeTableau();
        }

        for (int j = 0; j < gemmeGrid.height; j++) // Loop from 0 to height
        {
            EditorGUILayout.BeginHorizontal();
            for (int i = 0; i < gemmeGrid.width; i++)
            {
                bool newValue = EditorGUILayout.Toggle(gemmeGrid.tableau[i, gemmeGrid.height - 1 - j]); // Access inverted row
                if (newValue != gemmeGrid.tableau[i, gemmeGrid.height - 1 - j])
                {
                    gemmeGrid.tableau[i, gemmeGrid.height - 1 - j] = newValue;
                }
            }
            EditorGUILayout.EndHorizontal();
        }

        serializedObject.ApplyModifiedProperties();
    }
}