using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupSlot : MonoBehaviour
{
    [HideInInspector] public GameObject reference;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void SetReference(GameObject obj) {
        reference = obj;
    }
}
