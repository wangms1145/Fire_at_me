using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

public class playerMove : NetworkBehaviour
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
    [Tooltip("急停力度")]
    public double acc_stp = 0.7f;
    public int move_tick;
    private double acc_add = 0;
    private double tspd;
    private float timer = 0;

    void Start(){
        varibles = GetComponent<PlayerScript>();
        myRigidbody = GetComponent<Rigidbody2D>();
        player_logic = GetComponent<playerLogic>();
    }
    public void move(){
        if(!IsOwner)return;
        varibles.spdx = myRigidbody.velocity.x;
        varibles.spdy = myRigidbody.velocity.y;
        varibles.angSpd = (float)Math.Atan2(varibles.spdy,varibles.spdx);
        varibles.angSpd += (float)Math.PI;
        if(Input.GetKeyDown(KeyCode.Space) && player_logic.isGrounded()){
            addSpdRPC(Vector2.up * jumpStrength);
        }
        tspd = 0;
        if(Input.GetKey(KeyCode.A))tspd -= spd;
        if(Input.GetKey(KeyCode.D))tspd += spd;
        acc_add = 0;
        double time = Time.deltaTime;
        if(Input.GetKey(KeyCode.LeftShift) && player_logic.isGrounded()){
            acc_add = 0.7 - acc;
            tspd = 0;
            time *= acc_stp;
        }
        double spdc = CalcSpd.calcSpdAdd(acc+acc_add, tspd, varibles.spdx,time);
        addSpdRPC(Vector2.right * (float)spdc);
    }
    [Rpc(SendTo.Server)]
    public void addSpdRPC(Vector2 spd){
        myRigidbody.velocity += spd;
    }
}
