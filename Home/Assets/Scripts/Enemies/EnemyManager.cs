using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEnemy
{
	void Init (Crab crab);
    void OnHitByLight(Vector3 lightPos);
    void ListenToDead(System.Action<IEnemy> OnDead);
    Vector3 GetPosition();
}

public class EnemyManager : MonoBehaviour {
    public static EnemyManager instance;
	Transform camTrans;
	public float timeBetweenSpawns = 1;
	public GameObject[] enemyPrefabs;
	public GameObject tentaclePrefab;
    public float lightSpotDistance = 8;
	Crab[] crabs;
    private List<IEnemy> allEnemies = new List<IEnemy>();
	Vector3 camPos, lastSpawnPos = Vector3.zero;
	public float distanceBetweenTentacleSpawns = 8;
	float maxSpawnZPos = 13, spawnXOffset = 17.5f;

	void Start ()
	{
        instance = this;
		camTrans = Camera.main.transform;
        crabs = FindObjectsOfType<Crab>();
		Debug.Log(Util.Choose(crabs).gameObject.name);
		StartCoroutine(EnemySpawner());
		StartCoroutine(TentacleSpawner());
	}

    public void CheckEnemyhitByLight(Vector3 dir, Vector3 pos) {
        foreach (IEnemy e in allEnemies) {
            if(e != null) {
                float dot = Vector3.Dot(dir, (e.GetPosition() - pos).normalized);
                Debug.Log("Dot: " + dot);
                if (dot > 0.5f && Vector3.Distance(pos, e.GetPosition()) < lightSpotDistance) {
                    e.OnHitByLight(pos);
                }
            }
        }

    }

    IEnumerator EnemySpawner ()
	{
		yield return new WaitForSeconds(timeBetweenSpawns);
        Vector3 spawnPos = new Vector3(Camera.main.transform.position.x, 0, Camera.main.transform.position.z);
        Vector2 dir = Random.insideUnitCircle.normalized;
        spawnPos += new Vector3(dir.x, 0, dir.y) * 20;
        IEnemy nextEnemy = Instantiate(Util.Choose(enemyPrefabs), spawnPos, Quaternion.identity).GetComponent<IEnemy>();
		nextEnemy.Init(Util.Choose(crabs));
        nextEnemy.ListenToDead(OnEnemyDied);
        allEnemies.Add(nextEnemy);
        yield return StartCoroutine(EnemySpawner());
	}

	IEnumerator TentacleSpawner ()
	{
		bool spawned = false;
		while (true) {
			spawned = false;
			camPos = new Vector3(camTrans.position.x, 0, Camera.main.transform.position.z);
			if (Util.SquareDistance(camPos, lastSpawnPos) > distanceBetweenTentacleSpawns * distanceBetweenTentacleSpawns && !spawned) {
				spawned = true;
				Instantiate(Util.Choose(tentaclePrefab), new Vector3(camPos.x + spawnXOffset + Random.Range(0, 1.0f), 0, Random.Range(-maxSpawnZPos, maxSpawnZPos)), Quaternion.identity);
				lastSpawnPos = camPos;
			}
			yield return null;
		}
	}

	void OnEnemyDied(IEnemy e) {
        if (allEnemies.Contains(e)) {
            allEnemies.Remove(e);
        }
    }
}