using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class JoystickController : MonoBehaviour, IDragHandler,IBeginDragHandler, IEndDragHandler,IPointerUpHandler,IPointerDownHandler
{

    [SerializeField]RectTransform m_knobRectTransform;
    
    //the point beyond which we accept input from the joystick
    [SerializeField] float dragThreshold = 0.6f;

    //distance the joystick knob moves about on screen
    [SerializeField] int dragMovementDistance = 30;

    //used to normalize input to between 0 and 1;
    [SerializeField] int dragOffsetDistance = 100;

    public event Action<Vector2> OnMove;


    Vector2 m_originalKnobPosition;

    #region Drag interface implementation
    public void OnBeginDrag(PointerEventData eventData)
    {
        
    }

    public void OnDrag(PointerEventData eventData)
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            m_knobRectTransform,
            eventData.position,
            null,
            out Vector2 offset);
        offset = Vector2.ClampMagnitude(offset, dragOffsetDistance) / dragOffsetDistance;
        m_knobRectTransform.anchoredPosition = offset * dragMovementDistance;
        OnMove?.Invoke(offset);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
       
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        m_knobRectTransform.anchoredPosition = Vector2.zero;
        OnMove?.Invoke(Vector2.zero);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
      
    }

    #endregion
}
