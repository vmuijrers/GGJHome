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
        Vector3 spawnPos = new Vector3(Camera.main.transform.position.x, 0, Camera.main.transform.position.z);
        Vector2 dir = Random.insideUnitCircle.normalized;
        spawnPos += new Vector3(dir.x, 0, dir.y) * 15;
        IEnemy nextEnemy = Instantiate(Util.Choose(enemyPrefabs), spawnPos, Quaternion.identity).GetComponent<IEnemy>();
		//Debug.Log(nextEnemy.GetType());
		nextEnemy.Init(Util.Choose(crabs));
		yield return StartCoroutine(Spawner());
	}
}