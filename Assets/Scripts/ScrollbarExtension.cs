
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class ScrollbarExtension : MonoBehaviour, IDragHandler
{
    public UnityEvent Drag;
    public void OnDrag(PointerEventData eventData)
    {
        Drag.Invoke();
    }
}
