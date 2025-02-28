using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class treeScript : MonoBehaviour
{
    private HealthForObject health;
    private float timer;
    public float survive_time = 45;
    // Start is called before the first frame update
    void Start()
    {
        health = GetComponent<HealthForObject>();
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if(health.getHealth() <= 0 || timer > survive_time){
            GetComponent<NetworkObject>().Despawn();
            Destroy(gameObject);
        }
    }
}
