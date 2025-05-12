using UnityEngine;
[RequireComponent(typeof(RectTransform))]
public class ResponsiveUIHandler : MonoBehaviour {
    private RectTransform _targetRectTransform;
    [Tooltip("Direction to offset w.r.t transform anchor")]
    [SerializeField] private TargetScale _targetScale;
    [Tooltip("Offset % w.r.t target screen resolution")]
    [Range(0.1f, 100f)]
    [SerializeField] private float OffsetPercent;
    //==========================================================
#if UNITY_EDITOR //only for dev purpose
    private void OnValidate() {
        _targetRectTransform = GetComponent<RectTransform>();
        OffsetPercent = (_targetRectTransform.rect.width/1080)*100; 
    }
#endif
    private void Start() {
        _targetRectTransform = GetComponent<RectTransform>();
        AdjustUI();
      //  Debug.Log($"screen res : {Screen.currentResolution}");
    }
    //==========================================================
    private void AdjustUI() {
        float offsetVal;
        switch(_targetScale) { 
            case TargetScale.Width: 
                offsetVal = GlobalVars.Instance.CanvasRectTransform.rect.width * (OffsetPercent/100);
                _targetRectTransform.sizeDelta = new Vector2( offsetVal , _targetRectTransform.sizeDelta.y);
                break;
            case TargetScale.Height:
                offsetVal = GlobalVars.Instance.CanvasRectTransform.rect.height * (OffsetPercent / 100);
                _targetRectTransform.sizeDelta = new Vector2(_targetRectTransform.sizeDelta.x, offsetVal);
                break;
        }
    }
}
public enum TargetScale {
    Width, Height
}


