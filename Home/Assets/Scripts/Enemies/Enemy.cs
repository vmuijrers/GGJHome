using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, IEnemy
{
    protected float rotationSpeed = 180f;
    protected float runAwaySpeed = 8;
    protected Renderer renderer;
    private System.Action<IEnemy> OnDead;
    private void Awake() {
        renderer = GetComponentInChildren<Renderer>();
    }

    public virtual void OnHitByLight(Vector3 lightPoint) {
        StopAllCoroutines();
        StartCoroutine(MoveAway(lightPoint));
    }
    IEnumerator MoveAway(Vector3 lightPoint) {

        while (true) {
            Vector3 dir = (transform.position - lightPoint);
            dir = Vector3.Scale(dir, new Vector3(1, 0, 1));
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(dir), rotationSpeed * Time.deltaTime);
            transform.position += transform.forward * runAwaySpeed * Time.deltaTime;
            yield return null;
            if (!renderer.isVisible) {
                Die();
            }
        }
        
    }

    public void Die() {
        OnDead(this);
        Destroy(gameObject);
    }

    public virtual void Init(Crab crab) {
        renderer.GetComponent<Renderer>();
    }

    public void ListenToDead(System.Action<IEnemy> action) {
        if(action != null) {
            OnDead += action;
        }
    }

    public Vector3 GetPosition() {
        return transform.position;
    }
}
