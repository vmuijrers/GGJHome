using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReviveSelf : MonoBehaviour {
    public float reviveTime = 5;
    private Crab crab;
    private float timer = 0;
    // Start is called before the first frame update
    void Start() {
        crab = GetComponent<Crab>();
    }

    // Update is called once per frame
    void Update() {
        if (crab.dood) {
            timer += Time.deltaTime;
            if (timer > reviveTime) {
                crab.OnRevive();
            }
        } else {
            timer = 0;
        }
    }
}

