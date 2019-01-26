using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTentacle : MonoBehaviour
{
	Crab[] targetCrabScripts;

	public float attackTime = .4f, attackHitRadius = 1.5f, secondsBeforeReset = .7f, resetTime = 1;
	public int damage = 1;

	void Start ()
	{
		targetCrabScripts = FindObjectsOfType<Crab>();

		transform.forward = Vector3.up;
		StartCoroutine(TentacleThings());
	}

	IEnumerator TentacleThings ()
	{
		bool attacking = false;
		while (!attacking) {
			for (int i = 0; i < targetCrabScripts.Length; i++) {
				if (Util.SquareDistance(transform.position, targetCrabScripts[i].transform.position) < 9f) {
					StartCoroutine(Attack(() => { attacking = false; }, targetCrabScripts[i].transform.position));
					attacking = true;
				}
			}
			yield return null;
		}

		while (attacking) {
			yield return true;
		}

		yield return StartCoroutine(TentacleThings());
	}

	IEnumerator Attack (System.Action Callback, Vector3 target)
	{
		for (float t = 0; t < attackTime; t += Time.deltaTime) {
			transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(target - transform.position, Vector3.up), 90 / attackTime * Time.deltaTime);
			yield return null;
		}
		for (int i = 0; i < targetCrabScripts.Length; i++) {
			if (Util.SquareDistance(target, targetCrabScripts[i].transform.position) < attackHitRadius * attackHitRadius) {
				targetCrabScripts[i].GetAttacked(damage);
			}
		}
		yield return new WaitForSeconds(secondsBeforeReset);

		float angleToReset = Vector3.Angle(Vector3.up, transform.forward);
		for (float t = 0; t < resetTime; t += Time.deltaTime) {
			transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(Vector3.up, Vector3.back), angleToReset / resetTime * Time.deltaTime);
			yield return null;
		}

		transform.forward = Vector3.up;
		Callback();
	}
}