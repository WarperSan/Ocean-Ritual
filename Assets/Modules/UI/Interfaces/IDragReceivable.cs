using UnityEngine;

namespace UIModule.Interfaces
{
    /// <summary>
    /// Defines the objects that can receive another draggable object
    /// </summary>
    public interface IDragReceivable
    {
        RectTransform Rect { get; }

        /// <summary>
        /// Checks if the given draggable can be received by this object
        /// </summary>
        bool CanReceiveDraggable(IDraggable draggable);

        /// <summary>
        /// Called when this object receives a draggable
        /// </summary>
        /// <returns>Cancels the call of <see cref="IDraggable.OnDragEnd(IDragReceivable, RectTransform)"/></returns>
        void OnDragReceive(IDraggable draggable);

        /// <summary>
        /// Called when a draggable from this object leaves
        /// </summary>
        void OnDragLeave(IDraggable draggable);
    }

    /// <summary>
    /// Wrapper for <see cref="IDragReceivable"/>
    /// </summary>
    /// <typeparam name="T">Type of draggable allowed</typeparam>
    public interface IDragReceivable<T> : IDragReceivable where T : IDraggable
    {
        #region IDragReceivable

        /// <inheritdoc/>
        bool IDragReceivable.CanReceiveDraggable(IDraggable draggable) =>
            draggable is T typedDraggable && CanReceiveDraggable(typedDraggable);

        /// <inheritdoc/>
        void IDragReceivable.OnDragReceive(IDraggable draggable)
        {
            // If wrong type, skip
            if (draggable is not T typedDraggable)
                return;

            OnDragReceive(typedDraggable);
        }

        /// <inheritdoc/>
        void IDragReceivable.OnDragLeave(IDraggable draggable)
        {
            // If wrong type, skip
            if (draggable is not T typedDraggable)
                return;

            OnDragLeave(typedDraggable);
        }

        #endregion

        /// <inheritdoc cref="IDragReceivable.CanReceiveDraggable(IDraggable)"/>
        bool CanReceiveDraggable(T draggable) => true;

        /// <inheritdoc cref="IDragReceivable.OnDragReceive(IDraggable)"/>
        void OnDragReceive(T draggable);

        /// <inheritdoc cref="IDragReceivable.OnDragLeave(IDraggable)"/>
        void OnDragLeave(T draggable);
    }
}