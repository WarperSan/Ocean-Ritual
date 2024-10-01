using InteractModule;
using UnityEngine;

namespace SkyModule
{
    public class SkyboxDebugInteract : MonoBehaviour, IInteractable
    {
        public SkyboxManager skyboxManager;
        public float factor;

        public InteractionAsset InteractionAsset => null;

        public void OnClick()
        {
            for (int i = 0; i < skyboxManager.parallaxObjects.Count; i++)
            {
                SkyboxManager.ParallaxObject item = skyboxManager.parallaxObjects[i];
                item.factor *= factor;
            }

            skyboxManager.factor *= factor;
        }
    }
}