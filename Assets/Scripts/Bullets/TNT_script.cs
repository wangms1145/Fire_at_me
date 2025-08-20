using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using Unity.Mathematics;

public class TNT_script : NetworkBehaviour
{
    public float exp_time;
    public float aud_off;
    public float damage;
    public AudioSource source;
    public AudioClip explode_aud;
    public GameObject explode;
    private float timed;
    private Rigidbody2D myRigidbody;
    private bool aud_flag = true;
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
        myRigidbody.MoveRotation(0);
        transform.rotation = quaternion.RotateZ(0);
        timed += Time.deltaTime;
        if(timed > exp_time){
            GameObject exp = Instantiate(explode,transform.position,transform.rotation);
            exp.GetComponent<explode_script>().damage = damage;
            exp.GetComponent<explode_script>().sourcePlayer = GetComponent<playerIdentityScript>().player;
            exp.GetComponent<NetworkObject>().Spawn();
            net.Despawn();
            Destroy(gameObject);
        }
        if(timed > exp_time - aud_off && aud_flag){
            aud_flag = false;
            source.PlayOneShot(explode_aud);
        }
    }
    
}
