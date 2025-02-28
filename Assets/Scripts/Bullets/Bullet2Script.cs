using System;
using System.Collections;
using System.Collections.Generic;
using System.Transactions;
using Unity.Mathematics;
using UnityEngine;
using Unity.Netcode;

public class Bullet2Script : NetworkBehaviour
{
    private float timed = 0;
    public float impact;
    public Vector2 vel;
    public float damage;
    public LayerMask groundLayer;
    public GameObject bullet_hole;
    private Rigidbody2D myRigidbody;
    private bool waitDes = false;
    private double timer = 0;
    private RaycastHit2D hit;
    private NetworkObject net;
    // Start is called before the first frame update
    void Start()
    {
        net = GetComponent<NetworkObject>();
        myRigidbody = gameObject.GetComponent<Rigidbody2D>();
        if(!IsOwner)return;
        checkHit();
    }

    // Update is called once per frame
    void Update()
    {
        if(!IsOwner)return;
        if(waitDes){
            transform.position = hit.point;
            timer += Time.deltaTime;
            if(timer > GetComponent<TrailRenderer>().time){
                net.Despawn();
                Destroy(gameObject);
            }
            return;
        }
        timed += Time.deltaTime;
        if(timed > 6){
            net.Despawn();
            Destroy(gameObject);
        }
        myRigidbody.velocity = vel * (1-Time.deltaTime * 0.7f);
        checkHit();
    }
    private void checkHit(){
        vel = myRigidbody.velocity;
        
        hit = collide_check();
        if(hit){
            Vector2 a = hit.point;
            if(hit.collider.GetComponent<BotScript>() != null){
                BotScript aim = hit.collider.GetComponent<BotScript>();
                aim.health -= myRigidbody.velocity.magnitude/0.5f*damage;
            }
            if(hit.collider.GetComponent<playerLogic>() != null){
                playerLogic aim = hit.collider.GetComponent<playerLogic>();
                aim.damage(myRigidbody.velocity.magnitude/0.5f * damage);
            }
            if(hit.collider.GetComponent<HealthForObject>() != null){
                HealthForObject aim = hit.collider.GetComponent<HealthForObject>();
                aim.damageRPC(myRigidbody.velocity.magnitude/1 * damage);
            }
            if(hit.rigidbody != null){
                Vector2 diff = hit.point - (Vector2)transform.position;
                hit.rigidbody.velocity += angToSpd(impact * vel.magnitude / 400, spdToAng(diff));
            }
            Instantiate(bullet_hole,a,quaternion.RotateZ(0));
            myRigidbody.simulated = false;
            GetComponent<SpriteRenderer>().enabled = false;
            waitDes = true;
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
