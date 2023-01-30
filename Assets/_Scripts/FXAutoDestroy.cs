using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FXAutoDestroy : MonoBehaviour
{
    private void Start()
    {
        ParticleSystem PS = GetComponent<ParticleSystem>();
        float t1 = PS.main.duration;
        float t2 = PS.main.startLifetimeMultiplier;
            //Debug.Log(t1+t2);
        Destroy(gameObject, t1+t2);
    }
}
