using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VegetationSpawner : MonoBehaviour
{
	public GameObject[] vegetationPrefabs;

	Vector3 lastSpawnPos = Vector3.zero, camPos;
	public float maxSpawnZPos = 13, spawnXOffset = 17.5f, distanceBetweenSpawns = 4;

	private void Awake ()
	{
		StartCoroutine(SpawnVegetation());
	}

	IEnumerator SpawnVegetation ()
	{
		while (true) {
			camPos = new Vector3(Camera.main.transform.position.x, 0, Camera.main.transform.position.z);
			if (Util.SquareDistance(camPos, lastSpawnPos) > distanceBetweenSpawns * distanceBetweenSpawns) {
				Instantiate(Util.Choose(vegetationPrefabs), new Vector3(camPos.x + spawnXOffset + Random.Range(0, 3.0f), 0, Random.Range(-maxSpawnZPos, maxSpawnZPos)), Quaternion.Euler(0, Random.Range(0, 360), 0));
				lastSpawnPos = camPos;
			}
			yield return null;
		}
	}
}