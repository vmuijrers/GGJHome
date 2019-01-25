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
        //Message.AddListener(MessageEnum.ON_GRAB_SHELL,gameObject, Shake);


    }

    private void OnDisable() {
        //Message.RemoveListenener(MessageEnum.ON_GRAB_SHELL, gameObject, Shake);
    }

}
