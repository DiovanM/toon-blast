using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class PointerHandler : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler, IBeginDragHandler, IDragHandler
{
    public Action<PointerEventData> onPointerClick;
    public Action<PointerEventData> onPointerDown;
    public Action<PointerEventData> onPointerUp;
    public Action<PointerEventData> onBeginDrag;
    public Action<PointerEventData> onDrag;

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
