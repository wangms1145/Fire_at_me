using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;
using System;
using System.Text.RegularExpressions;

public class BotScript : MonoBehaviour
{
    public Rigidbody2D myRigidbody;

    //player max speed
    public float spd;

    //player move acceleration
    public float acc;

    //rotation lock acceleration
    public float aacc;

    //
    public float jumpStrength;
    
    //ray casting box size
    public Vector2 boxSize;
    //ray casting distance
    public float castDistance;
    //ray casting layermark
    public LayerMask groundLayer;
    public PhysicsMaterial2D inGame_material;
    public PhysicsMaterial2D died_material;
    public bool isAlive = true;
    public float diedYpos;
    public AudioSource audSource;
    public AudioClip fall;
    public AudioClip died;
    public AudioClip zhuiji;
    public AudioDistortionFilter filter;
    public float health = 100;
    //public int mag,c_mag;

   

    private float health_max;
    private float spdx,spdy;
    private float tspd;
    private bool groundFlag;
    private float ys = 0;
    private int i;
    // Start is called before the first frame update
    void Start()
    {
        myRigidbody.sharedMaterial = inGame_material;
        health_max = health;
    }

    // Update is called once per frame
    void Update()
    {
        if (isAlive) {
            myRigidbody.sharedMaterial = inGame_material;
            spdx = myRigidbody.velocity.x;
            spdy = myRigidbody.velocity.y;

            //jump
            if(Input.GetKeyDown(KeyCode.UpArrow) && isGrounded()){
                myRigidbody.velocity += Vector2.up * jumpStrength;
            }


            //move
            tspd = 0;
            if(Input.GetKey(KeyCode.LeftArrow))tspd += spd;
            if(Input.GetKey(KeyCode.RightArrow))tspd -= spd;
            float acc_add = 0;
            if(Input.GetKey(KeyCode.RightControl) && isGrounded()){acc_add = (float)(0.7 - acc); tspd = 0;}
            myRigidbody.velocity += Vector2.left * (tspd+spdx)*Math.Clamp((acc+acc_add)*Time.deltaTime*100,-1,1);

            //rotation lock
            myRigidbody.angularVelocity = (0-myRigidbody.rotation)*Math.Clamp(aacc*Time.deltaTime*100,-0.7f,0.7f)/Time.deltaTime;

            if(transform.position.y < diedYpos || Input.GetKeyDown(KeyCode.Y) || health < 0){
                isAlive = false;
                float ang = UnityEngine.Random.Range(-180, 180);
                myRigidbody.velocity += Vector2.up * (float)Math.Sin(ang)*5 + Vector2.right * (float)Math.Cos(ang)*5;
                myRigidbody.angularVelocity = (float)(UnityEngine.Random.Range(-15, 15)/3.0);
                myRigidbody.sharedMaterial = died_material;
                if(transform.position.y < diedYpos){
                    filter.distortionLevel = 0;
                    audSource.PlayOneShot(zhuiji);
                }
                else{
                    filter.distortionLevel = 0;
                    audSource.PlayOneShot(died);
                }
            }
            if(isGrounded() && !groundFlag){
                filter.distortionLevel = Mathf.InverseLerp(-10,-30,ys);
                audSource.PlayOneShot(fall);
            }
            groundFlag = isGrounded();
            if(i >= 5){ys = myRigidbody.velocity.y;i = 0;}
            i++;
        }
        else{
            if(transform.position.y < diedYpos-30){
                myRigidbody.simulated = false;
            }
            if(Input.GetKey(KeyCode.RightShift)){
                myRigidbody.simulated = true;
                myRigidbody.velocity = Vector2.zero;
                myRigidbody.position = Vector2.zero;
                myRigidbody.rotation = 0;
                health = health_max;
                isAlive = true;
            }
        }
    }

    //ground detect using box casting
    public bool isGrounded(){
        if(Physics2D.BoxCast(transform.position,boxSize,0,Vector2.down,castDistance,groundLayer)){
            return true;
        }
        else{
            return false;
        }
    }
    private void OnDrawGizmos(){
        Gizmos.DrawWireCube(transform.position - Vector3.up * castDistance,boxSize);
    }
}
