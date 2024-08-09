using UnityEngine;

namespace Interfaces
{
    public interface IInteractable
    {
        /// <summary>
        /// Called when something interacted with this object
        /// </summary>
        public void OnClick();

        /// <summary>
        /// Tries to find a target and interacts with it
        /// </summary>
        public static void TryInteract(Vector3 position, Vector3 direction, float? maxDistance = null)
        {
            if (!CanInteract(position, direction, out IInteractable interactable, maxDistance))
                return;

            Interact(interactable);
        }

        /// <summary>
        /// Checks if the ray touches something to interact with
        /// </summary>
        /// <returns>Is there something to interact with?</returns>
        public static bool CanInteract(Vector3 position, Vector3 direction, out IInteractable interactable, float? maxDistance = null)
        {
            int layer = 1 << LayerMask.NameToLayer("Interact");

            if (!Physics.Raycast(position, direction, out RaycastHit hit, maxDistance ?? float.MaxValue, layer))
            {
                interactable = null;
                return false;
            }

            return hit.collider.gameObject.TryGetComponent(out interactable);
        }

        /// <summary>
        /// Checks if the ray touches something to interact with
        /// </summary>
        /// <returns>Is there something to interact with?</returns>
        public static bool CanInteract(Vector3 position, Vector3 direction, float? maxDistance = null)
            => CanInteract(position, direction, out _, maxDistance);

        /// <summary>
        /// Starts an interaction with the given target
        /// </summary>
        public static void Interact(IInteractable target) => target.OnClick();
    }
}