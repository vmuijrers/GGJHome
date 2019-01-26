using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEnemy
{
	void Init (Crab crab);
}

public class EnemyManager : MonoBehaviour
{
	public float timeBetweenSpawns = 1;
	public GameObject[] enemyPrefabs;
	List<IEnemy> activeEels = new List<IEnemy>();
	Crab[] crabs;

	void Start ()
	{
		crabs = FindObjectsOfType<Crab>();
		StartCoroutine(Spawner());
	}

	IEnumerator Spawner ()
	{
		yield return new WaitForSeconds(timeBetweenSpawns);
		IEnemy nextEel = Instantiate(Util.Choose(enemyPrefabs), new Vector3(15, 0, 15), Quaternion.identity).GetComponent<EnemyEel>();
		activeEels.Add(nextEel);
		nextEel.Init(Util.Choose(crabs));
		yield return StartCoroutine(Spawner());
	}
}