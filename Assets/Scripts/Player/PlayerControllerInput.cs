using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerControllerInput : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    /*
     * Activated controller will handle jump , unactivated one will handle player rotation 
    */
    private const float MAX_DRAG_DISTANCE = 25f;

    public static event Action<ControllerType> OnHold;
    public static event Action<ControllerType> OnRelease;

    [SerializeField] private ControllerType _controllerType;

    private RectTransform _rectTransform;

    public static Vector2 _currentPos;

    private bool _isActivated = false;
    private Vector3 _initialPos;
    private float _maxDragX, _maxDragY, _minDragX, _minDragY;
    //====================================================================
    private void Start() {
        _rectTransform = GetComponent<RectTransform>();

        _initialPos = transform.position;
        _maxDragX = _initialPos.x + MAX_DRAG_DISTANCE;
        _maxDragY = _initialPos.y + MAX_DRAG_DISTANCE;
        _minDragX = _initialPos.x - MAX_DRAG_DISTANCE;
        _minDragY = _initialPos.y - MAX_DRAG_DISTANCE;
    }
    //====================================================================
    public void OnDrag(PointerEventData eventData) {
        _currentPos += eventData.delta;
        if (_currentPos.magnitude > MAX_DRAG_DISTANCE) { 
            _currentPos = _currentPos.normalized * MAX_DRAG_DISTANCE;
        }

        _rectTransform.anchoredPosition = _currentPos;
        /*float newX = transform.position.x;
        float newY = transform.position.y;
        if (!_isActivated) {
            newY = Mathf.Clamp(transform.position.y + eventData.delta.y, _minDragY, _maxDragY);
        } else {
            newX = Mathf.Clamp(transform.position.x + eventData.delta.x, _minDragX, _maxDragX);
        }

        transform.position = new Vector3( newX, newY, 0);*/
    }

    public void OnPointerDown(PointerEventData eventData) {
        //TODO: a coroutine to handle specified time to trigger hold
        _isActivated = true;
        _currentPos = _rectTransform.anchoredPosition;
        OnHold?.Invoke(_controllerType);
    }

    public void OnPointerUp(PointerEventData eventData) {
        _isActivated = false;
        OnRelease?.Invoke(_controllerType);
        transform.position = _initialPos;
    }
}
public enum ControllerType {
    LeftController, RightController, Both
}