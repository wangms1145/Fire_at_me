using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Mathematics;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Rendering;
public class Bullet_gene_scr : MonoBehaviour
{
    public GameObject[] bullets;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //transform.localPosition = Vector3.zero;
    }
    public void SpawnBullet(WeaponClass wp,float ang){
        //need spawn bullet
        Debug.Log("spawn");
        GameObject bullet = bullets[wp.type];
        transform.localPosition = wp.firePos;
        if(wp.type == 1){
            for (int i = 1; i <= UnityEngine.Random.Range(8,12); i++){
                GameObject bullet_spawned = Instantiate(bullet, transform.position, quaternion.RotateZ(ang));
                float angp = (float)(UnityEngine.Random.Range(-wp.ang_offset,wp.ang_offset)/180.0 * Math.PI);
                float spdp = (float)(UnityEngine.Random.Range(-wp.spd_offset,wp.spd_offset) / 3.0);
                bullet_spawned.GetComponent<Rigidbody2D>().velocity = (float)(Math.Cos(ang+angp) * (wp.bulletSpd+spdp)) * Vector2.right + (float)(Math.Sin(ang+angp) * (wp.bulletSpd+spdp)) * Vector2.up;
            }
        }
        else{
            GameObject bullet_spawned = Instantiate(bullet, transform.position, quaternion.RotateZ(ang));
            float angp = (float)(UnityEngine.Random.Range(-wp.ang_offset,wp.ang_offset)/180.0 * Math.PI);
            float spdp = (float)(UnityEngine.Random.Range(-wp.spd_offset,wp.spd_offset)/3.0);
            bullet_spawned.GetComponent<Rigidbody2D>().velocity = (float)(Math.Cos(ang+angp) * (wp.bulletSpd+spdp)) * Vector2.right + (float)(Math.Sin(ang+angp) * (wp.bulletSpd+spdp)) * Vector2.up;
        }
        
    }
}
