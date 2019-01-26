using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTentacle : MonoBehaviour
{
	Transform[] targetCrabTransforms;
	Crab[] targetCrabScripts;

	public float attackTime = .7f;
	public int damage = 1;

	void Start ()
	{
		targetCrabScripts = FindObjectsOfType<Crab>();
		targetCrabTransforms = new Transform[targetCrabScripts.Length];
		for (int i = 0; i < targetCrabScripts.Length; i++) {
			targetCrabTransforms[i] = targetCrabScripts[i].transform;
		}
		transform.forward = Vector3.up;
	}

	IEnumerator TentacleThings ()
	{
		bool attacking = false;
		while (!attacking) {
			for (int i = 0; i < targetCrabScripts.Length; i++) {
				if (Util.SquareDistance(transform.position, targetCrabTransforms[i].position) < 3.6f) {
					StartCoroutine(Attack(()=> { attacking = false; }, targetCrabTransforms[i].position));
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
		for (int i = 0; i < targetCrabTransforms.Length; i++) {
			if (Util.SquareDistance(target, targetCrabTransforms[i].position) < .5f) {
				targetCrabScripts[i].GetAttacked(damage);
			}
		}
		yield return new WaitForSeconds(.7f);

		for (float t = 0; t < 1.4f; t += Time.deltaTime) {
			transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(Vector3.up, Vector3.back), 90 / 1.4f * Time.deltaTime);
			yield return null;
		}

		Callback();
	}
}