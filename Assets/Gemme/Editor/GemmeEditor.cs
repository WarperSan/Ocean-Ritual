using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(FormeBool))]
public class GemmeEditor : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        SerializedProperty formeProperty = property.FindPropertyRelative("forme");

        // Ensure the forme array is initialized
        int width = 3; // Adjust this to the desired width of your forme
        int height = 3; // Adjust this to the desired height of your forme
        (property.serializedObject.targetObject as Gemme)?.InitializeForme(width, height);

        int rows = formeProperty.arraySize;
        int columns = formeProperty.GetArrayElementAtIndex(0).arraySize;

        // Draw the 2D array as a grid of toggle fields
        for (int i = 0; i < rows; i++)
        {
            SerializedProperty row = formeProperty.GetArrayElementAtIndex(i);
            Rect rowRect = new Rect(position.x, position.y + EditorGUIUtility.singleLineHeight * i, position.width, EditorGUIUtility.singleLineHeight);
            EditorGUI.BeginChangeCheck();
            for (int j = 0; j < columns; j++)
            {
                Rect cellRect = new Rect(rowRect.x + EditorGUIUtility.singleLineHeight * j, rowRect.y, EditorGUIUtility.singleLineHeight, EditorGUIUtility.singleLineHeight);
                bool newValue = EditorGUI.Toggle(cellRect, row.GetArrayElementAtIndex(j).boolValue);
                if (newValue != row.GetArrayElementAtIndex(j).boolValue)
                {
                    row.GetArrayElementAtIndex(j).boolValue = newValue;
                }
            }
            if (EditorGUI.EndChangeCheck())
            {
                property.serializedObject.ApplyModifiedProperties();
            }
        }

        EditorGUI.EndProperty();
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        SerializedProperty formeProperty = property.FindPropertyRelative("forme");
        int rows = formeProperty.arraySize;
        return EditorGUIUtility.singleLineHeight * rows;
    }
}