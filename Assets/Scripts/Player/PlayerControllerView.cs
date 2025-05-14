using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System;
public class PlayerControllerView : MonoBehaviour
{
    private const float SCALE_DOWN_VAL = 0.5f;

    [SerializeField] private List<Controller<Image>> _controllers;
    private Dictionary<ControllerType, Image> _controllerCollection = new();
    //===============================================================
    private void Start() {
        Init();
    }
    private void OnEnable() {
        PlayerControllerInput.OnHold += OnControllerHold;
        PlayerControllerInput.OnRelease += OnControllerRelease;
    }
    private void OnDisable() {
        PlayerControllerInput.OnHold -= OnControllerHold;
        PlayerControllerInput.OnRelease -= OnControllerRelease;
    }
    //===============================================================
    private void Init() {
        foreach (var controller in _controllers) {
            _controllerCollection[controller.Type] = controller.GenericRef;   
        }
        _controllers.Clear();
    }
    private void OnControllerHold(ControllerType controllerType, PlayerAction playerAction) {
        switch(playerAction) {
            case PlayerAction.Rotate:
                _controllerCollection[controllerType].color = Color.yellow;
                _controllerCollection[controllerType].transform.localScale = Vector3.one * SCALE_DOWN_VAL;
                break;
            case PlayerAction.Jump:
                _controllerCollection[controllerType].color = Color.red;
                break;
        }
        
    }
    private void OnControllerRelease(ControllerType controllerType, PlayerAction playerAction) {
        _controllerCollection[controllerType].color = Color.white;
        _controllerCollection[controllerType].transform.localScale = Vector3.one;
    }
}
[Serializable]
public class Controller<T> {
    public ControllerType Type;
    public T GenericRef;
}