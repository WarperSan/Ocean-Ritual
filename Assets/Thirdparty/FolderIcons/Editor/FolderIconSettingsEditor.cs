using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace FolderIcons
{
    [CustomEditor(typeof(FolderIconSettings))]
    internal class FolderIconSettingsEditor : Editor
    {
        // References
        private FolderIconSettings settings;
        private SerializedProperty serializedIcons;

        // Settings
        private bool showCustomFolders;

        private ReorderableList iconList;

        // Texture Save Settings
        private int heightIndex;

        // Sizing
        private const float MAX_LABEL_WIDTH = 90f;
        private const float MAX_FIELD_WIDTH = 150f;

        private const float PROPERTY_HEIGHT = 19f;
        private const float PROPERTY_PADDING = 4f;

        // Styling
        private GUIStyle elementStyle;

        private GUIStyle previewStyle;

        private void OnEnable()
        {
            if (target == null)
            {
                return;
            }

            settings = target as FolderIconSettings;
            serializedIcons = serializedObject.FindProperty("icons");

            showCustomFolders = settings.showCustomFolder;

            iconList ??= new ReorderableList(serializedObject, serializedIcons)
            {
                drawHeaderCallback = OnHeaderDraw,

                drawElementCallback = OnElementDraw,
                drawElementBackgroundCallback = DrawElementBackground,

                elementHeightCallback = GetPropertyHeight,

                showDefaultBackground = false,
            };
        }

        public override void OnInspectorGUI()
        {
            //Create styles
            if (previewStyle == null)
            {
                previewStyle = new GUIStyle(EditorStyles.label)
                {
                    fixedHeight = 64,
                    //fixedWidth = 64,
                    //stretchWidth = false,
                    alignment = TextAnchor.MiddleCenter
                };

                elementStyle = new GUIStyle(GUI.skin.box)
                {
                };
            }

            // Draw Settings
            EditorGUILayout.LabelField("Settings", EditorStyles.boldLabel);

            EditorGUI.BeginChangeCheck();
            {
                showCustomFolders = EditorGUILayout.ToggleLeft("Show Folder Textures", showCustomFolders);
            }
            if (EditorGUI.EndChangeCheck())
            {
                ApplySettings();
            }

            EditorGUILayout.Space(16f);

            EditorGUI.BeginChangeCheck();
            iconList.DoLayoutList();
            if (EditorGUI.EndChangeCheck())
            {
                serializedObject.ApplyModifiedProperties();
            }
        }

        private void ApplySettings()
        {
            FolderIconsReplacer.showFolder = settings.showCustomFolder = showCustomFolders;
        }

        #region Reorderable Array Draw

        private void OnHeaderDraw(Rect rect)
        {
            rect.y += 5f;
            rect.x -= 6f;
            rect.width += 12f;

            Handles.BeginGUI();
            Handles.DrawSolidRectangleWithOutline(rect,
                new Color(0.15f, 0.15f, 0.15f, 1f),
                new Color(0.15f, 0.15f, 0.15f, 1f));
            Handles.EndGUI();

            EditorGUI.LabelField(rect, "Folders", EditorStyles.boldLabel);
        }

        private void OnElementDraw(Rect rect, int index, bool isActive, bool isFocused)
        {
            SerializedProperty property = serializedIcons.GetArrayElementAtIndex(index);

            float fullWidth = rect.width;

            // Set sizes for correct draw
            float originalLabelWidth = EditorGUIUtility.labelWidth;
            float rectWidth = MAX_LABEL_WIDTH + MAX_FIELD_WIDTH;
            EditorGUIUtility.labelWidth = Mathf.Min(EditorGUIUtility.labelWidth, MAX_LABEL_WIDTH);
            rect.width = Mathf.Min(rect.width, rectWidth);

            //Draw property and settings in a line
            DrawPropertyNoDepth(rect, property);

            // ==========================
            //     Draw Icon Example
            // ==========================
            rect.x += rect.width;
            rect.width = fullWidth - rect.width;

            // References
            SerializedProperty folderTexture = property.FindPropertyRelative("folderIcon");

            // Object checks
            Object folderObject = folderTexture.objectReferenceValue;

            FolderIconGUI.DrawFolderPreview(rect, folderObject as Texture);

            // Revert width modification
            EditorGUIUtility.labelWidth = originalLabelWidth;
        }

        private void DrawPropertyNoDepth(Rect rect, SerializedProperty property)
        {
            rect.width++;
            Handles.BeginGUI();
            Handles.DrawSolidRectangleWithOutline(rect, Color.clear, new Color(0.15f, 0.15f, 0.15f, 1f));
            Handles.EndGUI();

            rect.x++;
            rect.width -= 3;
            rect.y += PROPERTY_PADDING;
            rect.height = PROPERTY_HEIGHT;

            SerializedProperty copy = property.Copy();
            bool enterChildren = true;

            while (copy.Next(enterChildren))
            {
                if (SerializedProperty.EqualContents(copy, property.GetEndProperty()))
                {
                    break;
                }

                EditorGUI.PropertyField(rect, copy, false);
                rect.y += PROPERTY_HEIGHT + PROPERTY_PADDING;

                enterChildren = false;
            }
        }

        private void DrawElementBackground(Rect rect, int index, bool isActive, bool isFocused)
        {
            EditorGUI.LabelField(rect, "", elementStyle);

            Color fill = isFocused ? FolderIconConstants.SelectedColor : Color.clear;

            Handles.BeginGUI();
            Handles.DrawSolidRectangleWithOutline(rect, fill, new Color(0.15f, 0.15f, 0.15f, 1f));
            Handles.EndGUI();
        }

        // ========================
        //
        // ========================

        private float GetPropertyHeight(SerializedProperty property)
        {
            if (heightIndex == 0)
            {
                heightIndex = property.CountInProperty();
            }

            // return (PROPERTY_HEIGHT + PROPERTY_PADDING) * (heightIndex-1) + PROPERTY_PADDING;

            //Property count returning wrong, so just supplying 3 for now
            //TODO: Investigate GetPropertyCount and find issue with invalid value
            return (PROPERTY_HEIGHT + PROPERTY_PADDING) * (3) + PROPERTY_PADDING;
        }

        private float GetPropertyHeight(int index)
        {
            return GetPropertyHeight(serializedIcons.GetArrayElementAtIndex(index));
        }

        #endregion Reorderable Array Draw
    }
}