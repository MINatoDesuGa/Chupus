using UnityEngine;

public class GlobalVars : MonoBehaviour
{
    public static GlobalVars Instance;

    public RectTransform CanvasRectTransform;
    private void Awake() {
        if(Instance == null) { 
            Instance = this; 
        } else {
            Destroy(this);
        }
    }
}
