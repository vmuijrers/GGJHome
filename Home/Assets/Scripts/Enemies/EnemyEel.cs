using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyEel : Enemy, IEnemy
{
	Transform targetCrabTrans;
	Crab targetCrabScript;
    public AudioClip eelAttack;
	public float farSpeed = 6, closeSpeed = 5, exitSpeed = 7, closeDistance = 7, contactDistance = 1;
	public int damage = 1;
	Vector3 exitPosition = new Vector3(-15, 0, -15);

	public override void Init (Crab targetCrab)
	{
		targetCrabScript = targetCrab;
		targetCrabTrans = targetCrab.transform;
		StartCoroutine(Attack());
	}

	IEnumerator Attack ()
	{
		//GO IN FOR THE CRAB
		bool targetHit = false;
		float speed = farSpeed;
		while (!targetHit) {
			transform.LookAt(targetCrabTrans);
			transform.position = Vector3.MoveTowards(transform.position, targetCrabTrans.position, speed * Time.deltaTime) + transform.right * Mathf.Sin(Time.time * 20) * .1f;

			float distanceToCrab = Util.SquareDistance(transform.position, targetCrabTrans.position);
			if (distanceToCrab < closeDistance * closeDistance)
				speed = closeSpeed;
			if (distanceToCrab < contactDistance * contactDistance) {
				targetCrabScript.GetAttacked(damage);
				targetHit = true;
                AudioSource.PlayClipAtPoint(eelAttack, transform.position);
			} else {
				speed = farSpeed;
			}
			yield return null;
		}

		//TARGET HIT, RETREAT
		transform.LookAt(exitPosition);
		while (true) {
			transform.position = Vector3.MoveTowards(transform.position, exitPosition, exitSpeed * Time.deltaTime) + transform.right * Mathf.Sin(Time.time * 8) * .1f;
			if (Util.SquareDistance(transform.position, exitPosition) < .5f) {
				break;
			}
			yield return null;
		}

        if (!renderer.isVisible) {
            Die();
        }
    }
}