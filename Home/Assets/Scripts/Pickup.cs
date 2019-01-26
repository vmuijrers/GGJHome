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
        if (joint != null && joint.connectedBody != null) {
            transform.LookAt(joint.connectedBody.transform.position);
            //transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(joint.connectedBody.transform.position - transform.position), 180f *Time.deltaTime);
        }
    }

    public void SetCrabToJoint(Rigidbody rb) {
        joint.connectedBody = rb;
        joint.xMotion = ConfigurableJointMotion.Limited;
        joint.zMotion = ConfigurableJointMotion.Limited;
    }

    public void FreeJoint() {
        joint.xMotion = ConfigurableJointMotion.Free;
        joint.zMotion = ConfigurableJointMotion.Free;
        joint.connectedBody = null;
    }

    public void BreakJoint() {
        Destroy(joint);
    }

    public void OnAttachToShell() {
        Collider[] col = GetComponentsInChildren<Collider>();
        foreach(Collider c in col) {
            c.enabled = false;
        }

        GetComponent<Rigidbody>().isKinematic = true;
    }
}
