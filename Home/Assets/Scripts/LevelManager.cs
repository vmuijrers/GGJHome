using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    private Pickup pickupPrefab;
    private int amountOfPickups = 10;
    private Vector3 levelBounds = new Vector3(100, 1, 6);

	bool gameRunning = false;

    void Start()
    {
        pickupPrefab = Resources.Load<Pickup>("Pickups/Pickup");
        SpawnLevel();
    }

	void SpawnLevel() {
        for(int i=0; i < amountOfPickups; i++) {
            Pickup pickup  = Instantiate(pickupPrefab, new Vector3(Random.Range(0, levelBounds.x), Random.Range(1, 30), Random.Range(-levelBounds.z / 2, levelBounds.z / 2)), Quaternion.Euler(0,Random.Range(0,360) ,0));
            pickup.Init(Util.Choose(DecorationType.Basic, DecorationType.Star));
        }
    }
}