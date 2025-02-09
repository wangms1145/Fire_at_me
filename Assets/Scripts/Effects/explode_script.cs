using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;

public class explode_script : NetworkBehaviour
{
    public float ani_time;
    public float radius;
    public float impact;
    public float damage;
    public LayerMask groundLayer;
    public GameObject hit_p;
    private bool flag = true;
    private float timed = 0;
    public AudioClip explode_aud;
    public AudioSource source;
    public float aud_time;
    // Start is called before the first frame update
    void Start()
    {
        source.PlayOneShot(explode_aud);
    }

    // Update is called once per frame
    void Update()
    {
        timed += Time.deltaTime;
        if(timed > ani_time){
            GetComponent<SpriteRenderer>().enabled = false;
        }
        if(timed > aud_time){
            Destroy(gameObject);
        }
        if(timed > 0.05 && flag){
            flag = false;
            for(float i=0;i<Math.PI*2;i+=(float)Math.PI/720){
                RaycastHit2D hit = explodeCheck(i);
                if(hit){
                    Instantiate(hit_p, hit.point, quaternion.RotateX(0));
                    if(hit.rigidbody != null){
                        Vector2 diff = hit.point - (Vector2)transform.position;
                        if(diff.magnitude < 0.5){
                            diff = diff.normalized * 0.5f;
                        }
                        hit.rigidbody.velocity += angToSpd(impact/(diff.magnitude/18 + 1.5f)/240, spdToAng(diff));
                    }
                    if(hit.collider.GetComponent<BotScript>() != null){
                        BotScript aim = hit.collider.GetComponent<BotScript>();
                        aim.health -= damage;
                    }
                    if(hit.collider.GetComponent<playerLogic>() != null){
                        playerLogic aim = hit.collider.GetComponent<playerLogic>();
                        aim.damage(damage);
                    }
                }
            }
        }
    }
    void OnDrawGizmos(){
        Gizmos.DrawWireSphere(transform.position, radius);
        /*
        for(float i = 0;i < Math.PI*2;i+=(float)Math.PI/60){
            Gizmos.DrawRay(transform.position,angToSpd(1,i)*radius);
        }
        */
    }
    private RaycastHit2D explodeCheck(float ang){
        return Physics2D.Raycast(transform.position,angToSpd(1,ang),radius,groundLayer);
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
