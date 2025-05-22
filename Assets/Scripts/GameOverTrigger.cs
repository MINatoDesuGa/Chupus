using System;
using UnityEngine;
[RequireComponent (typeof(Collider))]
public class GameOverTrigger : MonoBehaviour
{
    public static event Action OnPlayerOut;
    private void OnCollisionEnter(Collision collision) {
        OnPlayerOut?.Invoke();
    }
}
