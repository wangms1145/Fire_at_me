using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEditor.Timeline;
using UnityEngine;
using Unity.Netcode;
using System.Threading;

public class Bullet4Script : NetworkBehaviour
{
    [SerializeField]private ushort vel_tick_rt;
    private float timed = 0;
    private Rigidbody2D myRigidbody;
    private NetworkObject net;
    private float tick_timer = 0;
    // Start is called before the first frame update
    void Start()
    {
        net = GetComponent<NetworkObject>();
        myRigidbody = gameObject.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if(!IsOwner)return;
        tick_timer += Time.deltaTime;
        if(tick_timer > 1f/vel_tick_rt){
            SyncSpd(myRigidbody.velocity);
            tick_timer = 0;
        }
        if(myRigidbody.velocity.magnitude < 0.07){
            timed += Time.deltaTime;
        }
        else{
            timed = 0;
        }
        if(timed > 10){
            net.Despawn();
            Destroy(gameObject);
        }
        if(transform.position.y < -30){
            net.Despawn();
            Destroy(gameObject);
        }
    }
    public void SyncSpd(Vector2 spd){
        SyncSpdRpc(spd);
    }
    [Rpc(SendTo.ClientsAndHost,RequireOwnership = false)]
    private void SyncSpdRpc(Vector2 spd){
        if(!IsOwner)myRigidbody.velocity = spd;
    }
}
