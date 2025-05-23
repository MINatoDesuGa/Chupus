using UnityEngine;
using System.Collections.Generic;
using DG.Tweening;

public class PlayerController : MonoBehaviour 
{
    private const float DEFAULT_ROTATION_zVAL = 25f;
    private const float GROUND_CHECK_RADIUS = 1f; //sphere radius of groundCheck obj

    [SerializeField] private List<Controller<GameObject>> _controllers;
    [SerializeField] private LayerMask _groundCheckLayer;
    private Dictionary<ControllerType, GameObject> _controllerCollection = new();

    [SerializeField] private Rigidbody _rigidBody;

    private float _jumpDistance;
    private bool _isJumping = false;
    private bool _isGrounded = false;
    private Vector3 _startPos;
    //============================================================
    private void OnValidate() {
        if(_rigidBody == null) _rigidBody = GetComponent<Rigidbody>();
    }
    private void Start() {
        Init();
    }
    private void Update() {
        if (Input.GetKeyDown(KeyCode.Space)) {
            NewJump();
            //Jump();
        }
        ///TODO: change this to event based
        Rotate();
    }
    private void FixedUpdate() {
        GroundedCheck();
    }
    private void OnEnable() {
        PlayerControllerInput.OnHold += OnControllerHold;
        PlayerControllerInput.OnRelease += OnControllerRelease;
        GameOverTrigger.OnPlayerOut += OnPlayerOut;
    }
    private void OnDisable() {
        PlayerControllerInput.OnHold -= OnControllerHold;
        PlayerControllerInput.OnRelease -= OnControllerRelease;
        GameOverTrigger.OnPlayerOut -= OnPlayerOut;
    }
    //=============================================================
    private void GroundedCheck() {
        _isGrounded = Physics.CheckSphere(  _controllerCollection[ControllerType.LeftController].transform.position, 
                                            GROUND_CHECK_RADIUS, _groundCheckLayer);
        if (_isGrounded) return;
        _isGrounded = Physics.CheckSphere(_controllerCollection[ControllerType.RightController].transform.position,
                                            GROUND_CHECK_RADIUS, _groundCheckLayer);

     //   Debug.Log($"is grounded: {_isGrounded}");
    }
    private void NewJump() {
        if (!_isGrounded) return;
        _rigidBody.AddForce((Vector3.up * 5f) + (transform.forward * _jumpDistance), ForceMode.Impulse);
    }
    private void Init() {
        _startPos = transform.position;
        foreach (var controller in _controllers) {
            _controllerCollection[controller.Type] = controller.GenericRef;
            Debug.Log($"{controller.GenericRef.transform.rotation}");
        }
        _controllers.Clear();
    }
    private void OnControllerHold(PlayerControllerInput playerControllerInput) {
        switch(playerControllerInput.CurrentActiveAction) {
            case PlayerAction.Rotate:
                OnActivateController(playerControllerInput.ControllerType, false);
                break;
            case PlayerAction.Jump:
                ///TODO: power slider enable (Maybe cancel based or loop slider low to high)
                break;
        }
        
    }
    private void OnControllerRelease(PlayerControllerInput playerControllerInput) {
        switch(playerControllerInput.CurrentActiveAction) {
            case PlayerAction.Rotate:
                OnActivateController(ControllerType.Both, true);
                break;
            case PlayerAction.Jump2:
            case PlayerAction.Jump:
                if (SkillCancelHandler.IsSkillCancelled) {
                    SkillCancelHandler.IsSkillCancelled = false;
                    return;
                }
                _jumpDistance = GlobalVars.Instance.JumpPower;
                GlobalVars.Instance.JumpPower = 0;
                NewJump();
                //Jump();
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
    private void OnPlayerOut() {
        transform.DOMove(_startPos, 1f).SetEase(Ease.OutSine);
    }
    private void Rotate() {
        var input = PlayerControllerInput.InputDirection;
        Vector3 direction = new Vector3(input.x, 0f, input.y); // convert 2D input to 3D direction
        if (direction != Vector3.zero) {
            Quaternion targetRotation = Quaternion.LookRotation(direction, transform.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10f);
        }
    }
    private void Jump() {
        if(_isJumping) return;

        _isJumping = true;
        transform.DOJump(transform.position + (transform.forward * _jumpDistance), 1f, 1, .5f)
        .OnComplete(() => _isJumping = false);
    }
}
