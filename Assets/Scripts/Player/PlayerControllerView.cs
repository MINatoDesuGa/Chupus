using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System;
public class PlayerControllerView : MonoBehaviour
{
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
    private void OnControllerHold(ControllerType controllerType) {
        _controllerCollection[controllerType].color = Color.yellow;
    }
    private void OnControllerRelease(ControllerType controllerType) {
        _controllerCollection[controllerType].color = Color.white;
    }
}
[Serializable]
public class Controller<T> {
    public ControllerType Type;
    public T GenericRef;
}