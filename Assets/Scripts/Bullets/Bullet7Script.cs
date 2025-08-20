using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;

public class Bullet7Script : NetworkBehaviour
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
        if (!IsOwner) return;
        timed += Time.deltaTime;
        if (timed > exp_time)
        {
            GameObject exp = Instantiate(explode, transform.position, transform.rotation);
            exp.GetComponent<explode_script>().damage = damage;
            exp.GetComponent<explode_script>().sourcePlayer = GetComponent<playerIdentityScript>().player;
            exp.GetComponent<NetworkObject>().Spawn();
            net.Despawn();
            Destroy(gameObject);
        }
        if (timed > exp_time - aud_off && aud_flag)
        {
            aud_flag = false;
            source.PlayOneShot(explode_aud);
        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (!IsOwner) return;
        myRigidbody.simulated = false;
        Debug.Log("" + other.transform.name);
        transform.SetParent(other.transform);
        //transform.SetParent
    }
}
