using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class SpotLight : MonoBehaviour
{
    private Light light;
    private bool isActive = false;
    // Start is called before the first frame update
    void Start()
    {
        light = GetComponent<Light>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isActive) {
            if(EnemyManager.instance != null) {
                EnemyManager.instance.CheckEnemyhitByLight(transform.forward, transform.position);
            }
            
        }    
    }
    public void ActivateLight() {
        Tween lightTween = light.DOIntensity(5.5f, 1f);
        lightTween.Play();
        isActive = true;
    }
    public void DeActivateLight() {
        Tween lightTween = light.DOIntensity(0.2f, 3f);
        lightTween.Play();
        isActive = false;
    }
}
