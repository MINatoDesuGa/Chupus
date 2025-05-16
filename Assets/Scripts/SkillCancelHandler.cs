using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class SkillCancelHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {
    /* TODO:
     * On active skill, enable this right on top of the skill button
     * on enter and release on top of this obj, cancel the skill
     */
    public static bool IsSkillCancelled = false;
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
        switch(playerControllerInput.ControllerType) {
            case ControllerType.LeftController:
                transform.position = new Vector3(
                playerControllerInput.transform.position.x + 100f,
                playerControllerInput.transform.position.y + 300f,
                playerControllerInput.transform.position.z
            );
                break;
            case ControllerType.RightController:
                transform.position = new Vector3(
                playerControllerInput.transform.position.x - 100f,
                playerControllerInput.transform.position.y + 300f,
                playerControllerInput.transform.position.z
            );
                break;
        }
        
        gameObject.SetActive(true);
    }
    private void OnControllerRelease(PlayerControllerInput playerControllerInput) {
        if (playerControllerInput.CurrentActiveAction == PlayerAction.Rotate) return;
        _skillCancelImg.color = Color.white;
        gameObject.SetActive(false);
    }
}
public enum Skills {
    None, Jump
}