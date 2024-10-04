using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Weapon_Script : MonoBehaviour
{
    /************************************************************
    * Read This Before You Code On This Script!                 *
    *                                                           *
    * This is our first weapon code.                            *
    * Our weapon System should be here.                         *
    * Including the weapon choosing System.                     *
    * There will be an array of images and attibutes here       *
    *************************************************************/
    public PlayerScript ply;
    public WeaponClass[] weapon;
    public SpriteRenderer mySprite;
    public float recoil_ani;
    private float ang;
    private bool flag;
    private Vector3 scale;
    public Rigidbody2D myRigidbody;
    public PolygonCollider2D myCol;

    // Start is called before the first frame update
    void Start()
    {
        Change(weapon[0]);
        myRigidbody.simulated = false;
        scale.x = (float)-0.3;
        scale.z = 1;
        myCol.autoTiling = true;
    }

    // Call This method when ever you need to change the attibutes.
    void Change(WeaponClass wp){// There will be much more attributes then this
        ply.recoil = wp.recoil;
        ply.firing_time = wp.firing_time;//In seconds
        mySprite.sprite = wp.spr;
        ply.auto = wp.automatic;
        //transform.localScale = wp.scale;
        
        //Debug.Log(wp.scale);
    }
    // Update is called once per frame
    void Update()
    {
        if(ply.isAlive){
            myRigidbody.simulated = false;

            ang = Mathf.Atan(ply.disY/ply.disX);
            if(ply.disX<0){
                ang += Mathf.PI;
                //scale.y = -Math.Abs(scale.y);
                scale.y = (float)-0.3;
            }
            else{
                scale.y = (float)0.3;
            }


            if(recoil_ani != 0){
                transform.localPosition -= Vector3.right * (float)Math.Cos(ang)*recoil_ani + Vector3.up * (float)Math.Sin(ang)*recoil_ani;
                recoil_ani = 0;
                //Debug.Log(ang);
            }
            transform.localPosition += (Vector3.zero - transform.localPosition)*30*Time.deltaTime;
            flag = true;
            
            
            if(Input.GetKeyDown(KeyCode.C))Change(weapon[1]);
            if(Input.GetKeyDown(KeyCode.F))Change(weapon[0]);
            transform.localScale = scale;
            transform.rotation = quaternion.RotateZ(ang);
        }
        else{
            if(flag){
                flag = false;
                myRigidbody.simulated = true;
                float ang = UnityEngine.Random.Range(-180, 180);
                myRigidbody.velocity = Vector2.up * (float)Math.Sin(ang)*5 + Vector2.right * (float)Math.Cos(ang)*5;
                myRigidbody.angularVelocity = (float)(UnityEngine.Random.Range(-15, 15)/3.0);
            }
            if(transform.position.y < ply.diedYpos-30){
                myRigidbody.simulated = false;
            }
        }
    }
}
