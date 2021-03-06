﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour {

    public static System.Action<float, float, float> OnShake;
    Vector3 originalPos;
    private float strength;
    private float decay;
    private float duration;

    public float strengthVal = 0.1f;
    public float durationVal = 0.3f;
    public float decayVal = 0.01f;
    // Use this for initialization
    void Start () {
        originalPos = transform.localPosition;
        OnShake += DoShake;
    }

    private void OnDestroy() {
        OnShake -= DoShake;
    }
    private void DoShake(float strength, float duration, float decay)
    {
        StopAllCoroutines();
        StartCoroutine(Shake(strength, duration, decay));
    }

    IEnumerator Shake(float strength, float duration, float decay)
    {
        float t = 0;
        float step = 1 / duration;
        this.strength = strength;
        this.duration = duration;
        this.decay = decay;
        originalPos = transform.localPosition;
        while (t < 1)
        {
            transform.localPosition = originalPos + new Vector3(Random.Range(-this.strength, this.strength), Random.Range(0, this.strength), 0);
            t += step * Time.deltaTime;
            this.strength -= this.decay * Time.deltaTime;
            this.strength = Mathf.Clamp(this.strength, 0, this.strength);
            yield return null;
            transform.localPosition = originalPos;
        }

        transform.localPosition = originalPos;
    }
}
