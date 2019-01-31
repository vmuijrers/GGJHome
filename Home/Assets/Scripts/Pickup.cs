using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DecorationType {
    Star,
    Barny,
    Crayon,
    Football,
    LightHouse,
    Logo,
    Pan,
    Pearl,
    Shell,
    Shrimp,
    Treasure,
    Urchin,
    Wheel
}
public class Pickup : MonoBehaviour
{
    public DecorationType type;
    private GameObject artRef;
    private bool isInitialized = false;
    private Rigidbody rigidbody;
    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        if (!isInitialized) {
            Init(Util.GetRandomEnumerator<DecorationType>());
        }

    }
    public void Init(DecorationType type) {
        artRef = (GameObject)Instantiate(Resources.Load("Pickups/"+type.ToString()), transform.position, transform.rotation);
        artRef.transform.SetParent(transform);
        artRef.transform.localPosition = Vector3.zero;
        artRef.transform.localRotation = Quaternion.identity;
        isInitialized = true;
    }

    public void OnPickup(Transform newParent) {
        rigidbody.isKinematic = true;
        Collider[] col = GetComponentsInChildren<Collider>();
        foreach (Collider c in col) {
            c.enabled = false;
        }
        transform.SetParent(newParent);
        transform.localPosition = new Vector3(0, 0, 1);
    }

    public void OnRelease() {
        transform.SetParent(null);
        Collider[] col = GetComponentsInChildren<Collider>();
        foreach (Collider c in col) {
            c.enabled = true;
        }
        rigidbody.isKinematic = false;
    }

    public void OnAttachToShell() {
        Collider[] col = GetComponentsInChildren<Collider>();
        foreach(Collider c in col) {
            c.enabled = false;
        }
        GetComponent<Rigidbody>().isKinematic = true;
    }

    public void OnDetachFromShell() {
        transform.SetParent(null);
        Collider[] col = GetComponentsInChildren<Collider>();
        foreach (Collider c in col) {
            c.enabled = true;
        }
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.isKinematic = false;
        rb.AddForce(new Vector3(Random.Range(-1,1),1, Random.Range(-1, 1)).normalized * Random.Range(2000,3000));
        rb.AddTorque(Random.insideUnitSphere * Random.Range(5,10));
    }
}
