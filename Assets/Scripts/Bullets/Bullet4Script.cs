using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEditor.Timeline;
using UnityEngine;

public class Bullet4Script : MonoBehaviour
{
    private float timed = 0;
    private Rigidbody2D myRigidbody;
    // Start is called before the first frame update
    void Start()
    {
        myRigidbody = gameObject.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if(myRigidbody.velocity.magnitude < 0.07){
            timed += Time.deltaTime;
        }
        else{
            timed = 0;
        }
        if(timed > 10){
            Destroy(gameObject);
        }
        if(transform.position.y < -30){
            Destroy(gameObject);
        }
    }
}
