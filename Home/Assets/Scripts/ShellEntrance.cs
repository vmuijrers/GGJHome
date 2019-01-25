using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XInputDotNetPure;

public class ShellEntrance : MonoBehaviour
{
	Shell shell;
	bool crabAttached = false;
	Transform attachedCrab = null;

	private void Awake ()
	{
		shell = transform.parent.GetComponent<Shell>();
	}

	private void OnTriggerStay (Collider other)
	{
		if (other.CompareTag("Player") && !crabAttached) {
			PlayerIndex crabIndex = other.GetComponent<CrabMove>().playerIndex;
			GamePadState state = GamePad.GetState(crabIndex);
			if (state.Triggers.Left > .5f) {
				shell.CrabEnters(crabIndex);
			}
		}
	}

	private void FixedUpdate ()
	{
		if (crabAttached) {

		}
	}
}