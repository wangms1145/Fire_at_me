using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudSpawner : MonoBehaviour
{
    [SerializeField] private GameObject cloud;
    [SerializeField] private GameObject parent;
    [SerializeField] private float x_min;
    [SerializeField] private float x_max;
    [SerializeField] private float y_max;
    [SerializeField] private float y_min;
    [SerializeField] private float z_min;
    [SerializeField] private float z_max;
    [SerializeField] private float spn_time;
    private float timer = 0;
    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(x_min,y_min, z_min);
        for(int i = 0; i<70;i++){
            float x = Distributed_random.range(x_min,x_max,4);
            float y = Distributed_random.range(y_min,y_max,4);
            float z = Distributed_random.range(y_min,y_max,4);
            transform.position = new Vector3(x,y,z);
            GameObject ins = Instantiate(cloud,transform.position,transform.rotation);
            ins.GetComponent<CloudScript>().left_bound = x_min;
            ins.transform.SetParent(parent.transform);
        }
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if(timer > spn_time){
            float y = Distributed_random.range(y_min,y_max,4);
            float z = Distributed_random.range(y_min,y_max,4);
            transform.position = new Vector3(x_max,y,z);
            GameObject ins = Instantiate(cloud,transform.position,transform.rotation);
            ins.GetComponent<CloudScript>().left_bound = x_min;
            ins.transform.SetParent(parent.transform);
            timer = 0;
        }
    }

}
