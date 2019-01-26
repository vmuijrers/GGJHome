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
		rigidbody = GetComponent<Rigidbody>();
		renderer = GetComponentInChildren<Renderer>();
        cols = GetComponentsInChildren<Collider>();
        baseMat = renderer.material;
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
                    nearestShell.AttachCrab(this);
                }
                IsInShell = true;
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
            //Add pickup to the shell
            if (nearestPickup != null) {
                nearestPickup.FreeJoint();
                nearestPickup.BreakJoint();
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
		leftStickInput = new Vector2(gamePadState.ThumbSticks.Left.X, gamePadState.ThumbSticks.Left.Y);
		leftTrigger = gamePadState.Triggers.Left;
	}

	private void Move (Vector2 direction)
	{
		direction = direction.normalized;
		if (!IsInShell) {
            rigidbody.velocity = new Vector3(direction.x * speed, 0, direction.y * speed);
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
                Tween jumpTween = artReference.transform.DOLocalJump(artReference.transform.localPosition,1,1,0.7f);
                jumpTween.Play();
                deadTween.Play();
            }
			CameraShake.OnShake(.2f, .4f, .05f);
		} else {
            if (nearestShell != null) {
                nearestShell.shell.GetAttacked(damage);
            }
			    
		}
	}

    public void OnRevive() {
        DOTween.KillAll();
        Tween deadTween = artReference.transform.DORotateQuaternion(Quaternion.identity, 0.3f);
        deadTween.Play();
        dood = false;
        //renderer.material = baseMat;
    }
}