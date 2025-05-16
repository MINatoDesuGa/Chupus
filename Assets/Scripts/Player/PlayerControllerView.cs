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
    private void OnControllerHold(PlayerControllerInput playerControllerInput) {
        switch(playerControllerInput.CurrentActiveAction) {
            case PlayerAction.Rotate:
                _controllerCollection[playerControllerInput.ControllerType].color = Color.yellow;
                _controllerCollection[playerControllerInput.ControllerType].transform.localScale = Vector3.one * SCALE_DOWN_VAL;
                break;                
            case PlayerAction.Jump:  
                _controllerCollection[playerControllerInput.ControllerType].color = Color.red;
                break;
        }
        
    }
    private void OnControllerRelease(PlayerControllerInput playerControllerInput) {
        if (playerControllerInput.CurrentActiveAction == PlayerAction.Rotate) {
            _controllerCollection[playerControllerInput.ControllerType].color = Color.white;
            _controllerCollection[playerControllerInput.ControllerType].transform.localScale = Vector3.one;
            return;
        }

        _controllerCollection[playerControllerInput.ControllerType].color = Color.white;
    }
}
[Serializable]
public class Controller<T> {
    public ControllerType Type;
    public T GenericRef;
}