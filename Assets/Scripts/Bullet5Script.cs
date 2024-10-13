using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class Bullet5Script : MonoBehaviour
{
    public Vector2 vel;
    public float radius;
    public LayerMask groundLayer;
    public GameObject explode;
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
        timed += Time.deltaTime;
        if(timed > 30){
            Destroy(gameObject);
        }
        vel = myRigidbody.velocity;
        RaycastHit2D hit = collide_check();
        if(hit){
            Vector2 a = hit.point;
            Instantiate(explode,a,quaternion.RotateZ(0));
            Destroy(gameObject);
        }
    }
    private RaycastHit2D collide_check(){
        RaycastHit2D hit1 =  Physics2D.Raycast(transform.position,vel,vel.magnitude*Time.deltaTime*5,groundLayer);
        RaycastHit2D hit2 =  Physics2D.CircleCast(transform.position,radius,Vector2.right,0,groundLayer);
        if(hit2){
            return hit2;
        }
        else{
            return hit1;
        }
    }
    private void OnDrawGizmos(){
        //Gizmos.DrawWireCube(transform.position + vel/myRigidbody.velocity.magnitude * (boxSize.x/2-0.1f),boxSize);
        Gizmos.DrawRay(transform.position,vel*Time.deltaTime*5);
        Gizmos.DrawWireSphere(transform.position,radius);
    }
}
