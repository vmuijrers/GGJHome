using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTentacle : MonoBehaviour
{
	Transform[] targetCrabTransforms;
	Crab[] targetCrabScripts;

	public

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
		while (true) {
			for (int i = 0; i < targetCrabScripts.Length; i++) {
				if (Util.SquareDistance(transform.position, targetCrabTransforms[i].position) < 4) {
					StartCoroutine(Attack(targetCrabTransforms[i].position));
				}
			}
		}
	}

	IEnumerator Attack (Vector3 target)
	{
		yield return null;
	}
}