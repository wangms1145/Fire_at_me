using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class Bullet1Script : MonoBehaviour
{
    public float impact;
    public Vector2 vel;
    public LayerMask groundLayer;
    public float damage;
    public GameObject bullet_hole;
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
        if(timed > 6){
            Destroy(gameObject);
        }
        vel = myRigidbody.velocity;
        myRigidbody.velocity = vel * (1-Time.deltaTime * 0.6f);
        RaycastHit2D hit = collide_check();
        if(hit){
            Vector2 a = hit.point;

            if(hit.collider.GetComponent<BotScript>() != null){
                BotScript aim = hit.collider.GetComponent<BotScript>();
                aim.health -= myRigidbody.velocity.magnitude/1 * damage;
            }
            if(hit.rigidbody != null){
                Vector2 diff = hit.point - (Vector2)transform.position;
                hit.rigidbody.velocity += angToSpd(impact * vel.magnitude / 100, spdToAng(diff));
            }
            Instantiate(bullet_hole,a,quaternion.RotateZ(0));
            Destroy(gameObject);
        }
    }
    private RaycastHit2D collide_check(){
        //Debug.DrawRay(transform.position,vel.normalized*Time.deltaTime*2.3f);
        return Physics2D.Raycast(transform.position,vel.normalized,vel.magnitude*Time.deltaTime*2.3f,groundLayer);
    }
    private void OnDrawGizmos(){
        //Gizmos.DrawWireCube(transform.position + vel/myRigidbody.velocity.magnitude * (boxSize.x/2-0.1f),boxSize);
        Gizmos.DrawRay(transform.position,vel*Time.deltaTime*2.3f);
    }
    private Vector2 angToSpd(float strength,float ang){
        Vector2 a;
        a = Vector2.right * (float)Math.Cos(ang) * strength;
        a += Vector2.up * (float)Math.Sin(ang) * strength;
        return a;
    }
    private float spdToAng(Vector2 spd){
        double a;
        a = Math.Atan2(spd.y,spd.x);
        return (float)a;
    }
}
