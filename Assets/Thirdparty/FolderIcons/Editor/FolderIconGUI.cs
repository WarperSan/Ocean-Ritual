using UnityEditor;
using UnityEngine;

namespace FolderIcons
{
    /// <summary>
    /// GUI Methods for Folder Icons.
    /// </summary>
    public static class FolderIconGUI
    {
        /// <summary>
        /// Draw the folder preview
        /// </summary>
        /// <param name="rect">Rect to draw preview</param>
        /// <param name="folder">The folder texture</param>
        public static void DrawFolderPreview(Rect rect, Texture folder)
        {
            if (folder == null)
            {
                return;
            }

            if (folder != null)
            {
                GUI.DrawTexture(rect, folder, ScaleMode.ScaleToFit);
            }

            //Half size of overlay, and reposition to center
            rect.size *= 0.5f;
            rect.position += rect.size * 0.5f;
        }

        /// <summary>
        /// Draw the folder texture and background rect if required
        /// </summary>
        /// <param name="rect">Folder rect</param>
        /// <param name="folder">Folder texture</param>
        public static void DrawFolderTexture(Rect rect, Texture folder)
        {
            if (folder == null)
            {
                return;
            }

            EditorGUI.DrawRect(rect, FolderIconConstants.BackgroundColour);
            GUI.DrawTexture(rect, folder, ScaleMode.ScaleAndCrop);
        }

        /// <summary>
        /// Check if the current rect is the side view of folders
        /// </summary>
        /// <param name="rect">Current rect</param>
        public static bool IsSideView(Rect rect)
        {
#if UNITY_2019_3_OR_NEWER
            return rect.x != 14;
#else
            return rect.x != 13;
#endif
        }
    }
}