using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class SkillCancelHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {
    /* 
     * On active skill, enable this right on top of the skill button
     * on enter and release on top of this obj, cancel the skill
     */
    public static bool IsSkillCancelled = false;

    private const float X_POS_OFFSET_PERCENT = 0.25f;
    private const float Y_POS_OFFSET_PERCENT = 0.75f;

    [SerializeField] private Image _skillCancelImg;
    //===============================================================================
    private void OnValidate() {
        _skillCancelImg = GetComponent<Image>();
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
    //================================================================================
    public void OnPointerEnter(PointerEventData eventData) {
        _skillCancelImg.color = Color.red;
        IsSkillCancelled = true;
    }
    public void OnPointerExit(PointerEventData eventData) {
        _skillCancelImg.color = Color.white;
        IsSkillCancelled = false;
    }
    private void OnControllerHold(PlayerControllerInput playerControllerInput) {
        if (playerControllerInput.CurrentActiveAction == PlayerAction.Rotate) return;
        HandleUIPositioning(playerControllerInput.ControllerType);
        
        gameObject.SetActive(true);
    }
    private void OnControllerRelease(PlayerControllerInput playerControllerInput) {
        if (playerControllerInput.CurrentActiveAction == PlayerAction.Rotate) return;
        _skillCancelImg.color = Color.white;
        gameObject.SetActive(false);
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
                Screen.width * (1-X_POS_OFFSET_PERCENT),
                Screen.height * Y_POS_OFFSET_PERCENT, 0
            );
                break;
        }
    }
}
public enum Skills {
    None, Jump
}