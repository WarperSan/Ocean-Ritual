using UnityEngine;

namespace InteractModule
{
    [CreateAssetMenu(fileName = "InteractionAsset", menuName = "ScriptableObjects/InteractionAsset", order = 0)]
    public class InteractionAsset : ScriptableObject
    {
        public Sprite icon;
    }
}