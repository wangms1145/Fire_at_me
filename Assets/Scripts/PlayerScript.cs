using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;
using System;
using Unity.VersionControl.Git.ICSharpCode.SharpZipLib;
using System.Text.RegularExpressions;

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
    public bool shoot;
    public AudioSource audSource;
    public AudioClip fall;
    public AudioClip died;
    public AudioClip zhuiji;
    public AudioDistortionFilter filter;
    //public int mag,c_mag;

   
    [SerializeField]
    public Cam_script cam_script;
    public Weapon_Script wp;

    private float ang;
    private float spdx,spdy;
    private float tspd;
    public float time_last_shoot = -999;// Initialized to make sure you could shoot when ever you start the game
    public float mouY,mouX;
    public float disY,disX;
    public float sX,sY;
    private bool groundFlag;
    private float ys = 0;
    private int i;
    // Start is called before the first frame update
    void Start()
    {
        myRigidbody.sharedMaterial = inGame_material;
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isAlive) {
            myRigidbody.sharedMaterial = inGame_material;
            spdx = myRigidbody.velocity.x;
            spdy = myRigidbody.velocity.y;

            //jump
            if(Input.GetKeyDown(KeyCode.Space) && isGrounded()){
                myRigidbody.velocity += Vector2.up * jumpStrength;
            }

            //debug    Actually don't delete this bcs cameraPos takes that parameter.
            mouX = cam_script.mousePosition.x;
            mouY = cam_script.mousePosition.y;
            sX = transform.position.x;
            sY = transform.position.y;
            disY = (mouY - transform.position.y) * mouse_mult;
            disX = (mouX - transform.position.x) * mouse_mult;

            //recoil angle
            ang = Mathf.Atan(disY/disX);
            if(mouX - transform.position.x > 0){
                ang+=Mathf.PI;
            }


            //recoil
            if(shoot){
                myRigidbody.velocity += Vector2.right * (Mathf.Cos(ang) * recoil);
                myRigidbody.velocity += Vector2.up * (Mathf.Sin(ang) * recoil);
                time_last_shoot = Time.time;
                wp.recoil_ani = (float)Math.Clamp(recoil / 6.0,0,0.7);
                shoot = false;
            }

            //move
            tspd = 0;
            if(Input.GetKey(KeyCode.A))tspd += spd;
            if(Input.GetKey(KeyCode.D))tspd -= spd;
            float acc_add = 0;
            if(Input.GetKey(KeyCode.LeftShift) && isGrounded()){acc_add = (float)(0.7 - acc); tspd = 0;}
            myRigidbody.velocity += Vector2.left * (tspd+spdx)*(acc+acc_add)*Time.deltaTime*100;

            //rotation lock
            myRigidbody.MoveRotation(myRigidbody.rotation+(0-myRigidbody.rotation)*aacc*Time.deltaTime*100);

            if(transform.position.y < diedYpos || Input.GetKeyDown(KeyCode.G)){
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
            if(Input.GetKey(KeyCode.R)){
                myRigidbody.simulated = true;
                myRigidbody.velocity = Vector2.zero;
                myRigidbody.position = Vector2.zero;
                myRigidbody.rotation = 0;
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
