using System.Collections;
using System.Collections.Generic;
using ParrelSync.NonCore;
using Unity.Mathematics;
using Unity.Netcode;
using Unity.Netcode.Components;
using Unity.VisualScripting;
using UnityEngine;

public class Bullet8Script : NetworkBehaviour
{
    public float strength;
    public float decay_dis;
    public AudioSource source;
    public AudioClip hang;
    private Rigidbody2D myRigidbody;
    private bool hang_on = false;
    private NetworkObject net;
    public GameObject player;
    public GameObject connected_to;
    public GameObject hang_to;
    public Rigidbody2D other_rigid;
    public Vector2 dir;
    public float ext_time;
    private LineRenderer lr;
    private float timer = 0;
    // Start is called before the first frame update
    void Start()
    {
        net = GetComponent<NetworkObject>();
        myRigidbody = gameObject.GetComponent<Rigidbody2D>();
        lr = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!IsOwner) return;
        timer += Time.deltaTime;
        lr.SetPosition(0,transform.position);
        lr.SetPosition(1,connected_to.transform.position);
        if (connected_to.Equals(player))
        {
            if (hang_on)
            {

                calcDir(player);
                player.GetComponent<Rigidbody2D>().AddForce(dir * strength * 570 * Time.deltaTime);
                if (other_rigid != null)
                {
                    other_rigid.AddForceAtPosition(-dir * strength * 570 * Time.deltaTime, transform.position);
                }
            }
        }
        else
        {
            if (hang_on && connected_to.GetComponent<Bullet8Script>().hang_on)
            {

                if (other_rigid != null)
                {
                    calcDir(connected_to);
                    other_rigid.GetComponent<Rigidbody2D>().AddForceAtPosition(-dir * strength  * 570 * Time.deltaTime, transform.position);
                }
            }
            if (timer >= ext_time)
            {
                Destroy(connected_to);
            }
        }
        if (timer >= ext_time)
        {
            Destroy(gameObject);
        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (!IsOwner) return;
        myRigidbody.simulated = false;
        Debug.Log("" + other.transform.name);
        if (other.transform.GetComponent<NetworkTransform>() != null)
        {
            transform.SetParent(other.transform);

        }
        else if (other.transform.GetComponent<Rigidbody2D>() != null)
        {
            Debug.LogError("Try to hang on a physics object that is not synced through network. Name:" + other.transform.name);
        }
        hang_on = true;
        hang_to = other.gameObject;
        other_rigid = hang_to.GetComponent<Rigidbody2D>();
        //transform.SetParent
    }
    void calcDir(GameObject other)
    {
        dir = transform.position;
        dir -= (Vector2)other.transform.position;
        if (decay_dis > 0)
        {
            dir /= decay_dis;
            if (dir.magnitude > 1) dir = dir.normalized;
        }
        else
        {
            dir = dir.normalized;
        }
        
    }
}
