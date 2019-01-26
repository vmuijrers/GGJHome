using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Transform targetToFollow;
    private float zoomDistance;
    private float zoomIncrease = 1;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnEnable() {
        Message.AddListener(MessageEnum.ON_GRAB_SHELL, gameObject, Shake);
    }

    private void OnDisable() {
        Message.RemoveListener(MessageEnum.ON_GRAB_SHELL, gameObject, Shake);
    }

    private void Shake() {
        Debug.Log("Shake!");
        CameraShake.OnShake(0.3f, 0.5f, 1f);

    }
}
