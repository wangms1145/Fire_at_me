using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet1Script : MonoBehaviour
{
    private float timed = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timed += Time.deltaTime;
        if(timed > 6){
            Destroy(gameObject);
        }
    }
}
