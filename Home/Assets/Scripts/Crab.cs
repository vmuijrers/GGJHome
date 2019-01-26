using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XInputDotNetPure;

public class Crab : MonoBehaviour
{
	public PlayerIndex playerIndex = 0;
	public LayerMask ShellTriggerLayer;
    public LayerMask PickUpTriggerLayer;
	private GamePadState gamePadState;

	private Vector2 leftStickInput;
	private float leftTrigger;
	private Rigidbody rigidbody;
	private bool isNearShell;
	private ShellEntrance nearestShell;
    private Pickup nearestPickup;

	[Header("Settings")]
	public float speed = 5;
	[SerializeField] private float triggerTreshold = 0.5f;

	Renderer renderer;
	Material baseMat;
	public Material deathMat;
	[HideInInspector]
	public bool dood = false;

	private bool isInShell = false;
	public bool IsInShell
	{
		get {
			return isInShell;
		}
		set {
			if (value != isInShell) {
				if (value) {
					Message.SendMessage(MessageEnum.ON_GRAB_SHELL);
				} else {
					Message.SendMessage(MessageEnum.ON_RELEASE_SHELL);
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
		baseMat = renderer.material;
	}

	private void Update ()
	{
		gamePadState = GamePad.GetState(playerIndex);
		if (gamePadState.IsConnected) {
			GetInputs();
			Move(leftStickInput);
		}

		//Check if in shell and button pressed
		if (nearestShell != null && gamePadState.Triggers.Left > triggerTreshold) {
			if (!nearestShell.attachedCrab) {
				nearestShell.AttachCrab(this);
			}
			IsInShell = true;

		} else {
			IsInShell = false;
			if (nearestShell != null) {
				nearestShell.DetachCrab(this);
			}
		}

		if (IsInShell) {
			rigidbody.position = Vector3.Scale(nearestShell.transform.position, new Vector3(1, 0, 1));

            //rig.velocity = (targetPosShell.transform.position - transform.position).normalized * 3f;
        } else {
            if (nearestPickup != null) {

                if (gamePadState.Triggers.Right > triggerTreshold) {
                    nearestPickup.SetCrabToJoint(rigidbody);
                } else {
                    nearestPickup.FreeJoint();
                }
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
		if (!IsInShell)
			rigidbody.velocity = new Vector3(direction.x * speed, 0, direction.y * speed);
        transform.rotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.y),Vector3.up);
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
    }

	public void GetAttacked (int damage)
	{
		if (!IsInShell) {
			dood = true;
		} else {
			nearestShell.shell.GetAttacked(damage);
		}
	}
}