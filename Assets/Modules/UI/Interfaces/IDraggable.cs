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
        /// <param name="receivable">First receiver of the drag</param>
        /// <param name="target">First object that receive the drag on</param>
        public void OnDragEnd(IDragReceivable receivable, RectTransform target);

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

            IDragReceivable firstReceivable = null;
            RectTransform firstTarget = null;

            for (int i = 0; i < results.Count; i++)
            {
                GameObject target = results[i].gameObject;

                if (i == 0)
                    firstTarget = target.GetComponent<RectTransform>();

                // If not a receiver, skip
                if (!target.TryGetComponent(out IDragReceivable receivable))
                    continue;

                // If can't receive, skip
                if (!receivable.CanReceiveDraggable(this))
                    continue;

                firstReceivable = receivable;
                break;
            }

            firstReceivable?.OnDragReceive(this);
            
            this.OnDragEnd(firstReceivable, firstTarget);
        }

        #endregion
    }
}