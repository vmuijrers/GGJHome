using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
	public float timeBetweenSpawns = 4;
	public GameObject eelPrefab;
	List<EnemyEel> activeEels = new List<EnemyEel>();
	Crab[] crabs;

	void Start ()
	{
		crabs = FindObjectsOfType<Crab>();
		StartCoroutine(Spawner());
	}

	IEnumerator Spawner ()
	{
		yield return new WaitForSeconds(timeBetweenSpawns);
		activeEels.Add(Instantiate(eelPrefab, new Vector3(10, 0, 10), Quaternion.identity).GetComponent<EnemyEel>());
		activeEels[activeEels.Count - 1].Init(Util.Choose(crabs));
		yield return StartCoroutine(Spawner());
	}
}