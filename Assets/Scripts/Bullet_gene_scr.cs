using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Mathematics;
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
        GameObject bullet = bullets[wp.damageType];
        transform.localPosition = wp.firePos;
        //bullet.GetComponent<Rigidbody2D>().velocity = (float)(Math.Cos(ang) * wp.bulletSpd) * Vector2.right + (float)(Math.Sin(ang) * wp.bulletSpd) * Vector2.up;
        GameObject bullet_spawned = Instantiate(bullet, transform.position, quaternion.RotateZ(ang));
        bullet_spawned.GetComponent<Rigidbody2D>().velocity = (float)(Math.Cos(ang) * wp.bulletSpd) * Vector2.right + (float)(Math.Sin(ang) * wp.bulletSpd) * Vector2.up;
    }
}
