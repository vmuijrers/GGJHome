using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XInputDotNetPure;

public class CrabMove : MonoBehaviour
{
	public int playerIndex = 0;
	GamePadState gamePadState;

	Vector2 leftStickInput;

	Rigidbody rig;

	[Header("Settings")]
	public float speed = 5;

	private void Awake ()
	{
		rig = GetComponent<Rigidbody>();
	}

	private void Update ()
	{
		gamePadState = GamePad.GetState((PlayerIndex) playerIndex);
		if (gamePadState.IsConnected) {
			GetInputs();
			Debug.Log(leftStickInput + " " + playerIndex);
			
			Move(leftStickInput);
		}
	}

	void GetInputs ()
	{
		leftStickInput = new Vector2(gamePadState.ThumbSticks.Left.X, gamePadState.ThumbSticks.Left.Y);
	}

	void Move (Vector2 direction)
	{
		direction = direction.normalized;
		rig.velocity = new Vector3(direction.x * speed, 0, direction.y * speed);
	}
}