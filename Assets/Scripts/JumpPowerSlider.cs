using System.Collections;
using UnityEngine;
using UnityEngine.UI;
[RequireComponent(typeof(Slider))]
public class JumpPowerSlider : MonoBehaviour
{
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
    private void OnControllerHold(ControllerType controllerType, PlayerAction playerAction) {
        if (playerAction != PlayerAction.Jump) return;
        gameObject.SetActive(true);
        ResetCoroutine();
        _sliderCoroutine = StartCoroutine(PowerSliderTrigger());

        IEnumerator PowerSliderTrigger() { 
            while(true) {
                yield return null;
                GlobalVars.Instance.JumpPower = _slider.value += (_sliderMoveSpeed * Time.deltaTime);
            }
        }
    }
    private void OnControllerRelease(ControllerType controllerType, PlayerAction playerAction) {
        if (playerAction != PlayerAction.Jump) { return; }
        ///TODO: pass slider value to jump power , perform jump then reset
        _slider.value = 0f;
        gameObject.SetActive(false);
    }
    private void ResetCoroutine() {
        if (_sliderCoroutine != null) {
            StopCoroutine(_sliderCoroutine);
            _sliderCoroutine = null;
        }
    }
}
