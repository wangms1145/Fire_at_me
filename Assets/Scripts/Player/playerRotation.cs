using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
public class playerRotation : NetworkBehaviour
{
    public PlayerScript varibles;
    public Rigidbody2D myRigidbody;
    [Tooltip("角加速度")]
    public float aacc;
    void Start(){
        varibles = GetComponent<PlayerScript>();
        myRigidbody = GetComponent<Rigidbody2D>();
    }
    public void rotate(){
        myRigidbody.angularVelocity = (0-myRigidbody.rotation)*Math.Clamp(aacc*Time.deltaTime*100,-0.7f,0.7f)/Time.deltaTime;

    }
}
