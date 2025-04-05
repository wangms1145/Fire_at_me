using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.ParticleSystem;

public class time_for_pariticle : MonoBehaviour
{
    private ParticleSystem particleSystem;
    private float timer = 0;
    // Start is called before the first frame update
    void Start()
    {
        particleSystem = GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if(particleSystem != null && timer > particleSystem.duration + particleSystem.startLifetime){
            Destroy(gameObject);
        }
    }
}
