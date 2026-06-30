using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(componentPowerGemObject))]
[CanEditMultipleObjects]
public class PowerGemmeEditor : UnityEditor.Editor
{
    private SerializedProperty powerGemObjectProp;
    private SerializedProperty typeGemProp;
    private SerializedProperty SocleContainer;
    private SerializedProperty GemContainer;
    private SerializedProperty gridScript;

    private void OnEnable()
    {
        powerGemObjectProp = serializedObject.FindProperty("PowerGemObjectScript");
        gridScript = powerGemObjectProp.FindPropertyRelative("GridGemme");
        typeGemProp = powerGemObjectProp.FindPropertyRelative("GemcomponentList");
        SocleContainer = powerGemObjectProp.FindPropertyRelative("SocleContainer");
        GemContainer = powerGemObjectProp.FindPropertyRelative("GemContainer");
    }

    public override void OnInspectorGUI()
    {
       
        serializedObject.Update();
        ShowSingleProperty(gridScript, "GridScript");
        ShowSingleProperty(SocleContainer, "Socle Conteneur");
        ShowSingleProperty(GemContainer, "Gem Conteneur");
        ShowGameObjectListProperties(typeGemProp);

        serializedObject.ApplyModifiedProperties();
    }

    private void ShowGameObjectListProperties(SerializedProperty listProperty)
    {
        if (listProperty == null)
            return;

        EditorGUILayout.LabelField("Gem List", EditorStyles.boldLabel);

        if (GUILayout.Button("Add Gem"))
            listProperty.InsertArrayElementAtIndex(listProperty.arraySize);

        for (int i = 0; i < listProperty.arraySize; i++)
        {
            EditorGUILayout.BeginHorizontal();

            SerializedProperty element = listProperty.GetArrayElementAtIndex(i);
            EditorGUILayout.PropertyField(element, GUIContent.none);

            if (GUILayout.Button("Remove"))
                listProperty.DeleteArrayElementAtIndex(i);

            EditorGUILayout.EndHorizontal();
        }
    }

    private void ShowSingleProperty(SerializedProperty property, string label)
    {
        if (property == null)
            return;

        EditorGUILayout.PropertyField(property, new GUIContent(label));
    }
}