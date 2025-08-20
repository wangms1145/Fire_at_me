using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using Unity.Netcode;
public class Bullet5Script : NetworkBehaviour
{
    public Vector2 vel;
    public float radius;
    public float damage;
    public LayerMask groundLayer;
    public GameObject explode;
    private float timed = 0;
    private Rigidbody2D myRigidbody;
    private NetworkObject net;
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
        timed += Time.deltaTime;
        if(timed > 30){
            net.Despawn();
            Destroy(gameObject);
        }
        vel = myRigidbody.velocity;
        RaycastHit2D hit = collide_check();
        if(hit){
            //Instantiate(explode,transform.position,quaternion.RotateZ(0)).GetComponent<explode_script>().damage = damage;
            GameObject exp = Instantiate(explode,transform.position,transform.rotation);
            exp.GetComponent<explode_script>().damage = damage;
            exp.GetComponent<explode_script>().sourcePlayer = GetComponent<playerIdentityScript>().player;
            exp.GetComponent<NetworkObject>().Spawn();
            net.Despawn();
            Destroy(gameObject);
        }
    }
    private RaycastHit2D collide_check(){
        return Physics2D.CircleCast(transform.position,radius,Vector2.right,0,groundLayer);
    }
    private void OnDrawGizmos(){
        //Gizmos.DrawWireCube(transform.position + vel/myRigidbody.velocity.magnitude * (boxSize.x/2-0.1f),boxSize);
        Gizmos.DrawWireSphere(transform.position,radius);
    }
}
