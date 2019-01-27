using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XInputDotNetPure;
using System.Linq;
using DG.Tweening;

public class Shell : MonoBehaviour {

    private int size = 1;
    private ShellEntrance entrance;
    public float speedModifier = 10;
    public float lightStrength = 0.5f;
    public GameObject visualBottom, visualTop;
    private Pickup pickupPrefab;

    [Header("References")]
    public Transform SpotLightHolder;
    public Transform shellVisual;

    [HideInInspector] public MeshRenderer renderer;

    private Rigidbody rigidBody;
    private Vector2 direction;
    private float rotationSpeed = 180;
    private Light light;
    private int numCrabs = 0;
    private List<PickupSlot> decorations = new List<PickupSlot>();
    private bool isAnimating;

    private void Awake() {
        renderer = GetComponentInChildren<MeshRenderer>();
        rigidBody = GetComponent<Rigidbody>();
        entrance = GetComponentInChildren<ShellEntrance>();
        light = GetComponentInChildren<Light>();
        entrance.Init(this);
        decorations = GetComponentsInChildren<PickupSlot>().ToList();
        transform.localScale = new Vector3(size, size, size);
        pickupPrefab = Resources.Load<Pickup>("Pickups/Pickup");
        StartCoroutine(PulseLight());

        foreach (PickupSlot slot in decorations) {
            Pickup pickup = Instantiate(pickupPrefab, slot.transform.position, slot.transform.rotation);
            pickup.Init(DecorationType.Basic);
            pickup.OnAttachToShell();
            AttachPickup(pickup);
        }

    }
    public int GetMaxHP() {
        return decorations.Count;
    }
    public int GetHPLeft() {
        return decorations.Count(x => x.reference != null);
    }
    private void Update() {
        if (Input.GetKeyDown(KeyCode.Space)) {
            TakeDamage(1);
        }
    }

    private void FixedUpdate() {


        int crabCount = entrance.attachedCrabs.Count;
        if (crabCount > 0) {
            SpotLightHolder.GetComponentInChildren<SpotLight>().ActivateLight();
            rigidBody.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionY;
            Vector2 dir = Vector2.zero;
            foreach(Crab c in entrance.attachedCrabs) {
                dir += c.GetLeftStickInput();
            }
            dir /= crabCount;
            Animate(dir);
            DoMove(dir, speedModifier);
            rigidBody.velocity = Vector3.zero;
        } else {
            
            rigidBody.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ;
        }
    }

    private void Animate(Vector2 dir) {
        if(dir != Vector2.zero) {
            if (!isAnimating) {
                isAnimating = true;
                shellVisual.transform.DOPunchRotation(new Vector3(0, 0, 15), 0.5f, 0, 0).OnComplete(() => { isAnimating = false; });
            }
            
        }
    }
    public void DoMove(Vector2 dir, float speedModifier) {
        //rigidBody.velocity = new Vector3(dir.x, 0, dir.y) * speedModifier;
        rigidBody.MovePosition(rigidBody.position + new Vector3(dir.x, 0, dir.y) * speedModifier * Time.deltaTime);
        if (dir.magnitude != 0) {
            rigidBody.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(new Vector3(dir.x, 0, dir.y), Vector3.up), rotationSpeed * Time.deltaTime);
        }
            
    }

	public void TakeDamage (int damage)
	{
        CameraShake.OnShake(0.4f, 0.3f, 0.01f);
        if(GetHPLeft() > 0) {
            DetachRandomPickup();
        } else {
            BreakShell();
        }
	}

    void BreakShell() {
        Debug.Log("Shell Broken");
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
    public void DetachRandomPickup() {
        foreach(PickupSlot obj in decorations) {
            if(obj.reference != null) {
                Pickup pickup = obj.reference.GetComponent<Pickup>();
                pickup.OnDetachFromShell();
                obj.reference = null;
                break;
            }
        }
    }

    public IEnumerator PulseLight() {
        float baseIntensity = light.intensity;
        while(true) {
            float value = Mathf.PingPong(Time.time, lightStrength);
            light.intensity = baseIntensity + value;
            yield return null;
        }
    }

    public void UpdateCrabVisuals(int crabCount) {
        if(visualBottom)
            visualBottom.SetActive(crabCount > 0 ? true :  false);
        if(visualTop)
            visualTop.SetActive(crabCount > 1 ? true : false);

        if(crabCount == 0) {
            SpotLightHolder.GetComponentInChildren<SpotLight>().DeActivateLight();
        } else {
            SpotLightHolder.GetComponentInChildren<SpotLight>().ActivateLight();
        }
    }
}