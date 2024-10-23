using InteractModule;
using UIModule;
using UIModule.Menus;
using UnityEngine;

namespace BlacksmithModule
{
    public class InteractBlacksmith : MonoBehaviour, IInteractable
    {
        public InteractionAsset InteractionAsset => null;

        /// <inheritdoc/>
        public void OnClick() => UIManager.Open<BlacksmithMenu>();
    }
}