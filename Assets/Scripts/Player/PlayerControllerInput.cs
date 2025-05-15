using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerControllerInput : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    /*
     * UnActivated controller will handle jump , Activated one will handle player rotation 
    */
    private const float MAX_DRAG_DISTANCE = 50f;

    public static event Action<ControllerType, PlayerAction> OnHold;
    public static event Action<ControllerType, PlayerAction> OnRelease;
    public static event Action<bool> OnActionStarted;

    //public static Vector2 _currentPos;
    public static Vector2 InputDirection;

    [SerializeField] private ControllerType _controllerType;

    private RectTransform _rectTransform;
    [SerializeField]
    private PlayerAction _currentActiveAction;
    private Vector3 _initialPos;
    [SerializeField]
    private RectTransform joystickBackground;
    

    //====================================================================
    private void Start() {
        _rectTransform = GetComponent<RectTransform>();

        _initialPos = transform.position;
    }
    private void OnEnable() {
        OnActionStarted += OnAnyActionStarted;
    }
    private void OnDisable() {
        OnActionStarted -= OnAnyActionStarted;
    }
    //====================================================================
    private void OnAnyActionStarted(bool active) {
        if(!active) {
            _currentActiveAction = PlayerAction.NotActive;
            return;
        }

        if(_currentActiveAction == PlayerAction.NotActive) {
            _currentActiveAction = PlayerAction.Jump;
        }
    }
    public void OnDrag(PointerEventData eventData) {
        if (_currentActiveAction != PlayerAction.Rotate) return;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            joystickBackground,
            eventData.position,
            eventData.pressEventCamera,
            out Vector2 localPoint
        );

        // Calculate normalized input direction
        Vector2 radius = joystickBackground.sizeDelta / 2;
        InputDirection = localPoint / radius;
        InputDirection = Vector2.ClampMagnitude(InputDirection, 1f);

        // Move the handle visually
        _rectTransform.anchoredPosition = InputDirection * radius;
    }

    public void OnPointerDown(PointerEventData eventData) {
        if(_currentActiveAction == PlayerAction.NotActive) {
            _currentActiveAction = PlayerAction.Rotate;
            OnActionStarted?.Invoke(true);
        }
        //InputDirection = _rectTransform.anchoredPosition;
        OnHold?.Invoke(_controllerType, _currentActiveAction);
    }

    public void OnPointerUp(PointerEventData eventData) {
        OnRelease?.Invoke(_controllerType, _currentActiveAction);
        if(_currentActiveAction == PlayerAction.Rotate) {
            OnActionStarted?.Invoke(false);
        }
        transform.position = _initialPos;
        InputDirection = Vector2.zero;
    }
}
public enum ControllerType {
    LeftController, RightController, Both
}
public enum PlayerAction {
    NotActive, Rotate, Jump
}