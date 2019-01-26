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
	Crab[] crabs;

	void Start ()
	{
		crabs = FindObjectsOfType<Crab>();
		Debug.Log(Util.Choose(crabs).gameObject.name);
		StartCoroutine(Spawner());
	}

	IEnumerator Spawner ()
	{
		yield return new WaitForSeconds(timeBetweenSpawns);
		IEnemy nextEnemy = Instantiate(Util.Choose(enemyPrefabs), new Vector3(15, 0, 15), Quaternion.identity).GetComponent<IEnemy>();
		Debug.Log(nextEnemy.GetType());
		nextEnemy.Init(Util.Choose(crabs));
		yield return StartCoroutine(Spawner());
	}
}