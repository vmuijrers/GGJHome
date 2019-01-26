using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CliffSpawner : MonoBehaviour
{
    public GameObject cliffPrefab;
    // Start is called before the first frame update
    void Start()
    {
        for(int i=0;i< 20; i++) {
            Instantiate(cliffPrefab, new Vector3(i * 9.6f, 0, -15.6f),Quaternion.Euler(0,180,0));
            Instantiate(cliffPrefab, new Vector3(i * 9.6f, 0, 15.6f),Quaternion.identity);
        }
    }

}
