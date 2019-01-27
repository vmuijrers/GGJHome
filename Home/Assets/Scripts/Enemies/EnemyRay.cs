using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRay : Enemy, IEnemy
{

	Transform targetCrabTrans;
	Crab targetCrabScript;

	public float farSpeed = 3, closeSpeed = 8, closeDistance = 7, contactDistance = 1;
	public int damage = 1;

	public void Init (Crab targetCrab)
	{
		renderer = transform.GetComponentInChildren<Renderer>();
		targetCrabScript = targetCrab;
		targetCrabTrans = targetCrab.transform;
		StartCoroutine(Move());
	}

	IEnumerator Move ()
	{
		//GO IN FOR THE CRAB
		bool closeToTarget = false;
		float speed = farSpeed;
		while (!closeToTarget) {
			transform.LookAt(targetCrabTrans);
			transform.position += transform.forward * farSpeed * Time.deltaTime;

			float distanceToCrab = Util.SquareDistance(transform.position, targetCrabTrans.position);
			if (distanceToCrab < closeDistance * closeDistance) {
				speed = closeSpeed;
				closeToTarget = true;
				break;
			}
			yield return null;
		}

		//CHARGE DAT CRAB
		StartCoroutine(CheckForCrab());
		bool inScope = true;
		while (inScope) {
			transform.position += transform.forward * speed * Time.deltaTime;

			if (!renderer.isVisible)
				inScope = false;
			yield return null;
		}

        if (!renderer.isVisible) {
            Die();
        }
    }

	IEnumerator CheckForCrab ()
	{
		while (true) {
			if (Util.SquareDistance(transform.position, targetCrabTrans.position) < contactDistance * contactDistance) {
				targetCrabScript.GetAttacked(damage);
			}
			yield return null;
		}
	}


}