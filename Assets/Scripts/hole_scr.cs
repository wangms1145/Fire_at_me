using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hole_scr : MonoBehaviour
{
    private double time = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        if(time > 0.5){
            Destroy(gameObject);
        }
    }
}
