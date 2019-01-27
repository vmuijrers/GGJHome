using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XInputDotNetPure;
public class LevelManager : MonoBehaviour
{
    private Pickup pickupPrefab;
    private int amountOfPickups = 10;
    private Vector3 levelBounds = new Vector3(100, 1, 6);
    // Start is called before the first frame update
    void Start()
    {
        pickupPrefab = Resources.Load<Pickup>("Pickups/Pickup");
        SpawnLevel();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void SpawnLevel() {
        for(int i=0; i < amountOfPickups; i++) {
            Pickup pickup  = Instantiate(pickupPrefab, new Vector3(Random.Range(0, levelBounds.x), Random.Range(1, 30), Random.Range(-levelBounds.z / 2, levelBounds.z / 2)), Quaternion.Euler(0,Random.Range(0,360) ,0));
            pickup.Init(Util.GetRandomEnumerator<DecorationType>());

        }

    }
}
