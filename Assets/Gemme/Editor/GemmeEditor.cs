using UnityEditor;
using UnityEngine;
using static EnumGeneral;

[CustomEditor(typeof(PowerGemmeObject))]
[CanEditMultipleObjects]
public class GemmeEditor : Editor
{
    private SerializedProperty typeDeSocleProp;
    private SerializedProperty typeArmeProp;
    private SerializedProperty typeBateauProp;
    private SerializedProperty typeFiletProp;

    void OnEnable()
    {
        typeDeSocleProp = serializedObject.FindProperty("typeDeSocle");
        typeArmeProp = serializedObject.FindProperty("typeArme");
        typeBateauProp = serializedObject.FindProperty("typeBoat");
        typeFiletProp = serializedObject.FindProperty("typeFilet");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

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
        
        var powerGemmeObject = (PowerGemmeObject)target;
        powerGemmeObject.ResetLists();
    }
}