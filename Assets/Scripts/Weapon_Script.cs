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
    private Vector2 pos;
    private Vector3 scale;
    private float ang;
    private bool flag;
    public Rigidbody2D myRigidbody;
    // Start is called before the first frame update
    void Start()
    {
        Change(14,(float)0.7);
        myRigidbody.simulated = false;
    }

    // Call This method when ever you need to change the attibutes.
    void Change(float recoil,float time){// There will be much more attributes then this
        ply.recoil = recoil;
        ply.reloading_time = time;//In seconds
    }
    // Update is called once per frame
    void Update()
    {
        if(ply.isAlive){
            myRigidbody.simulated = false;
            transform.localPosition = Vector3.zero;
            flag = true;
            /*
            pos.x = ply.sX;
            pos.y = ply.sY;
            transform.position = pos;
            */

            ang = Mathf.Atan(ply.disY/ply.disX);
            scale.x = (float)-0.3;
            scale.z = (float)1;
            if(ply.disX<0){
                ang += Mathf.PI;
                scale.y = (float)-0.3;
            }
            else{
                scale.y = (float)0.3;
            }
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
