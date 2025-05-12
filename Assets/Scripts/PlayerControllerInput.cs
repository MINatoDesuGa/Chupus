using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerControllerInput : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    public static event Action<Controller> OnHold;

    [SerializeField] private Controller _controller;

    public void OnDrag(PointerEventData eventData) {
    }

    public void OnPointerDown(PointerEventData eventData) {
    }

    public void OnPointerUp(PointerEventData eventData) {
    }
}
public enum Controller {
    LeftController, RightController
}