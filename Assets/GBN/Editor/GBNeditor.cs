using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static EnumGeneral;

[CustomEditor(typeof(ComponantGBN))]
[CanEditMultipleObjects]
public class GBNeditor : Editor
{
    private SerializedProperty typeDeSocleProp;
    private SerializedProperty typeArmeProp;
    private SerializedProperty typeBateauProp;
    private SerializedProperty typeFiletProp;
    private SerializedProperty SocleListProp;
    private SerializedProperty GBNProp;
    private SerializedProperty NameProp;
    //
    void OnEnable()
    {
       
        GBNProp = serializedObject.FindProperty("GBNScript");
        NameProp = GBNProp.FindPropertyRelative("Name");
        typeDeSocleProp = GBNProp.FindPropertyRelative("typeDeSocle");
        typeArmeProp = GBNProp.FindPropertyRelative("typeArme");
        typeBateauProp = GBNProp.FindPropertyRelative("typeBoat");
        typeFiletProp = GBNProp.FindPropertyRelative("typeFilet");
        SocleListProp = GBNProp.FindPropertyRelative("SocleListe");
    }
    //
    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        ShowSingleProperty(NameProp, "Name");
        ShowGameObjectListProperties(SocleListProp);
        EditorGUILayout.PropertyField(typeDeSocleProp);

        EnumGeneral.TypeDeSocle socleType = (EnumGeneral.TypeDeSocle)typeDeSocleProp.enumValueIndex;

        switch (socleType)
        {
            case EnumGeneral.TypeDeSocle.Arme:
                ShowListProperties(typeArmeProp, typeof(TypeArme));
                break;
            case EnumGeneral.TypeDeSocle.Bateau:
                ShowListProperties(typeBateauProp, typeof(TypeBateau));
                break;
            case EnumGeneral.TypeDeSocle.Filet:
                ShowListProperties(typeFiletProp, typeof(TypeFilet));
                break;
        }

        // Add a reset button
        if (GUILayout.Button("Reset Lists"))
        {
            ResetLists();
        }

        serializedObject.ApplyModifiedProperties();
    }

    private void ShowGameObjectListProperties(SerializedProperty listProperty)
    {
        if (listProperty == null)
            return;

        EditorGUILayout.LabelField("Socle List", EditorStyles.boldLabel);

        if (GUILayout.Button("Add GameObject"))
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
    private void ShowListProperties(SerializedProperty listProperty, System.Type enumType)
    {
        if (listProperty == null)
            return;

        if (listProperty.isArray && listProperty.arraySize == 0)
        {
            var enumValues = System.Enum.GetValues(enumType);

            foreach (var value in enumValues)
            {
                listProperty.InsertArrayElementAtIndex(listProperty.arraySize);
                var element = listProperty.GetArrayElementAtIndex(listProperty.arraySize - 1);
                element.FindPropertyRelative("Type").enumValueIndex = (int)value;
                element.FindPropertyRelative("Quantite").floatValue = 1;
            }
        }

        EditorGUILayout.PropertyField(listProperty, true);
    }

    private void ResetLists()
    {
        var componentGBN = (ComponantGBN)target;
        componentGBN.GBNScript.ResetLists();
    }
}