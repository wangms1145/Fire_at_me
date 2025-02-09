using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using Unity.Mathematics;

public class TNT_script : MonoBehaviour
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
    // Start is called before the first frame update
    void Start()
    {
        myRigidbody = gameObject.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        myRigidbody.MoveRotation(0);
        transform.rotation = quaternion.RotateZ(0);
        timed += Time.deltaTime;
        if(timed > exp_time){
            Instantiate(explode,transform.position,transform.rotation).GetComponent<explode_script>().damage = damage;
            Destroy(gameObject);
        }
        if(timed > exp_time - aud_off && aud_flag){
            aud_flag = false;
            source.PlayOneShot(explode_aud);
        }
    }
    
}
