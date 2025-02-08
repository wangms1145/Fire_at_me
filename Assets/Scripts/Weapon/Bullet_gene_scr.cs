using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Mathematics;
using Unity.Netcode;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Rendering;
public class Bullet_gene_scr : MonoBehaviour
{
    public GameObject[] bullets;
    [SerializeField] private PlayerScript ply;
    // Start is called before the first frame update
    void Start()
    {
        ply = GetComponentInParent<Transform>().GetComponentInParent<PlayerScript>();
    }

    // Update is called once per frame
    void Update()
    {
        //transform.localPosition = Vector3.zero;
    }
    public void SpawnBullet(WeaponClass wp,float ang,Vector2 ply_vel,bool sec){
        //need spawn bullet
        Debug.Log("spawn");
        GameObject bullet = bullets[wp.type];
        transform.localPosition = sec ? wp.Sec_pos + wp.firePos : wp.firePos;
        switch (wp.type){
            case 0:
                def_spawn(wp,ang,bullet,ply_vel).GetComponent<Bullet1Script>().damage = wp.damage/wp.bulletSpd;
                break;
            case 1:
                for (int i = 1; i <= UnityEngine.Random.Range(8,12); i++){
                    def_spawn(wp,ang,bullet,ply_vel).GetComponent<Bullet2Script>().damage = wp.damage/wp.bulletSpd;
                }
                break;
            case 2:
                def_spawn(wp,ang,bullet,ply_vel).GetComponent<Bullet3Script>().damage = wp.damage/wp.bulletSpd;
                break;
            case 3:
                def_spawn(wp,ang,bullet,ply_vel);
                break;
            case 4:
                def_spawn(wp,ang,bullet,ply_vel).GetComponent<Bullet5Script>().damage = wp.damage/1440;
                break;
            case 5:
                GameObject bullet_spawned = Instantiate(bullet, transform.position, quaternion.RotateZ(0));
                float angp = (float)(UnityEngine.Random.Range(-wp.ang_offset,wp.ang_offset)/180.0 * Math.PI);
                float spdp = (float)(UnityEngine.Random.Range(-wp.spd_offset,wp.spd_offset)/3.0);
                bullet_spawned.GetComponent<TNT_script>().damage = wp.damage / 1440;
                bullet_spawned.GetComponent<Rigidbody2D>().velocity = ply_vel;
                bullet_spawned.GetComponent<Rigidbody2D>().velocity = (float)(Math.Cos(ang+angp) * (wp.bulletSpd+spdp)) * Vector2.right + (float)(Math.Sin(ang+angp) * (wp.bulletSpd+spdp)) * Vector2.up;
                break;
            case 6:
                def_spawn(wp,ang,bullet,ply_vel).GetComponent<Fish_script>().damage = wp.damage;
                break;
            case 7:
                float damage = Math.Clamp(wp.hold_time/wp.time,0,1);
                Debug.Log(damage);
                damage = Mathf.Lerp(wp.start_damage,wp.damage,damage);
                Debug.Log(damage);
                def_spawn(wp, ang, bullet, ply_vel).GetComponent<Bullet6Script>().damage = damage;
                break;
            case 8:
                Debug.Log(wp.hold_time);
                GameObject spawed = def_spawn(wp,ang,bullet,ply_vel);
                spawed.GetComponent<Grenade_script>().damage = wp.damage;
                spawed.GetComponent<Grenade_script>().hold_time = wp.hold_time;
                break;
            default:
                def_spawn(wp,ang,bullet,ply_vel);
                break;
        }
        if(wp.fireEff != null)Instantiate(wp.fireEff,transform.position,transform.rotation,transform);
    }
    GameObject def_spawn(WeaponClass wp,float ang,GameObject bullet,Vector2 ply_vel){
        String name = bullet.name;
        
        ply.RequestSpawn(name, transform.position, ang);
        GameObject bullet_spawned = GameObject.FindGameObjectWithTag("just_spawned_bullet");
        bullet_spawned.tag = "";
        float angp = (float)(UnityEngine.Random.Range(-wp.ang_offset,wp.ang_offset)/180.0 * Math.PI);
        float spdp = (float)(UnityEngine.Random.Range(-wp.spd_offset,wp.spd_offset)/3.0);
        bullet_spawned.GetComponent<Rigidbody2D>().velocity = ply_vel;
        bullet_spawned.GetComponent<Rigidbody2D>().velocity += (float)(Math.Cos(ang+angp) * (wp.bulletSpd+spdp)) * Vector2.right + (float)(Math.Sin(ang+angp) * (wp.bulletSpd+spdp)) * Vector2.up;
        //if(net != null) ply.RequestSpawn(bullet_spawned);
        return bullet_spawned;
    }
}
