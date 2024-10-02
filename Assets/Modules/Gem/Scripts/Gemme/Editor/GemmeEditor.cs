using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(FormBool))]
public class GemmeEditor : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        SerializedProperty widthProp = property.FindPropertyRelative("width");
        SerializedProperty heightProp = property.FindPropertyRelative("height");
        SerializedProperty flatFormeProp = property.FindPropertyRelative("flatForme");

        int width = widthProp.intValue;
        int height = heightProp.intValue;

        // Draw the dimensions
        widthProp.intValue = EditorGUI.IntField(new Rect(position.x, position.y, position.width / 2 - 2, EditorGUIUtility.singleLineHeight), "Width", width);
        heightProp.intValue = EditorGUI.IntField(new Rect(position.x + position.width / 2 + 2, position.y, position.width / 2 - 2, EditorGUIUtility.singleLineHeight), "Height", height);

        position.y += EditorGUIUtility.singleLineHeight + 2;

        // Ensure the flatForme list has the correct size
        if (flatFormeProp.arraySize != width * height)
        {
            flatFormeProp.arraySize = width * height;
        }

        // Draw the grid
        for (int j = height - 1; j >= 0; j--)
        {
            EditorGUILayout.BeginHorizontal();
            for (int i = 0; i < width; i++)
            {
                int index = j * width + i;
                SerializedProperty element = flatFormeProp.GetArrayElementAtIndex(index);
                bool newValue = EditorGUI.Toggle(new Rect(position.x + i * 20, position.y + j * 20, 20, 20), element.boolValue);
                if (newValue != element.boolValue)
                {
                    element.boolValue = newValue;
                }
            }
            EditorGUILayout.EndHorizontal();
        }

        EditorGUI.EndProperty();
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        SerializedProperty heightProp = property.FindPropertyRelative("height");
        return EditorGUIUtility.singleLineHeight + 2 + (heightProp.intValue * 20);
    }
}