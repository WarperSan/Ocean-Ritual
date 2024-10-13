using UnityEngine;

namespace UIModule.Interfaces
{
    /// <summary>
    /// Defines the objects that can receive another draggable object
    /// </summary>
    public interface IDragReceivable
    {
        public RectTransform Rect { get; }

        public bool CanReceive(IDraggable draggable);

        public void OnDragReceive(IDraggable draggable);
        public void OnDragLeave(IDraggable draggable);
    }

    /// <summary>
    /// Wrapper for <see cref="IDragReceivable"/>
    /// </summary>
    public interface IDragReceivable<T> : IDragReceivable where T : IDraggable
    {
        #region IDragReceivable

        /// <inheritdoc/>
        bool IDragReceivable.CanReceive(IDraggable draggable)
            => draggable is T typedDraggable && this.CanReceive(typedDraggable);

        /// <inheritdoc/>
        void IDragReceivable.OnDragReceive(IDraggable draggable)
        {
            if (draggable is not T typedDraggable)
                return;

            this.OnDragReceive(typedDraggable);
        }

        /// <inheritdoc/>
        void IDragReceivable.OnDragLeave(IDraggable draggable)
        {
            if (draggable is not T typedDraggable)
                return;

            this.OnDragLeave(typedDraggable);
        }

        #endregion

        /// <inheritdoc cref="IDragReceivable.CanReceive(IDraggable)"/>
        public virtual bool CanReceive(T draggable) => true;

        /// <inheritdoc cref="IDragReceivable.OnDragReceive(IDraggable)"/>
        public void OnDragReceive(T draggable);

        /// <inheritdoc cref="IDragReceivable.OnDragLeave(IDraggable)"/>
        public void OnDragLeave(T draggable);
    }
}