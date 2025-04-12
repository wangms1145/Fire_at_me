using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.Netcode;
using Unity.VisualScripting;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.Networking;

public class Bullet1Script : NetworkBehaviour
{
    public float impact;
    public Vector2 vel;
    public LayerMask groundLayer;
    public float damage;
    public GameObject bullet_hole;
    public GameObject particle;
    private float timed = 0;
    private Rigidbody2D myRigidbody;
    private bool waitDes = false;
    private double timer = 0;
    private RaycastHit2D hit;
    private NetworkObject net;
    // Start is called before the first frame update
    void Start()
    {
        if(!IsOwner) return;
        myRigidbody = gameObject.GetComponent<Rigidbody2D>();
        net = gameObject.GetComponent<NetworkObject>();
        checkHit();
    }

    // Update is called once per frame
    void Update()
    {
        if(!IsOwner) return;
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
        myRigidbody.velocity = vel * (1-Time.deltaTime * 0.6f);
        checkHit();
    }
    private void checkHit(){
        vel = myRigidbody.velocity;
        hit = collide_check();
        if(hit){
            Vector2 a = hit.point;

            if(hit.collider.GetComponent<BotScript>() != null){
                BotScript aim = hit.collider.GetComponent<BotScript>();
                aim.health -= myRigidbody.velocity.magnitude/1 * damage;
            }
            if(hit.collider.GetComponent<playerLogic>() != null){
                playerLogic aim = hit.collider.GetComponent<playerLogic>();
                aim.damage(myRigidbody.velocity.magnitude/1 * damage);
            }
            if(hit.collider.GetComponent<HealthForObject>() != null){
                HealthForObject aim = hit.collider.GetComponent<HealthForObject>();
                aim.damageRPC(myRigidbody.velocity.magnitude/1 * damage);
            }
            if(hit.rigidbody != null){
                Vector2 diff = hit.point - (Vector2)transform.position;
                hit.rigidbody.velocity += angToSpd(impact * vel.magnitude / 100, spdToAng(diff));
            }
            Instantiate(bullet_hole,a,quaternion.RotateZ(0));
            GameObject particle_ins = Instantiate(particle,a,quaternion.RotateZ(0));
            ParticleSystem.MainModule part =  particle_ins.GetComponent<ParticleSystem>().main;
            part.startColor = Color_Identifier.DetectSpriteColor(hit);
            myRigidbody.simulated = false;
            GetComponent<SpriteRenderer>().enabled = false;
            waitDes = true;
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
