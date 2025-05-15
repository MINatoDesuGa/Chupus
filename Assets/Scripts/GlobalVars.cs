using UnityEngine;

public class GlobalVars : MonoBehaviour
{
    public static GlobalVars Instance;

    public static float MAX_JUMP_DISTANCE = 3f;

    public float JumpPower = 0f;

    public RectTransform CanvasRectTransform;
    private void Awake() {
        if(Instance == null) { 
            Instance = this; 
        } else {
            Destroy(this);
        }
    }
}
