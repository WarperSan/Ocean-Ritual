using UnityEditor;
using UnityEngine;

namespace InteractModule.Editor
{
    [InitializeOnLoad]
    public class IInteractableEditor
    {
        // Subscribe to the event that gets called whenever the scene is loaded or an object is changed
        static IInteractableEditor()
        {
            EditorApplication.hierarchyChanged += () => CheckLayer(Selection.activeGameObject);
        }

        private static void CheckLayer(GameObject target)
        {
            // If target is invalid, skip
            if (target == null)
                return;

            // If target is on correct layer, skip
            if (target.layer == IInteractable.LAYER)
                return;

            // If target has no interactable script, skip
            if (target.GetComponent<IInteractable>() == null)
                return;

            bool clickedOK = EditorUtility.DisplayDialog(
                "Fix the layer?",
                $"We found an object with a script implementing '{nameof(IInteractable)}' not on the '{LayerMask.LayerToName(IInteractable.LAYER)}' layer. This makes the script unusable.\n\nDo you want to fix this?",
                "Yes, fix it",
                "No, I know what I'm doing");

            // If didn't skip OK, skip
            if (!clickedOK)
                return;

            target.layer = IInteractable.LAYER;
        }
    }
}