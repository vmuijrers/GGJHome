using UnityEngine;
using System.Collections;

public class Tail : MonoBehaviour {

    public Transform[] tailParts;
    public int amountTailParts = 5;
    public float moveSpeed = 0;
    public float maxSpeed = 3;
    public float dist = 1;
    public float wobbleSpeed = 3;
    public float amountWobbles = 4;
    private float offSet;
	// Use this for initialization
	void Start () {

        //tailParts = new Transform[amountTailParts];
        //for(int i=0; i < amountTailParts; i++)
        //{
        //    GameObject s1 = GameObject.CreatePrimitive(PrimitiveType.Cube);
        //    s1.transform.position = transform.position - transform.forward * i;
        //    s1.transform.SetParent(transform);
        //    float scale = 1 - (i / (float)amountTailParts*0.5f);
        //    s1.transform.localScale = new Vector3(scale,scale,scale);
        //}

        //int c = 0;
        //foreach(Transform t in transform)
        //{
        //    tailParts[c] = t;
        //    c++;
        //}
        //tailParts = GetComponentsInChildren<Transform>();
        amountTailParts = tailParts.Length;

        Debug.Log(tailParts.Length);
        offSet = amountWobbles*Mathf.PI / amountTailParts;
        
	}
	
	// Update is called once per frame
	void Update () {

        //moveSpeed += 0.1f * Time.deltaTime;
        moveSpeed = Mathf.Lerp(moveSpeed, maxSpeed, Time.deltaTime);

        float distOffsetToParent = 0;
        for (int i = 0; i < tailParts.Length; i++)
        {
            distOffsetToParent += 1;// tailParts[i].localScale.x - 0.3f;
            tailParts[i].transform.localPosition = new Vector3(-distOffsetToParent,0,dist * (moveSpeed / maxSpeed) *  ((i + 1) / (float)amountTailParts) * Mathf.Sin(offSet * i + (wobbleSpeed *moveSpeed * Time.time)) );
            if (i != 0) { 
                tailParts[i].transform.LookAt(tailParts[i - 1].transform.position);
            }else
            {
                tailParts[i].transform.LookAt(transform.position);
            }

        }
        transform.position += transform.forward * moveSpeed * Time.deltaTime;

	}
}
