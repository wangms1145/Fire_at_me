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
    RaycastHit2D hit;
    private Stack<GameObject> hit_objects = new Stack<GameObject>();
    private Stack<LayerMask> hit_layers = new Stack<LayerMask>();
    // Start is called before the first frame update
    void Start()
    {
        net = GetComponent<NetworkObject>();
        myRigidbody = gameObject.GetComponent<Rigidbody2D>();
        vel = myRigidbody.velocity;
        
        hitCollide();

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
        hitCollide();
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
    private void hitCollide(){
        hit_objects.Clear();
        do{
            hit = collide_check();
            if(hit){
                Vector2 a = hit.point;
                hit_objects.Push(hit.collider.gameObject);
                hit_layers.Push(hit.collider.gameObject.layer);
                hit.collider.gameObject.layer = LayerMask.NameToLayer("temp");
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
        }while(hit);
        while(hit_objects.Count > 0){
            GameObject change = hit_objects.Pop();
            change.layer = hit_layers.Pop();
        }
    }
}
