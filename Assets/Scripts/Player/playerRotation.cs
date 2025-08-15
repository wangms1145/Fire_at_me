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
    private NetworkVariable<float> targetAngle = new NetworkVariable<float>(0f,NetworkVariableReadPermission.Everyone,NetworkVariableWritePermission.Owner);
    void Start(){
        varibles = GetComponent<PlayerScript>();
        myRigidbody = GetComponent<Rigidbody2D>();
    }
    public void rotate(){
        //do sth here
        
    }
    void Update()
    {
        if(!IsHost)return;
        if(varibles.isAlive)myRigidbody.angularVelocity = (targetAngle.Value-myRigidbody.rotation)*Math.Clamp(aacc*Time.deltaTime*100,-0.7f,0.7f)/Time.deltaTime;
    }
    public void updateAng(float ang){
        angRPC(ang);
    }
    [Rpc(SendTo.Server)]
    private void angRPC(float ang){
        targetAngle.Value = ang;
    }
}
