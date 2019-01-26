using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    private ConfigurableJoint joint;
    // Start is called before the first frame update
    void Start()
    {
        joint = GetComponent<ConfigurableJoint>();
    }

    // Update is called once per frame
    void Update()
    {
        if(joint.connectedBody != null) {
            transform.LookAt(joint.connectedBody.transform.position);
            //transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(joint.connectedBody.transform.position - transform.position), 180f *Time.deltaTime);
        }
    }
}
