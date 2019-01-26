using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
	public float timeBetweenSpawns = 1;
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
		EnemyEel nextEel = Instantiate(eelPrefab, new Vector3(15, 0, 15), Quaternion.identity).GetComponent<EnemyEel>();
		activeEels.Add(nextEel);
		nextEel.Init(Util.Choose(crabs));
		yield return StartCoroutine(Spawner());
	}
}