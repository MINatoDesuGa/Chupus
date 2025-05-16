using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class SkillCancelHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {
    /* TODO:
     * On active skill, enable this right on top of the skill button
     * on enter and release on top of this obj, cancel the skill
     */
    [SerializeField] private Image _skillCancelImg;
    private bool _canCancelSkill = false;
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
        _canCancelSkill = true;
    }
    public void OnPointerExit(PointerEventData eventData) {
        _skillCancelImg.color = Color.white;
        _canCancelSkill = false;
    }
    private void OnControllerHold(PlayerControllerInput playerControllerInput) {
        if (playerControllerInput.CurrentActiveAction == PlayerAction.Rotate) return;
        gameObject.SetActive(true);
    }
    private void OnControllerRelease(PlayerControllerInput playerControllerInput) {
        if (playerControllerInput.CurrentActiveAction == PlayerAction.Rotate) return;
        if (_canCancelSkill) {
            ///TODO: handle cancel skill 
            _skillCancelImg.color = Color.white;
            _canCancelSkill = false;
            Debug.Log("skill cancel trigger");
        }
        gameObject.SetActive(false);
    }
}
public enum Skills {
    None, Jump
}