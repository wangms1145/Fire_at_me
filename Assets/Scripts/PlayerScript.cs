using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;
using System;
using Unity.VersionControl.Git.ICSharpCode.SharpZipLib;
using System.Text.RegularExpressions;
//using UnityEngine.Random;

public class PlayerScript : MonoBehaviour
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
    public bool DoBunnyHop;

    
    public float recoil;
    public float reloading_time;
    
    //ray casting box size
    public Vector2 boxSize;
    //ray casting distance
    public float castDistance;
    //ray casting layermark
    public LayerMask groundLayer;
    public PhysicsMaterial2D inGame_material;
    public PhysicsMaterial2D died_material;

    //for debug
    public float mouse_mult;
    public bool isAlive = true;
    public float diedYpos;

   
    [SerializeField]
    public Cam_script cam_script;


    private float ang;
    private float spdx,spdy;
    private float tspd;
    private float time_last_shoot = -999;// Initialized to make sure you could shoot when ever you start the game
    public float mouY,mouX;
    public float disY,disX;
    public float sX,sY;
    // Start is called before the first frame update
    void Start()
    {
        //myRigidbody.freezeRotation = true;
        //transform.position.y;
        myRigidbody.sharedMaterial = inGame_material;
    }

    // Update is called once per frame
    void Update()
    {
        if (isAlive) {
            spdx = myRigidbody.velocity.x;
            spdy = myRigidbody.velocity.y;

            //jump
            if(Input.GetKeyDown(KeyCode.Space) && isGrounded()){
                myRigidbody.velocity += Vector2.up * jumpStrength;
            }

            //debug
            mouX = cam_script.mousePosition.x;
            mouY = cam_script.mousePosition.y;
            sX = transform.position.x;
            sY = transform.position.y;
            disY = (mouY - transform.position.y) * mouse_mult;
            disX = (mouX - transform.position.x) * mouse_mult;

            //recoil angle
            ang = Mathf.Atan(disY/disX);
            //Debug.LogWarning((mouY - transform.position.y) + " " + (mouX - transform.position.x));
            //ang/=Mathf.PI;
            //ang*=180;
            if(mouX - transform.position.x > 0){
                ang+=Mathf.PI;
            }


            //recoil
            if(Input.GetKeyDown(KeyCode.Mouse0) && Time.time >= (time_last_shoot + reloading_time)){
                myRigidbody.velocity += Vector2.right * (Mathf.Cos(ang) * recoil);
                myRigidbody.velocity += Vector2.up * (Mathf.Sin(ang) * recoil);
                time_last_shoot = Time.time;
            }
            //Debug.LogWarning(""+ time_last_shoot + " " + reloading_time + " " + Time.time);

            //move
            tspd = 0;
            if(Input.GetKey(KeyCode.A))tspd += spd;
            if(Input.GetKey(KeyCode.D))tspd -= spd;
            myRigidbody.velocity += Vector2.left * (tspd+spdx)*acc*Time.deltaTime*100;

            //rotation lock
            myRigidbody.MoveRotation(myRigidbody.rotation+(0-myRigidbody.rotation)*aacc*Time.deltaTime*100);

            if(transform.position.y < diedYpos || Input.GetKeyDown(KeyCode.G)){
                isAlive = false;

                //transform.position = Vector2.zero;
                float ang = UnityEngine.Random.Range(-180, 180);
                myRigidbody.velocity += Vector2.up * (float)Math.Sin(ang)*5 + Vector2.right * (float)Math.Cos(ang)*5;
                myRigidbody.angularVelocity = (float)(UnityEngine.Random.Range(-15, 15)/3.0);
                myRigidbody.sharedMaterial = died_material;
            }
        }
        else{
            //died
            mouX = cam_script.mousePosition.x;
            mouY = cam_script.mousePosition.y;
            sX = transform.position.x;
            sY = transform.position.y;
            disY = (mouY - transform.position.y) * mouse_mult;
            disX = (mouX - transform.position.x) * mouse_mult;
            if(transform.position.y < diedYpos-30){
                myRigidbody.simulated = false;
            }
        }
    }

    //ground detect using ray casting
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
