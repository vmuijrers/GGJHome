using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShrimp : MonoBehaviour, IEnemy
{
	Renderer renderer;
	Transform[] shrimpTransforms;

	Crab[] targetCrabScripts;

	public float speed = 7, secondsPerMovementUpdate = .5f, contactDistance = 1.25f;
	public int damage = 1;

	public float maxTurnRate = 360;
	float turnRate = 0;

	public float shrimpSpinRate = 180;

	public void Init (Crab targetCrab)
	{
		renderer = transform.GetChild(0).GetComponent<Renderer>();
		targetCrabScripts = FindObjectsOfType<Crab>();

		StartCoroutine(Move());

		shrimpTransforms = new Transform[transform.childCount];
		for (int i = 0; i < transform.childCount; i++) {
			shrimpTransforms[i] = transform.GetChild(i);
		}
		StartCoroutine(SpinningShrimp());
	}

	IEnumerator Move ()
	{
		StartCoroutine(CheckForCrab());
		transform.LookAt(Util.Choose(targetCrabScripts).transform);

		float timer = 0;
		bool hasBeenVisible = false;
		while (true) {
			if (renderer.isVisible)
				hasBeenVisible = true;
			else if (hasBeenVisible)
				break;

			transform.Rotate(0, turnRate * Time.deltaTime, 0);
			transform.position += transform.forward * speed * Time.deltaTime;
			timer += Time.deltaTime;
			if (timer >= secondsPerMovementUpdate) {
				turnRate = Random.Range(-maxTurnRate, maxTurnRate);
				timer = 0;
			}
			yield return null;
		}

		Destroy(gameObject);
	}

	IEnumerator CheckForCrab ()
	{
		while (true) {
			for (int i = 0; i < targetCrabScripts.Length; i++) {
				if (Util.SquareDistance(transform.position, targetCrabScripts[i].transform.position) < contactDistance * contactDistance) {
					targetCrabScripts[i].GetAttacked(damage);
				}
			}
			yield return null;
		}
	}

	IEnumerator SpinningShrimp ()
	{
		while (true) {
			foreach (Transform t in shrimpTransforms) {
				t.Rotate(0, 0, -shrimpSpinRate * Time.deltaTime);
			}
			yield return null;
		}
	}
}