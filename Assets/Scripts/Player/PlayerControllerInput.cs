using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerControllerInput : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    public static event Action<ControllerType> OnHold;
    public static event Action<ControllerType> OnRelease;

    [SerializeField] private ControllerType _controllerType;

    public void OnDrag(PointerEventData eventData) {
    }

    public void OnPointerDown(PointerEventData eventData) {
        //TODO: a coroutine to handle specified time to trigger hold
        OnHold?.Invoke(_controllerType);
    }

    public void OnPointerUp(PointerEventData eventData) {
        OnRelease?.Invoke(_controllerType);
    }
}
public enum ControllerType {
    LeftController, RightController, Both
}