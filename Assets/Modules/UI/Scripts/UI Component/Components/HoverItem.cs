using UnityEngine;

namespace UIModule.Components
{
    public abstract class HoverItem : UIComponent
    {
        public abstract bool CanShowData(ItemData data);
        public abstract void SetData(ItemData data);
    }

    public abstract class HoverItem<T> : HoverItem where T : ItemData
    {
        public sealed override bool CanShowData(ItemData data) => data is T;
        public sealed override void SetData(ItemData data)
        {
            if (data is not T typedData)
            {
                Debug.LogWarning($"Tried to set data of type '{data.GetType().Name}' in a hover item for '{nameof(T)}'.");
                return;
            }

            this.SetData(typedData);
        }

        protected abstract void SetData(T data);
    }
}