using UnityEngine;
using UnityEngine.Events;

namespace InteractModule.Premade
{
    /// <summary>
    /// Script that allows to easily add click interaction for an object
    /// </summary>
    [RequireComponent(typeof(Collider))]
    public class Interactable : MonoBehaviour, IInteractable
    {
        public UnityEvent OnClick;

        /// <inheritdoc/>
        void IInteractable.OnClick() => OnClick?.Invoke();

        public InteractionAsset asset;

        /// <inheritdoc/>
        public InteractionAsset InteractionAsset => asset;
    }
}