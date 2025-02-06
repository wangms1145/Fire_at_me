using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class Grenade_script : NetworkBehaviour
{
    public float exp_time;
    public float aud_off;
    public float damage;
    public float hold_time;
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
        //myRigidbody.MoveRotation(0);
        timed += Time.deltaTime;
        if (timed+hold_time > exp_time)
        {
            Instantiate(explode, transform.position, transform.rotation).GetComponent<explode_script>().damage = damage;
            Destroy(gameObject);
        }
        if (timed+hold_time > exp_time - aud_off && aud_flag)
        {
            aud_flag = false;
            source.PlayOneShot(explode_aud);
        }
    }
}
