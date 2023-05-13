using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class PointerHandler : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler, IBeginDragHandler, IDragHandler
{
    public UnityEvent<PointerEventData> onPointerClick;
    public UnityEvent<PointerEventData> onPointerDown;
    public UnityEvent<PointerEventData> onPointerUp;
    public UnityEvent<PointerEventData> onBeginDrag;
    public UnityEvent<PointerEventData> onDrag;

    public void OnPointerClick(PointerEventData eventData)
    {
        if(!eventData.dragging)
            onPointerClick?.Invoke(eventData);
    }
    public void OnPointerDown(PointerEventData eventData) => onPointerDown?.Invoke(eventData);
    public void OnPointerUp(PointerEventData eventData) => onPointerUp?.Invoke(eventData);
    public void OnBeginDrag(PointerEventData eventData) => onBeginDrag?.Invoke(eventData);
    public void OnDrag(PointerEventData eventData) => onDrag?.Invoke(eventData);

}
