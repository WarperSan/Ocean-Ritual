using Chain;
using UnityEngine;

namespace ChainInGame
{
    public class InteractableGear : MonoBehaviour
    {
        public Cogwheel _gear;
        private BoxCollider _collider;
        public void Setup(Cogwheel gear)
        {
            _gear = gear;
            AddCollider();
            SetSize();
        }

        private void AddCollider()
        {
            _collider = gameObject.AddComponent<BoxCollider>();
        }

        private void SetSize()
        {
            float size = _gear.Data.Radius * 1.5f;
            _collider.size = new Vector3(size, 1, size);
        }
    }

}
