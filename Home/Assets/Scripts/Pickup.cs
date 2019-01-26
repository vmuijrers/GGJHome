using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DecorationType { Basic, Star }
public class Pickup : MonoBehaviour
{
    public DecorationType type;
    private ConfigurableJoint joint;
    private GameObject artRef;
    // Start is called before the first frame update
    void Start()
    {
        joint = GetComponent<ConfigurableJoint>();


    }
    public void Init(DecorationType type) {
        switch (type) {
            case DecorationType.Basic: artRef = (GameObject)Instantiate(Resources.Load("Pickups/Basic"), transform.position, transform.rotation); break;
            case DecorationType.Star: artRef = (GameObject)Instantiate(Resources.Load("Pickups/Star"), transform.position, transform.rotation); break;
        }
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

    public void OnDetachFromShell() {
        Collider[] col = GetComponentsInChildren<Collider>();
        foreach (Collider c in col) {
            c.enabled = true;
        }

        GetComponent<Rigidbody>().isKinematic = false;
        GetComponent<Rigidbody>().AddForce(new Vector3(Random.Range(-1,1),1, Random.Range(-1, 1)) * 100);
    }
}
