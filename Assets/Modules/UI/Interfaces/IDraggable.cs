using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UIModule.Interfaces
{
    /// <summary>
    /// Defines an object that can be dragged
    /// </summary>
    public interface IDraggable : IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        public RectTransform Rect { get; }

        /// <summary>
        /// Called when this object starts to being dragged
        /// </summary>
        public void OnDragStart();

        /// <summary>
        /// Called when this object stops being dragged
        /// </summary>
        /// <param name="receivable">Receiver of the drag or null if end on none</param>
        public void OnDragEnd(IDragReceivable receivable);

        #region IBeginDragHandler

        void IBeginDragHandler.OnBeginDrag(PointerEventData eventData)
        {
            // If invalid, skip
            if (this.Rect == null)
                return;

            this.OnDragStart();
        }

        #endregion

        #region IDragHandler

        void IDragHandler.OnDrag(PointerEventData eventData)
        {
            // If invalid, skip
            if (this.Rect == null)
                return;

            this.Rect.position = eventData.position;
        }

        #endregion

        #region IEndDragHandler

        void IEndDragHandler.OnEndDrag(PointerEventData eventData)
        {
            // Set up the new Pointer Event
            PointerEventData m_PointerEventData = new(EventSystem.current)
            {
                // Set the Pointer Event Position to that of the game object
                position = Input.mousePosition
            };

            // Create a list of Raycast Results
            List<RaycastResult> results = new();

            // Raycast using the Graphics Raycaster and mouse click position
            EventSystem.current.RaycastAll(m_PointerEventData, results);

            IDragReceivable firstTarget = null;

            foreach (RaycastResult item in results)
            {
                if (!item.gameObject.TryGetComponent(out IDragReceivable receivable))
                    continue;

                firstTarget = receivable;
                break;
            }

            if (firstTarget != null)
            {

            }

            this.OnDragEnd(firstTarget);
        }

        #endregion
    }
}