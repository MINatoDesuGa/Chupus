using UnityEngine;
using System.Collections.Generic;
using DG.Tweening;

public class PlayerController : MonoBehaviour 
{
    private const float DEFAULT_ROTATION_zVAL = 25f;

    [SerializeField] private List<Controller<GameObject>> _controllers;
    private Dictionary<ControllerType, GameObject> _controllerCollection = new();

    private CharacterController _characterController;

    private bool _isJumping = false;
    //============================================================
    private void Start() {
        Init();
    }
    private void Update() {
        if (Input.GetKeyDown(KeyCode.Space)) {
            Jump();
        }
        ///TODO: change this to event based
        Rotate();
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
        _characterController = GetComponent<CharacterController>();
        foreach (var controller in _controllers) {
            _controllerCollection[controller.Type] = controller.GenericRef;
            Debug.Log($"{controller.GenericRef.transform.rotation}");
        }
        _controllers.Clear();
    }
    private void OnControllerHold(ControllerType controllerType, PlayerAction playerAction) {
        switch(playerAction) {
            case PlayerAction.Rotate:
                OnActivateController(controllerType, false);
                break;
            case PlayerAction.Jump:
                ///TODO: power slider enable (Maybe cancel based or loop slider low to high)
                break;
        }
        
    }
    private void OnControllerRelease(ControllerType controllerType, PlayerAction playerAction) {
        switch(playerAction) {
            case PlayerAction.Rotate:
                OnActivateController(ControllerType.Both, true);
                break;
            case PlayerAction.Jump: 
                Jump();
                break;
        }
        
    }
    private void OnActivateController(ControllerType controllerType, bool active) {
        switch(controllerType) {
            case ControllerType.Both:
                _controllerCollection[ControllerType.LeftController].SetActive(active);
                _controllerCollection[ControllerType.RightController].SetActive(active);

                _controllerCollection[ControllerType.LeftController].transform.localRotation = Quaternion.Euler(new Vector3(0, 0, -DEFAULT_ROTATION_zVAL));
                _controllerCollection[ControllerType.RightController].transform.localRotation = Quaternion.Euler(new Vector3(0, 0, DEFAULT_ROTATION_zVAL));
                break;
            case ControllerType.LeftController:
                _controllerCollection[controllerType].SetActive(active);

                _controllerCollection[ControllerType.RightController].transform.localRotation = Quaternion.identity;
                break;
            case ControllerType.RightController:
                _controllerCollection[controllerType].SetActive(active);

                _controllerCollection[ControllerType.LeftController].transform.localRotation = Quaternion.identity;
                break;
        }
        
    }
    private void Rotate() {
        var input = PlayerControllerInput._currentPos;
        Vector3 direction = new Vector3(input.x, 0f, input.y); // convert 2D input to 3D direction
        if (direction != Vector3.zero) {
            Quaternion targetRotation = Quaternion.LookRotation(direction, transform.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10f);
        }
    }
    private void Jump() {
        if(_isJumping) return;

        _isJumping = true;
        transform.DOJump(transform.position + (transform.forward * 2f), 1f, 1, .5f).OnComplete(() => _isJumping = false);
    }
}
