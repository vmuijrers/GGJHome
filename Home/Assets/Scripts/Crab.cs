using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XInputDotNetPure;
using DG.Tweening;
public class Crab : MonoBehaviour
{
	public PlayerIndex playerIndex = 0;
	public LayerMask ShellTriggerLayer;
    public LayerMask PickUpTriggerLayer;
    public LayerMask CrabTriggerLayer;
    public GameObject artReference;
	private GamePadState gamePadState;

	private Vector2 leftStickInput;
    private Vector2 rightStickInput;
	private float leftTrigger;
	private Rigidbody rigidbody;
	private bool isNearShell;
	private ShellEntrance nearestShell;
    private Pickup nearestPickup;
    private Crab nearestCrab;

	[Header("Settings")]
	public float speed = 5;
	[SerializeField] private float triggerTreshold = 0.5f;

	Renderer renderer;
	Material baseMat;
	public Material deathMat;
	[HideInInspector]
	public bool dood = false;

    private Collider[] cols;

    //Audio
    private AudioSource aSource;
    private float maxFootStepVolume = 0.4f;
    private GameObject bubbleParticle;

    private bool isInShell = false;
	public bool IsInShell
	{
		get {
			return isInShell;
		}
		set {
			if (value != isInShell) {
				if (value) {
                    OnEnterShell();
                } else {
                    OnExitShell();
                }
			}
			isInShell = value;
		}
	}

	public Vector2 GetLeftStickInput ()
	{
		gamePadState = GamePad.GetState(playerIndex);
		if (gamePadState.IsConnected) {
			return new Vector2(gamePadState.ThumbSticks.Left.X, gamePadState.ThumbSticks.Left.Y);
		}
		return Vector2.zero;
	}
	private void Awake ()
	{
        aSource = GetComponent<AudioSource>();
        aSource.time = Random.Range(0, aSource.clip.length);
        aSource.volume = 0;
        aSource.Play();

        rigidbody = GetComponent<Rigidbody>();
		renderer = GetComponentInChildren<Renderer>();
        cols = GetComponentsInChildren<Collider>();
        baseMat = renderer.material;
        bubbleParticle = (GameObject)Resources.Load("BubbleParticle");
    }

    private void Start() {
        InvokeRepeating("DoBubbles", 8f, 12f);
    }

    private void OnEnterShell() {
        Message.SendMessage(MessageEnum.ON_GRAB_SHELL);
        SetColliders(false);

        rigidbody.isKinematic = true;
    }

    private void OnExitShell() {
        Message.SendMessage(MessageEnum.ON_RELEASE_SHELL);
        SetColliders(true);
        rigidbody.isKinematic = false;
    }

    private void SetColliders(bool value) {
        foreach (Collider c in cols) {
            c.enabled = value;
        }
    }

    private void DoBubbles() {
        Destroy(Instantiate(bubbleParticle, transform.position, Quaternion.Euler(-90,0,0)),10f);
    }

	private void Update ()
	{
		if (dood)
			return;

		gamePadState = GamePad.GetState(playerIndex);
		if (gamePadState.IsConnected) {
			GetInputs();
			Move(leftStickInput);
		}

		//Check if in shell and button pressed
		if (nearestShell != null) {

            if (gamePadState.Triggers.Left > triggerTreshold) {
                if (!IsInShell) {
                    if (nearestShell.AttachCrab(this)) {
                        IsInShell = true;
                    }
                }
                
            } else {
                IsInShell = false;
                nearestShell.DetachCrab(this);
            }

		} else {
			IsInShell = false;
			if (nearestShell != null) {
				nearestShell.DetachCrab(this);
			}
		}

		if (IsInShell) {

            //SpotLight rotation;
            if(rightStickInput != Vector2.zero) {
                nearestShell.shell.SpotLightHolder.transform.rotation =
            Quaternion.RotateTowards(
                nearestShell.shell.SpotLightHolder.transform.rotation,
                Quaternion.Euler(0, Mathf.Atan2(-rightStickInput.y, rightStickInput.x) * Mathf.Rad2Deg + 90, 0), 180f * Time.deltaTime);
            }


            //Add pickup to the shell
            if (nearestPickup != null) {
                nearestPickup.FreeJoint();
                //nearestPickup.BreakJoint();
                nearestPickup.OnAttachToShell();
                nearestShell.shell.AttachPickup(nearestPickup);
                nearestPickup = null;
                
            }
			rigidbody.position = Vector3.Scale(nearestShell.transform.position, new Vector3(1, 0, 1));
        } else {
            if (nearestPickup != null) {
                if (gamePadState.Triggers.Right > triggerTreshold) {
                    nearestPickup.SetCrabToJoint(rigidbody);
                } else {
                    nearestPickup.FreeJoint();
                }
            }
        }

        if(nearestCrab != null && nearestCrab.dood) {
            if(gamePadState.Triggers.Right > triggerTreshold) {
                nearestCrab.OnRevive();
            }
        }
	}

	private void GetInputs ()
	{
        rightStickInput = new Vector2(gamePadState.ThumbSticks.Right.X, gamePadState.ThumbSticks.Right.Y);
        leftStickInput = new Vector2(gamePadState.ThumbSticks.Left.X, gamePadState.ThumbSticks.Left.Y);
		leftTrigger = gamePadState.Triggers.Left;
	}

	private void Move (Vector2 direction)
	{
		direction = direction.normalized;
		if (!IsInShell) {
            rigidbody.velocity = new Vector3(direction.x * speed, 0, direction.y * speed);
            if(rigidbody.velocity.magnitude > 0) {
                aSource.volume = Mathf.Lerp(aSource.volume, maxFootStepVolume, Time.deltaTime);
            } else {
                aSource.volume = Mathf.Lerp(aSource.volume, 0, Time.deltaTime);
            }
            if(direction != Vector2.zero)
                transform.rotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.y), Vector3.up);
        }
    }

	private void OnTriggerEnter (Collider col)
	{
		if ((1 << col.gameObject.layer & ShellTriggerLayer) != 0) {
			Debug.Log("OnTrigger Enter");
			isNearShell = true;
			nearestShell = col.GetComponent<ShellEntrance>();
		}

        if ((1 << col.gameObject.layer & PickUpTriggerLayer) != 0 && nearestPickup == null) {
            nearestPickup = col.GetComponent<Pickup>();
        }

        if((1 << col.gameObject.layer & CrabTriggerLayer) != 0 ) {
            nearestCrab = col.GetComponent<Crab>();
        }
    }

	private void OnTriggerExit (Collider col)
	{
		if ((1 << col.gameObject.layer & ShellTriggerLayer) != 0) {
			Debug.Log("OnTrigger Exit");
			isNearShell = false;
			nearestShell = null;
		}

        if ((1 << col.gameObject.layer & PickUpTriggerLayer) != 0) { 
            if (nearestPickup != null) {
                nearestPickup.FreeJoint();
            }
            nearestPickup = null;
        }
        if ((1 << col.gameObject.layer & CrabTriggerLayer) != 0) {
            if(nearestCrab != null) {
                nearestCrab = null;
            }
            
        }
    }

	public void GetAttacked (int damage)
	{
		if (!IsInShell && !dood) {
			dood = true;
			//renderer.material = deathMat;
            if(artReference != null) {
                Tween deadTween = artReference.transform.DORotateQuaternion(artReference.transform.rotation * Quaternion.Euler(0,0,180), 0.6f);
                Tween jumpTween = artReference.transform.DOLocalJump(artReference.transform.localPosition+ new Vector3(0,0.5f,0),1,1,0.7f);
                jumpTween.Play();
                deadTween.Play();
            }
			CameraShake.OnShake(.2f, .4f, .05f);
		} else {
            if (nearestShell != null) {
                nearestShell.shell.TakeDamage(damage);
            }
			    
		}
	}

    public void OnRevive() {
        DOTween.KillAll();
        Tween deadTween = artReference.transform.DORotateQuaternion(Quaternion.Euler(0,0,0), 0.3f).OnComplete(()=> { artReference.transform.localPosition = new Vector3(0, 0, -.65f); artReference.transform.localRotation = Quaternion.Euler(0, 0, 0); });
        deadTween.Play();
      
        dood = false;
        //renderer.material = baseMat;
    }
}