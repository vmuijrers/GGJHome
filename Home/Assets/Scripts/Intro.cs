using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XInputDotNetPure;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Intro : MonoBehaviour
{
	public float fadeOutTime = 1.5f;
	public Image fadeImage;

	private void Start ()
	{
		StartCoroutine(FadeIn());
	}

	private void Update ()
	{
		for (int i = 0; i < 2; i++) {
			GamePadState state = GamePad.GetState((PlayerIndex) i);
			if (state.IsConnected) {
				if (state.Buttons.Start == ButtonState.Pressed) {
					StartCoroutine(FadeOut());
				}
			}
		}
	}

	IEnumerator FadeOut ()
	{
		for (float t = 0; t < 1; t += Time.deltaTime / fadeOutTime) {
			Color c = new Color(0, 0, 0, t);
			fadeImage.color = c;
			yield return null;
		}

		SceneManager.LoadScene(1);
	}

	IEnumerator FadeIn ()
	{
		for (float t = 1; t > 0; t -= Time.deltaTime / fadeOutTime) {
			Color c = new Color(0, 0, 0, t);
			fadeImage.color = c;
			yield return null;
		}
	}
}