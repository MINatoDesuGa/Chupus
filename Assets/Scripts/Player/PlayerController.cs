using UnityEngine;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour 
{
    private const float DEFAULT_ROTATION_zVAL = 25f;

    [SerializeField] private List<Controller<GameObject>> _controllers;
    private Dictionary<ControllerType, GameObject> _controllerCollection = new();
    //============================================================
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
    //=============================================================
    private void Init() {
        foreach (var controller in _controllers) {
            _controllerCollection[controller.Type] = controller.GenericRef;
            Debug.Log($"{controller.GenericRef.transform.rotation}");
        }
        _controllers.Clear();
    }
    private void OnControllerHold(ControllerType controllerType) {
        OnActivateController(controllerType, false);
    }
    private void OnControllerRelease(ControllerType controllerType) {
        OnActivateController(ControllerType.Both, true);
    }
    private void OnActivateController(ControllerType controllerType, bool active) {
        switch(controllerType) {
            case ControllerType.Both:
                _controllerCollection[ControllerType.LeftController].SetActive(active);
                _controllerCollection[ControllerType.RightController].SetActive(active);

                _controllerCollection[ControllerType.LeftController].transform.rotation = Quaternion.Euler(new Vector3(0, 0, -DEFAULT_ROTATION_zVAL));
                _controllerCollection[ControllerType.RightController].transform.rotation = Quaternion.Euler(new Vector3(0, 0, DEFAULT_ROTATION_zVAL));
                break;
            case ControllerType.LeftController:
                _controllerCollection[controllerType].SetActive(active);

                _controllerCollection[ControllerType.RightController].transform.rotation = Quaternion.identity;
                break;
            case ControllerType.RightController:
                _controllerCollection[controllerType].SetActive(active);

                _controllerCollection[ControllerType.LeftController].transform.rotation = Quaternion.identity;
                break;
        }
        
    }
    
}
