using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyEel : MonoBehaviour
{
	Transform targetCrabTrans;
	Crab targetCrabScript;

	const float farSpeed = 6, closeSpeed = 10, exitSpeed = 7, closeDistance = 7, contactDistance = 1;
	float speed = farSpeed;
	Vector3 exitPosition = new Vector3(-15, 0, -15);

	public void Init (Crab targetCrab)
	{
		targetCrabScript = targetCrab;
		targetCrabTrans = targetCrab.transform;
		StartCoroutine(Attack());
	}

	IEnumerator Attack ()
	{
		//GO IN FOR THE CRAB
		Debug.Log("Going in!");
		bool targetHit = false;
		while (!targetHit) {
			transform.position = Vector3.MoveTowards(transform.position, targetCrabTrans.position, speed * Time.deltaTime);

			float distanceToCrab = Util.SquareDistance(transform.position, targetCrabTrans.position);
			if (distanceToCrab < closeDistance * closeDistance)
				speed = closeSpeed;
			if (distanceToCrab < contactDistance * contactDistance) {
				targetCrabScript.GetAttacked(1);
				targetHit = true;
			} else {
				speed = farSpeed;
			}
			yield return null;
		}

		//TARGET HIT, RETREAT
		Debug.Log("Target hit!");
		while (true) {
			transform.position = Vector3.MoveTowards(transform.position, exitPosition, exitSpeed * Time.deltaTime);
			if (Util.SquareDistance(transform.position, exitPosition) < .5f) {
				break;
			}
			yield return null;
		}

		Debug.Log("Bye!");
		Destroy(gameObject);
	}
}