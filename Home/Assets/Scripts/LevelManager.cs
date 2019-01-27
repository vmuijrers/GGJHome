using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using XInputDotNetPure;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    private Pickup pickupPrefab;
    private int amountOfPickups = 10;
    private Vector3 levelBounds = new Vector3(100, 1, 6);

	bool gameRunning = false;

	Crab[] crabs;

	public Image fadeImage;
	public float fadeOutTime = 1.5f;

    void Start()
    {
		StartCoroutine(FadeIn());
        pickupPrefab = Resources.Load<Pickup>("Pickups/Pickup");
        SpawnLevel();
		crabs = FindObjectsOfType<Crab>();
    }

	private void Update ()
	{
		bool oneAlive = false;
		foreach (Crab c in crabs) {
			if (!c.dood)
				oneAlive = true;
		}
		if (!oneAlive) {
			StartCoroutine(FadeOut());
		}

		for (int i = 0; i < 2; i++) {
			GamePadState state = GamePad.GetState((PlayerIndex) i);
			if (state.Buttons.Back == ButtonState.Pressed) {
				SceneManager.LoadScene(0);
			}
		}
	}

	void SpawnLevel() {
        for(int i=0; i < amountOfPickups; i++) {
            Pickup pickup  = Instantiate(pickupPrefab, new Vector3(Random.Range(0, levelBounds.x), Random.Range(1, 30), Random.Range(-levelBounds.z / 2, levelBounds.z / 2)), Quaternion.Euler(0,Random.Range(0,360) ,0));
            pickup.Init(Util.Choose(DecorationType.Basic, DecorationType.Star));
        }
    }

	IEnumerator FadeOut ()
	{
		for (float t = 0; t < 1; t += Time.deltaTime / fadeOutTime) {
			Color c = new Color(0, 0, 0, t);
			fadeImage.color = c;
			yield return null;
		}

		SceneManager.LoadScene(0);
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