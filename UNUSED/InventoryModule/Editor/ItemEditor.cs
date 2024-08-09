using UnityEditor;
using Inventory;
using UnityEngine;
using Extensions;

namespace Editor.Editors
{
    [CustomEditor(typeof(Item), true)]
    public class ItemEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (this.target is not Item item)
                return;

            EditorGUILayout.Separator();
            EditorGUILayout.LabelField("Item", EditorStyles.boldLabel);
            EditorGUILayout.LabelField("Namespace: " + item.Namespace);

            // Skip if target is not an asset
            if (!AssetDatabase.Contains(this.target))
                return;

            string newNamespace = GenerateNamespace(item);

            // Skip if equals
            if (newNamespace.Equals(item.Namespace))
                return;

            GUILayout.Space(10);

            // If not clicked, skip
            if (!GUILayout.Button("Update Namespace"))
                return;

            Undo.RecordObject(item, "Update Namespace");
            item.Namespace = newNamespace;
            Debug.Log($"Updated the namespace of '{this.target.name}' to '{item.Namespace}'.");
        }

        /// <summary>
        /// Generates the namespace for the given item
        /// </summary>
        private static string GenerateNamespace(Item item)
        {
            string path = AssetDatabase.GetAssetPath(item);
            path = path.Replace("Resources/", ""); // Remove Resources
            path = path.Replace("Items/", ""); // Remove Items
            path = path.Split(".")[0]; // Remove extension

            string[] folders = path.Split("/");
            folders[0] = Application.productName; // Application name as first

            return folders.Join(".").ToLower().Replace(" ", "_");
        }
    }
}