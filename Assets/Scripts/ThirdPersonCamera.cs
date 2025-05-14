using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour {
    [SerializeField] private Transform _target;           // Player transform
    [SerializeField] private Vector3 _offset = new Vector3(0, 5, -7); // Default offset
    [SerializeField] private float _followSpeed = 10f;    // Smooth follow speed
   // [SerializeField] private float rotationSmoothSpeed = 5f;

    private Vector3 currentVelocity;
    private Vector3 _defaultOffset;
    //====================================================================
    private void Start() {
        _defaultOffset = _offset = transform.position;
    }
    void LateUpdate() {
        if (!_target) return;

       // Debug.Log($"Player rotation: {Vector3.Dot(_target.forward, Vector3.forward)}");
        if (Vector3.Dot(_target.forward, Vector3.forward) < 0) { //looking back
            _offset = _defaultOffset - (Vector3.forward * 2f);
        } else { // looking front
            _offset = _defaultOffset;
        }

        // Smooth position follow
        Vector3 desiredPosition = _target.position + _offset; //target.TransformDirection(offset);
        transform.position = Vector3.SmoothDamp(transform.position, desiredPosition, ref currentVelocity, 1f / _followSpeed);

        // Smoothly rotate the camera to match the target's rotation
        /*Quaternion desiredRotation = Quaternion.LookRotation(target.position - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, desiredRotation, rotationSmoothSpeed * Time.deltaTime);*/
    }
}
