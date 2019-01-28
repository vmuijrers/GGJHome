using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using XInputDotNetPure;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
	private Pickup pickupPrefab;
	private int amountOfPickups = 80;
	private Vector3 levelBounds = new Vector3(120, 1, 12);

	public GameObject[] vegetationPrefabs;
	bool gameRunning = false;

	Crab[] crabs;

	public Image fadeImage;
	public float fadeOutTime = 1.5f;

	void Start ()
	{
		StartCoroutine(FadeIn());
		pickupPrefab = Resources.Load<Pickup>("Pickups/Pickup");
		SpawnLevel();
		crabs = FindObjectsOfType<Crab>();
		PopulateStartWithVegetation();
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
                SceneManager.LoadScene(1);
			}
		}
	}

	void SpawnLevel ()
	{
		for (int i = 0; i < amountOfPickups; i++) {
			Pickup pickup = Instantiate(pickupPrefab, new Vector3(Random.Range(-levelBounds.x, levelBounds.x), Random.Range(1, 30), Random.Range(-levelBounds.z / 2, levelBounds.z / 2)), Quaternion.Euler(0, Random.Range(0, 360), 0));
			pickup.Init(Util.GetRandomEnumerator<DecorationType>());
		}
	}

	void PopulateStartWithVegetation ()
	{
		for (int i = 0; i < 14; i++) {
			Vector2 v = Random.insideUnitCircle.normalized;
			Instantiate(Util.Choose(vegetationPrefabs), Vector3.zero + new Vector3(v.x, 0, v.y).normalized * Random.Range(8.0f, 12.0f), Quaternion.identity);
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