using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShrimp : MonoBehaviour, IEnemy
{
	Renderer renderer;

	Transform[] targetCrabTransforms;
	Crab[] targetCrabScripts;

	public float speed = 6, secondsPerMovementUpdate = 1.2f, contactDistance = .5f;
	public int damage = 1;

	public float maxTurnRate = 60;
	float turnRate = 0; 

	public void Init (Crab targetCrab)
	{
		renderer = transform.GetChild(0).GetComponent<Renderer>();
		targetCrabScripts = FindObjectsOfType<Crab>();
		targetCrabTransforms = new Transform[targetCrabScripts.Length];
		for (int i = 0; i < targetCrabScripts.Length; i++) {
			targetCrabTransforms[i] = targetCrabScripts[i].transform;
		}

		StartCoroutine(Move());
	}

	IEnumerator Move ()
	{
		StartCoroutine(CheckForCrab());
		transform.LookAt(Util.Choose(targetCrabTransforms));

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

		Debug.Log("Bye!");
		Destroy(gameObject);
	}

	IEnumerator CheckForCrab ()
	{
		while (true) {
			for (int i=0; i< targetCrabTransforms.Length; i++) {
				if (Util.SquareDistance(transform.position, targetCrabTransforms[i].position) < contactDistance * contactDistance) {
					targetCrabScripts[i].GetAttacked(damage);
				}
			}
			yield return null;
		}
	}
}