using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ComponantPowerGemmeObject))]
[CanEditMultipleObjects]
public class PowerGemmeEditor : UnityEditor.Editor
{
    private SerializedProperty powerGemmeObjectProp;
    private SerializedProperty typeGemmeProp;
    private SerializedProperty SocleConteneur;
    private SerializedProperty GemmeConteneur;
    private SerializedProperty gridScript;
    void OnEnable()
    {
        powerGemmeObjectProp = serializedObject.FindProperty("PowerGemmeObjectScript");
        gridScript = powerGemmeObjectProp.FindPropertyRelative("GridGemme");
        typeGemmeProp = powerGemmeObjectProp.FindPropertyRelative("GemmeComponantList");
        SocleConteneur = powerGemmeObjectProp.FindPropertyRelative("SocleConteneur");
        GemmeConteneur = powerGemmeObjectProp.FindPropertyRelative("GemmeConteneur");
    }

    public override void OnInspectorGUI()
    {
       
        serializedObject.Update();
        ShowSingleProperty(gridScript, "GridScript");
        ShowSingleProperty(SocleConteneur, "Socle Conteneur");
        ShowSingleProperty(GemmeConteneur, "Gemme Conteneur");
        ShowGameObjectListProperties(typeGemmeProp);

        serializedObject.ApplyModifiedProperties();
    }

    private void ShowGameObjectListProperties(SerializedProperty listProperty)
    {
        if (listProperty == null)
            return;

        EditorGUILayout.LabelField("Gemme List", EditorStyles.boldLabel);

        if (GUILayout.Button("Add Gemme"))
        {
            listProperty.InsertArrayElementAtIndex(listProperty.arraySize);
        }

        for (int i = 0; i < listProperty.arraySize; i++)
        {
            EditorGUILayout.BeginHorizontal();

            SerializedProperty element = listProperty.GetArrayElementAtIndex(i);
            EditorGUILayout.PropertyField(element, GUIContent.none);

            if (GUILayout.Button("Remove"))
            {
                listProperty.DeleteArrayElementAtIndex(i);
            }

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