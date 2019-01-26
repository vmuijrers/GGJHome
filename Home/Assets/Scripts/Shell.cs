using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XInputDotNetPure;
using System.Linq;
using DG.Tweening;

public class Shell : MonoBehaviour {

    public int size = 1;
    public ShellEntrance entrance;
    public float speedModifier = 10;
    public float strength = 0.5f;
    public GameObject visualBottom, visualTop;
    

    [HideInInspector] public MeshRenderer renderer;

    private Rigidbody rigidBody;
    private Vector2 direction;
    private float rotationSpeed = 180;
    private Light light;
    private int numCrabs = 0;
    private List<PickupSlot> decorations = new List<PickupSlot>();

    private void Awake() {
        renderer = GetComponentInChildren<MeshRenderer>();
        rigidBody = GetComponent<Rigidbody>();
        entrance = GetComponentInChildren<ShellEntrance>();
        light = GetComponentInChildren<Light>();
        entrance.Init(this);
        decorations = GetComponentsInChildren<PickupSlot>().ToList();
        transform.localScale = new Vector3(size, size, size);
        StartCoroutine(PulseLight());

    }
    private void Update() {

        int crabCount = entrance.attachedCrabs.Count;
        if (crabCount > 0) {
            rigidBody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezePositionY;
            Vector2 dir = Vector2.zero;
            foreach(Crab c in entrance.attachedCrabs) {
                dir += c.GetLeftStickInput();
            }
            dir /= crabCount;
            DoMove(dir, crabCount * speedModifier);

        } else {
            rigidBody.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ;
        }
    }

    public void DoMove(Vector2 dir, float speedModifier) {
        //rigidBody.velocity = new Vector3(dir.x, 0, dir.y) * speedModifier;
        rigidBody.MovePosition(rigidBody.position + new Vector3(dir.x, 0, dir.y) * speedModifier * Time.deltaTime);
        if (dir.magnitude != 0) {
            rigidBody.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(new Vector3(dir.x, 0, dir.y), Vector3.up), rotationSpeed * Time.deltaTime);
        }
            
    }

	public void GetAttacked (int damage)
	{

	}

    public void AttachPickup(Pickup pickup) {
        foreach(PickupSlot obj in decorations) {
            if(obj.reference == null) {
                obj.reference = pickup.gameObject;
                pickup.transform.SetParent(obj.transform,false);
                pickup.transform.localPosition = Vector3.zero;
                pickup.transform.localRotation = Quaternion.identity;
                break;
            }
        }
        
    }

    public IEnumerator PulseLight() {
        float baseIntensity = light.intensity;
        while(true) {
            float value = Mathf.PingPong(Time.time, strength);
            light.intensity = baseIntensity + value;
            yield return null;
        }
    }

    public void UpdateCrabVisuals(int crabCount) {
        if(visualBottom)
            visualBottom.SetActive(crabCount > 0 ? true :  false);
        if(visualTop)
            visualTop.SetActive(crabCount > 1 ? true : false);
    }
}