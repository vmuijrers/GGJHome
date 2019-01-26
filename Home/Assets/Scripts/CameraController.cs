using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using DG.Tweening;
public class CameraController : MonoBehaviour
{
    public float smoothSpeed = 4f;
    public int crabWeight = 2;

    private Transform targetToFollow;
    private float zoomDistance;
    private float zoomIncrease = 1;
    private float heightPosition = 7;
    private float viewAngle = 45;
    private List<Transform> transformsToFollow = new List<Transform>();
    private Crab[] crabs;
    private Shell[] shells;

    // Start is called before the first frame update
    void Start()
    {
        crabs = FindObjectsOfType<Crab>();
        foreach(Crab c in crabs) {
            transformsToFollow.Add(c.transform);
        }
        
        shells = FindObjectsOfType<Shell>();
    }

    private void OnEnable() {
        Message.AddListener(MessageEnum.ON_GRAB_SHELL, gameObject, Shake);
    }

    private void OnDisable() {
        Message.RemoveListener(MessageEnum.ON_GRAB_SHELL, gameObject, Shake);
    }

    private void Update() {
        int num = 0;
        Vector3 averagePosition = Vector3.zero;
        foreach(Crab crab in crabs) {
            for(int i=0;i < crabWeight; i++) {
                averagePosition += crab.transform.position;
                num++;
            }
        }
        foreach(Shell shell in shells) {
            if (shell.renderer.isVisible) {
                averagePosition += shell.transform.position;
                num++;
            }
        }
        averagePosition /= num;
        averagePosition -= new Vector3(0, 0, Mathf.Tan(viewAngle * Mathf.Deg2Rad) * heightPosition);
        transform.position = Vector3.Lerp(transform.position, averagePosition, smoothSpeed * Time.deltaTime);
        transform.position = new Vector3(transform.position.x, heightPosition, transform.position.z);
    }

    private void Shake() {
        Debug.Log("Shake!");
        CameraShake.OnShake(0.3f, 0.5f, 1f);

    }
}
