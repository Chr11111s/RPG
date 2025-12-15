using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class WndMove : MonoBehaviour, IBeginDragHandler, IDragHandler
{
    Vector3 _offset;
    public void OnBeginDrag(PointerEventData eventData)
    {
        Vector3 pos = eventData.position;
        _offset = transform.position - pos;
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector3 pos = eventData.position;
        transform.position = pos + _offset;
    }
}
