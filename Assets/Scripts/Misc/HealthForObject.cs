using System.Collections;
using System.Collections.Generic;
using Mono.Cecil;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.AI;

public class HealthForObject : NetworkBehaviour
{
    public float health_max;
    private float health;
    private NetworkVariable<float> net_health_max = new NetworkVariable<float>(
        0f,
        NetworkVariableReadPermission.Everyone,
        NetworkVariableWritePermission.Owner
    );
    private NetworkVariable<float> net_health = new NetworkVariable<float>(
        0f,
        NetworkVariableReadPermission.Everyone,
        NetworkVariableWritePermission.Owner
    );
    // Start is called before the first frame update
    void Start()
    {
        health = health_max;
        net_health.Value = health;
        net_health_max.Value = health;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    [Rpc(SendTo.Owner)]
    public void damageRPC(float rate){
        damage(rate);
    }
    [Rpc(SendTo.Owner)]
    public void healRPC(float rate){
        heal(rate);
    }
    [Rpc(SendTo.Owner)]
    public void setToMaxRPC(){
        setToMax();
    }
    private void damage(float rate){
        if(!IsOwner){return;}
        health -= rate;
        health = Mathf.Clamp(health,0,health_max);
        net_health.Value = health;
    }
    private void heal(float rate){
        if(!IsOwner){return;}
        health += rate;
        health = Mathf.Clamp(health,0,health_max);
        net_health.Value = health;
    }
    private void setToMax(){
        if(!IsOwner){return;}
        health = health_max;
        net_health.Value = health;
    }
    public float getHealth(){
        return net_health.Value;
    }
    public float getHealthMax(){
        return net_health_max.Value;
    }
}
