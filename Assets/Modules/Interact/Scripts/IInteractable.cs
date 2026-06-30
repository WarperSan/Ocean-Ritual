using UnityEngine;

namespace InteractModule
{
    public interface IInteractable
    {
        /// <summary>
        /// Called when something interacted with this object
        /// </summary>
        void OnClick();

        InteractionAsset InteractionAsset { get; }

        #region Static Fields

        /// <summary>Layer index of the interactable layer</summary>
        static readonly int LAYER = LayerMask.NameToLayer("Interactable");

        /// <summary>Layer mask containing the interactable layer</summary>
        static readonly int LAYER_MASK = 1 << LAYER;

        #endregion

        #region Static Methods

        /// <summary>Starts an interaction with the given target</summary>
        private static void Interact(IInteractable target)
        {
            if (target == null)
            {
                Debug.LogWarning("Tried to interact with an invalid target.");
                return;
            }

            target.OnClick();
        }

        /// <summary>
        /// Checks if the ray touches something to interact with
        /// </summary>
        /// <returns>Is there something to interact with?</returns>
        static bool CanInteract(
            Vector3           position,
            Vector3           direction,
            out IInteractable target,
            float             maxDistance = float.MaxValue
        )
        {
            // If hit something
            if (Physics.Raycast(position,
                    direction,
                    out RaycastHit hit,
                    maxDistance,
                    LAYER_MASK))
                return hit.collider.TryGetComponent(out target);

            target = null;
            return false;
        }

        /// <summary>
        /// Tries to find a target and interacts with it
        /// </summary>
        static IInteractable TryInteract(Vector3 position, Vector3 direction, float maxDistance = float.MaxValue)
        {
            // Interact if possible
            if (CanInteract(position,
                    direction,
                    out IInteractable target,
                    maxDistance))
                Interact(target);

            return target;
        }

        #endregion
    }
}