using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(RectTransform))]
public class DragAndDropHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private RectTransform rectTransform;

    public delegate void DragStart();
    public event DragStart OnDragStart;

    public delegate void DragEnd(List<RaycastResult> results);
    public event DragEnd OnDragEnd;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    public void OnBeginDrag(PointerEventData eventData) => OnDragStart?.Invoke();

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        // Set up the new Pointer Event
        PointerEventData m_PointerEventData = new(EventSystem.current);
        
        // Set the Pointer Event Position to that of the game object
        m_PointerEventData.position = Input.mousePosition;

        // Create a list of Raycast Results
        List<RaycastResult> results = new();

        // Raycast using the Graphics Raycaster and mouse click position
        EventSystem.current.RaycastAll(m_PointerEventData, results);

        OnDragEnd?.Invoke(results);
    }
}
