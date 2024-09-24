using ControllerModule.Controllers;
using UnityEngine;

namespace InteractModule.Premade
{
    /// <summary>
    /// Script that allows to easily switch to a desired controller
    /// </summary>
    [RequireComponent(typeof(Collider))]
    public class ControllerInteract : MonoBehaviour, IInteractable
    {
        public Controller Controller;

        /// <inheritdoc/>
        void IInteractable.OnClick() 
        {
            // If invalid, skip
            if (this.Controller == null)
            {
                Debug.LogWarning("Tried to switch to an invalid controller.");
                return;
            }

            ControllerManager.SwitchTo(this.Controller);
        }

        public InteractionAsset asset;

        /// <inheritdoc/>
        public InteractionAsset InteractionAsset => this.asset;
    }
}