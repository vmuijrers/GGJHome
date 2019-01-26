using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XInputDotNetPure;
using System.Linq;

public class Shell : MonoBehaviour
{
    public ShellEntrance[] entranceList;

    private Rigidbody rigidBody;
    private Vector2 direction;
    private float rotationSpeed = 30f;
    public int size = 1;

    public float speedModifier = 10;
    private void Awake() {
        rigidBody = GetComponent<Rigidbody>();
        entranceList = GetComponentsInChildren<ShellEntrance>();
        for (int i = 0; i < entranceList.Length; i++) {
            entranceList[i].Init(this);
        }
        transform.localScale = new Vector3(size, size, size);
    }
    private void Update() {

        int crabCount = entranceList.Count(x => x.attachedCrab != null);
        if (crabCount > 0) {
            rigidBody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
            Vector2 dir = Vector2.zero;
            foreach(ShellEntrance c in entranceList) {
                if(c.attachedCrab != null) {
                    dir += c.attachedCrab.GetLeftStickInput();
                }
            }
            dir /= crabCount;
            DoMove(dir, crabCount * speedModifier);

        } else {
            rigidBody.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ;
        }
    }

    public void DoMove(Vector2 dir, float speedModifier) {
        rigidBody.velocity = new Vector3(dir.x, 0, dir.y) * speedModifier;
        if(dir.magnitude != 0) {
            rigidBody.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(rigidBody.velocity, Vector3.up), rotationSpeed * Time.deltaTime);
        }
            
    }

}