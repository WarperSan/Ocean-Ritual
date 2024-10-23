using UnityEngine;

namespace UIModule.Interfaces
{
    /// <summary>
    /// Defines the objects that can receive another draggable object
    /// </summary>
    public interface IDragReceivable
    {
        public RectTransform Rect { get; }

        /// <summary>
        /// Checks if the given draggable can be received by this object
        /// </summary>
        public bool CanReceiveDraggable(IDraggable draggable);

        /// <summary>
        /// Called when this object receives a draggable
        /// </summary>
        /// <returns>Cancels the call of <see cref="IDraggable.OnDragEnd(IDragReceivable, RectTransform)"/></returns>
        public void OnDragReceive(IDraggable draggable);

        /// <summary>
        /// Called when a draggable from this object leaves
        /// </summary>
        public void OnDragLeave(IDraggable draggable);
    }

    /// <summary>
    /// Wrapper for <see cref="IDragReceivable"/>
    /// </summary>
    /// <typeparam name="T">Type of draggable allowed</typeparam>
    public interface IDragReceivable<T> : IDragReceivable where T : IDraggable
    {
        #region IDragReceivable

        /// <inheritdoc/>
        bool IDragReceivable.CanReceiveDraggable(IDraggable draggable) 
            => draggable is T typedDraggable && this.CanReceiveDraggable(typedDraggable);

        /// <inheritdoc/>
        void IDragReceivable.OnDragReceive(IDraggable draggable)
        {
            // If wrong type, skip
            if (draggable is not T typedDraggable)
                return;

            this.OnDragReceive(typedDraggable);
        }

        /// <inheritdoc/>
        void IDragReceivable.OnDragLeave(IDraggable draggable)
        {
            // If wrong type, skip
            if (draggable is not T typedDraggable)
                return;

            this.OnDragLeave(typedDraggable);
        }

        #endregion

        /// <inheritdoc cref="IDragReceivable.CanReceiveDraggable(IDraggable)"/>
        public bool CanReceiveDraggable(T draggable) => true;

        /// <inheritdoc cref="IDragReceivable.OnDragReceive(IDraggable)"/>
        public void OnDragReceive(T draggable);

        /// <inheritdoc cref="IDragReceivable.OnDragLeave(IDraggable)"/>
        public void OnDragLeave(T draggable);
    }
}