using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    private Pickup pickupPrefab;
    private int amountOfPickups = 10;
    private Vector3 levelBounds = new Vector3(50, 10, 20);
    // Start is called before the first frame update
    void Start()
    {
        pickupPrefab = Resources.Load<Pickup>("Pickups/Pickup");
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SpawnLevel() {
        for(int i=0; i < amountOfPickups; i++) {
            //Instantiate(pickupPrefab, new Vector3(Random.Range(-levelBounds.x/2, )))
        }

    }
}
