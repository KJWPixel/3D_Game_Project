using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DraggableItem : MonoBehaviour, IBeginDragHandler,  IDragHandler, IEndDragHandler
{
    public Image image;
    public Transform parentAfterDrag;
    
    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log("드래그 시작");
        parentAfterDrag = transform.parent;
        transform.SetParent(transform.root);
        transform.SetAsLastSibling();
        image.raycastTarget = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        Debug.Log("드래그 중");
        transform.position = Input.mousePosition; // 드래그 중인면 마우스위치
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("드래그  끝");
        transform.SetParent(parentAfterDrag);
        image.rectTransform.localPosition = Vector3.zero;
        image.raycastTarget = true;
    }
}
