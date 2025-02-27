using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using Unity.Netcode;

public class Bullet3Script : NetworkBehaviour
{
    public float trig,des,damage;
    private float timed = 0;
    public Rigidbody2D myRigidbody;
    private NetworkObject net;
    public GameObject bullet_hole;
    // Start is called before the first frame update
    void Start()
    {
        des = CalcTime(trig);
        net = GetComponent<NetworkObject>();
    }
    void OnCollisionEnter2D(Collision2D other){
        Vector2 a = other.GetContact(0).point;
        Debug.Log(other.collider.gameObject.name);
        if(other.collider.GetComponent<BotScript>() != null){
            BotScript aim = other.collider.GetComponent<BotScript>();
            aim.health -= damage;
        }
        if(other.collider.GetComponent<playerLogic>() != null){
            playerLogic aim = other.collider.GetComponent<playerLogic>();
            aim.damage(damage);
        }
        Instantiate(bullet_hole,a,quaternion.RotateZ(0));
    }

    // Update is called once per frame
    void Update()
    {
        if(!IsOwner)return;
        timed += Time.deltaTime;
        if(timed > trig){
            
            if(trig/2 >= 0.01){
                GameObject sec_bullet = Instantiate(gameObject,transform.position,transform.rotation);
                sec_bullet.GetComponent<Bullet3Script>().trig = trig/2;
                sec_bullet.GetComponent<Rigidbody2D>().velocity = conAng(myRigidbody.velocity.magnitude,SpdToAng(myRigidbody.velocity) + (float)Math.PI/8);
                sec_bullet.GetComponent<Bullet3Script>().damage = damage/2;
                sec_bullet.GetComponent<NetworkObject>().Spawn();
                sec_bullet = Instantiate(gameObject,transform.position,transform.rotation);
                sec_bullet.GetComponent<Bullet3Script>().trig = trig/2;
                sec_bullet.GetComponent<Rigidbody2D>().velocity = conAng(myRigidbody.velocity.magnitude,SpdToAng(myRigidbody.velocity) - (float)Math.PI/8);
                sec_bullet.GetComponent<Bullet3Script>().damage = damage/2;
                sec_bullet.GetComponent<NetworkObject>().Spawn();
                trig = 0;
            }
            myRigidbody.velocity = Vector2.zero;
            gameObject.GetComponent<SpriteRenderer>().enabled = false;
            gameObject.GetComponent<CircleCollider2D>().enabled = false;
            if(timed > des*1.5){
                gameObject.GetComponent<TrailRenderer>().enabled = false;
                net.Despawn();
                Destroy(gameObject);
            }
        }
    }
    Vector2 conAng(float amount,float ang){
        return Vector2.right * (float)(Math.Cos(ang)*amount) + Vector2.up * (float)(Math.Sin(ang)*amount);
    }
    float SpdToAng(Vector2 a){
        double ang;
        ang = Math.Atan(a.y/a.x);
        if(a.x<0){
            ang += Math.PI;
        }
        return (float)ang;
    }
    float CalcTime(float time){
        if(time < 0.01){
            return time;
        }
        else{
            return time + CalcTime(time/2);
        }
    }
}
