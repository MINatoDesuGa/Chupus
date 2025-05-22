using System.Collections;
using UnityEngine;
using UnityEngine.UI;
[RequireComponent(typeof(Slider))]
public class JumpPowerSlider : MonoBehaviour
{
    public static bool IsJumpSliderRunning = false;

    private const float X_POS_OFFSET_PERCENT = 0.05f;
    private const float Y_POS_OFFSET_PERCENT = 0.55f;

    [SerializeField] private Slider _slider;
    [SerializeField] private float _sliderMoveSpeed = 10f;

    private Coroutine _sliderCoroutine;
    private WaitForSeconds _sliderUpdateDelay;
    //=====================================================
    private void OnValidate() {
        _slider = GetComponent<Slider>();
    }
    private void Awake() {
        PlayerControllerInput.OnHold += OnControllerHold;
        PlayerControllerInput.OnRelease += OnControllerRelease;
        gameObject.SetActive(false);
    }
    private void OnDestroy() {
        PlayerControllerInput.OnHold -= OnControllerHold;
        PlayerControllerInput.OnRelease -= OnControllerRelease;
    }
    //=====================================================
    private void OnControllerHold(PlayerControllerInput playerControllerInput) {
        if (playerControllerInput.CurrentActiveAction != PlayerAction.Jump) return;
     //   Debug.Log("jump slider start");
        HandleUIPositioning(playerControllerInput.ControllerType);

        gameObject.SetActive(true);
        ResetCoroutine();
        IsJumpSliderRunning = true;
        _sliderCoroutine = StartCoroutine(PowerSliderTrigger());

        IEnumerator PowerSliderTrigger() { 
            while(true) {
                yield return null;
                GlobalVars.Instance.JumpPower = _slider.value += (_sliderMoveSpeed * Time.deltaTime);
            }
        }
    }
    private void OnControllerRelease(PlayerControllerInput playerControllerInput) {
        if (playerControllerInput.CurrentActiveAction == PlayerAction.Rotate) return;
        gameObject.SetActive(false);
        IsJumpSliderRunning = false;
        _slider.value = 0f;
    }
    private void HandleUIPositioning(ControllerType controllerType) {
        switch (controllerType) {
            case ControllerType.LeftController:
                transform.position = new Vector3(
                    Screen.width * X_POS_OFFSET_PERCENT,
                    Screen.height * Y_POS_OFFSET_PERCENT, 0
                );
                break;
            case ControllerType.RightController:
                transform.position = new Vector3(
                    Screen.width * (1f - X_POS_OFFSET_PERCENT),
                    Screen.height * Y_POS_OFFSET_PERCENT, 0
                );
                break;
        }
    }
    private void ResetCoroutine() {
        if (_sliderCoroutine != null) {
            StopCoroutine(_sliderCoroutine);
            _sliderCoroutine = null;
        }
    }
}
