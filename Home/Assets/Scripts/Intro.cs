using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XInputDotNetPure;
using UnityEngine.SceneManagement;

public class Intro : MonoBehaviour
{
	private void Update ()
	{
		for (int i = 0; i < 2; i++) {
			GamePadState state = GamePad.GetState((PlayerIndex) i);
			if (state.IsConnected) {
				if (state.Buttons.Start == ButtonState.Pressed) {
					SceneManager.LoadScene(1);
				}
			}
		}
	}
}