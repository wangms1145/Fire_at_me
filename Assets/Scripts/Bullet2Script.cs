using System;
using System.Collections;
using System.Collections.Generic;
using System.Transactions;
using Unity.Mathematics;
using UnityEngine;

public class Bullet2Script : MonoBehaviour
{
    private float timed = 0;
    public float impact;
    public Vector2 vel;
    public float damage;
    public LayerMask groundLayer;
    public GameObject bullet_hole;
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
        myRigidbody.velocity = vel * (1-Time.deltaTime * 0.7f);
        RaycastHit2D hit = collide_check();
        if(hit){
            Vector2 a = hit.point;
            if(hit.collider.GetComponent<BotScript>() != null){
                BotScript aim = hit.collider.GetComponent<BotScript>();
                aim.health -= myRigidbody.velocity.magnitude/0.5f*damage;
            }
            if(hit.rigidbody != null){
                Vector2 diff = hit.point - (Vector2)transform.position;
                hit.rigidbody.velocity += angToSpd(impact * vel.magnitude / 400, spdToAng(diff));
            }
            Instantiate(bullet_hole,a,quaternion.RotateZ(0));
            Destroy(gameObject);
        }
    }
    private RaycastHit2D collide_check(){
        return Physics2D.Raycast(transform.position,vel.normalized,vel.magnitude*0.03333f,groundLayer);
    }
    private void OnDrawGizmos(){
        Gizmos.DrawRay(transform.position,vel*0.03333f);
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
