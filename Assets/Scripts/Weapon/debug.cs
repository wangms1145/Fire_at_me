using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class debug : MonoBehaviour
{
    /*******************************************************************
    * Read This first before changing this code!!!!                    *
    *                                                                  *
    * This code is for debuging mouse position only                    *
    *                                                                  *
    * PLS create a new script for the aim point and crosshair!!!!!!    *
    ********************************************************************/
    public PlayerScript ply;
    public Rigidbody2D myRigidbody;
    private UnityEngine.Vector3 pos;
    private bool flag;
    // Start is called before the first frame update
    void Start()
    {
        myRigidbody.simulated = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(ply != null && ply.isAlive){
            myRigidbody.simulated = false;
            myRigidbody.rotation = 0;
            transform.rotation = Quaternion.Euler(0,0,0);
            flag = true;
            //displaying mouse pos using the player as an origin
            pos.x = ply.sX + ply.disX;
            pos.y = ply.sY + ply.disY;
            pos.z = 0;
            transform.localPosition = pos;
        }
        else{
            //died
            if(flag){
                flag = false;
                myRigidbody.simulated = true;
                float ang = UnityEngine.Random.Range(-180, 180);
                myRigidbody.velocity = Vector2.up * (float)Math.Sin(ang)*5 + Vector2.right * (float)Math.Cos(ang)*5;
                myRigidbody.angularVelocity = (float)(UnityEngine.Random.Range(-15, 15)/3.0);
            }
            if(transform.position.y<-40f){
                myRigidbody.simulated = false;
            }
        }
    }
}
