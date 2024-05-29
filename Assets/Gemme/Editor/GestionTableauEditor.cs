using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(GemmeGrid))]
public class GestionTableauEditor : Editor
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

        for (int j = gemmeGrid.height - 1; j >= 0; j--)
        {
            EditorGUILayout.BeginHorizontal();
            for (int i = 0; i < gemmeGrid.width; i++)
            {
                bool newValue = EditorGUILayout.Toggle(gemmeGrid.tableau[i, j]);
                if (newValue != gemmeGrid.tableau[i, j])
                {
                    gemmeGrid.tableau[i, j] = newValue;
                }
            }
            EditorGUILayout.EndHorizontal();
        }

        serializedObject.ApplyModifiedProperties();
    }
}