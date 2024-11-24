using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

public class playerMove : MonoBehaviour
{
    public PlayerScript varibles;
    public playerLogic player_logic;
    public Rigidbody2D myRigidbody;
    [Tooltip("最大速度")]
    public float spd;

    //player move acceleration
    [Tooltip("加速度")]
    public float acc;
    [Tooltip("跳跃力度")]
    public float jumpStrength;
    [Tooltip("是否连跳（没做）")]
    public bool DoBunnyHop;
    private double tspd;

    void Start(){
        varibles = GetComponent<PlayerScript>();
        myRigidbody = GetComponent<Rigidbody2D>();
        player_logic = GetComponent<playerLogic>();
    }
    public void move(){
        varibles.spdx = myRigidbody.velocity.x;
        varibles.spdy = myRigidbody.velocity.y;
        varibles.angSpd = (float)Math.Atan2(varibles.spdy,varibles.spdx);
        varibles.angSpd += (float)Math.PI;
        if(Input.GetKeyDown(KeyCode.Space) && player_logic.isGrounded()){
            myRigidbody.velocity += Vector2.up * jumpStrength;
        }
        tspd = 0;
        if(Input.GetKey(KeyCode.A))tspd += spd;
        if(Input.GetKey(KeyCode.D))tspd -= spd;
        float acc_add = 0;
        if(Input.GetKey(KeyCode.LeftShift) && player_logic.isGrounded()){acc_add = (float)(0.7 - acc); tspd = 0;}
        myRigidbody.velocity += Vector2.left * (float)(tspd+varibles.spdx)*Math.Clamp((acc+acc_add)*Time.deltaTime*100,-1,1);
    }
    
}
