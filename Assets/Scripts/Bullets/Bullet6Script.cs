using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using Unity.Netcode;

public class Bullet6Script : NetworkBehaviour
{
    public float impact;
    public Vector2 vel;
    public LayerMask groundLayer;
    public float damage;
    public GameObject bullet_hole;
    private float timed = 0;
    private Rigidbody2D myRigidbody;
    private RaycastHit2D last_hit;
    private NetworkObject net;
    // Start is called before the first frame update
    void Start()
    {
        net = GetComponent<NetworkObject>();
        myRigidbody = gameObject.GetComponent<Rigidbody2D>();
        vel = myRigidbody.velocity;
        
        RaycastHit2D hit = collide_check();
        if(hit){
            Vector2 a = hit.point;

            if(hit.collider.GetComponent<BotScript>() != null){
                BotScript aim = hit.collider.GetComponent<BotScript>();
                aim.health -= damage;
            }
            if(hit.collider.GetComponent<playerLogic>() != null){
                playerLogic aim = hit.collider.GetComponent<playerLogic>();
                aim.damage(damage);
            }
            if(hit.collider.GetComponent<HealthForObject>() != null){
                HealthForObject aim = hit.collider.GetComponent<HealthForObject>();
                aim.damageRPC(damage);
            }
            Instantiate(bullet_hole,a,quaternion.RotateZ(0));
            //Destroy(gameObject);
        }
        //Debug.Break();
    }

    // Update is called once per frame
    void Update()
    {
        if(!IsOwner)return;
        timed += Time.deltaTime;
        if(timed > 6){
            net.Despawn();
            Destroy(gameObject);
        }
        vel = myRigidbody.velocity;
        myRigidbody.velocity = vel * (1-Time.deltaTime * 0.6f);
        RaycastHit2D hit = collide_check();
        if(hit){
            Vector2 a = hit.point;

            if(hit.collider.GetComponent<BotScript>() != null && !hit.collider.Equals(last_hit.collider)){
                BotScript aim = hit.collider.GetComponent<BotScript>();
                aim.health -= damage;
            }
            if(hit.collider.GetComponent<playerLogic>() != null){
                playerLogic aim = hit.collider.GetComponent<playerLogic>();
                aim.damage(damage);
            }
            Instantiate(bullet_hole,a,quaternion.RotateZ(0));
            last_hit = hit;
            //Destroy(gameObject);
        }
    }
    private RaycastHit2D collide_check(){
        //Debug.DrawRay(transform.position,vel.normalized*Time.deltaTime*2.3f);
        return Physics2D.Raycast(transform.position,vel.normalized,vel.magnitude*0.03333f,groundLayer);
    }
    private void OnDrawGizmos(){
        //Gizmos.DrawWireCube(transform.position + vel/myRigidbody.velocity.magnitude * (boxSize.x/2-0.1f),boxSize);
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
