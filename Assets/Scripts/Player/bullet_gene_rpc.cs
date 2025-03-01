using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using Unity.Mathematics;
using System;

public class bullet_gene_rpc : NetworkBehaviour
{
    [SerializeField] private NetworkPrefabsList list;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void RequestSpawn(WeaponClass wp,String name, Vector3 pos,Vector2 ply_vel, float rotate)
    {
        //Debug.Log("" + name);
        SpawnObjectServerRpc(name,pos,ply_vel,wp.damage,wp.bulletSpd,wp.spd_offset,wp.ang_offset,wp.type,rotate,wp.hold_time,wp.start_damage,wp.time);
    }
    [ServerRpc(RequireOwnership = false)]// 允许任何客户端调用
    private void SpawnObjectServerRpc(String Objname,Vector3 pos,Vector2 ply_vel, float damage ,float bulletSpd,float spd_offset,float ang_offset,int type,float rotate,float hold_time,float start_damage,float time, ServerRpcParams rpcParams = default)
    {
        IReadOnlyList<NetworkPrefab> bullets = list.PrefabList;
        //Debug.Log(bullets.Count);
        GameObject bullet = null;
        foreach(NetworkPrefab Netprefab in bullets){
            GameObject prefab = Netprefab.Prefab;
            //Debug.Log(prefab.name);
            if(prefab != null && prefab.name.Equals(Objname)){
                bullet = prefab;
                break;
            }
        }
        /*if(bullet != null)spawned = Instantiate(bullet, pos, quaternion.RotateZ(rotate));
        else Debug.LogError("You forget to add prefab: \"" + Objname + "\" dumbass!");
        spawned.GetComponent<NetworkObject>().Spawn();
        spawned.tag = "just_spawned_bullet";
        */
        switch (type){
            case 0:
                def_spawn(ang_offset,spd_offset,bulletSpd,rotate,bullet,ply_vel,pos).GetComponent<Bullet1Script>().damage = damage/bulletSpd;
                break;
            case 1:
                for (int i = 1; i <= UnityEngine.Random.Range(8,12); i++){
                    def_spawn(ang_offset,spd_offset,bulletSpd,rotate,bullet,ply_vel,pos).GetComponent<Bullet2Script>().damage = damage/bulletSpd;
                }
                break;
            case 2:
                def_spawn(ang_offset,spd_offset,bulletSpd,rotate,bullet,ply_vel,pos).GetComponent<Bullet3Script>().damage = damage;
                break;
            case 3:
                def_spawn(ang_offset,spd_offset,bulletSpd,rotate,bullet,ply_vel,pos);
                break;
            case 4:
                def_spawn(ang_offset,spd_offset,bulletSpd,rotate,bullet,ply_vel,pos).GetComponent<Bullet5Script>().damage = damage/1440;
                break;
            case 5:
                GameObject bullet_spawned = def_spawn(ang_offset,spd_offset,bulletSpd,rotate,bullet,ply_vel,pos);
                bullet_spawned.GetComponent<TNT_script>().damage = damage / 1440;
                break;
            case 6:
                def_spawn(ang_offset,spd_offset,bulletSpd,rotate,bullet,ply_vel,pos).GetComponent<Fish_script>().damage = damage;
                break;
            case 7:
                float damage1 = Math.Clamp(hold_time/time,0,1);
                //Debug.Log(damage1);
                damage1 = Mathf.Lerp(start_damage,damage,damage1);
                //Debug.Log(damage1);
                def_spawn(ang_offset,spd_offset,bulletSpd, rotate, bullet, ply_vel,pos).GetComponent<Bullet6Script>().damage = damage1;
                break;
            case 8:
                Debug.Log(hold_time);
                GameObject spawed = def_spawn(ang_offset,spd_offset,bulletSpd,rotate,bullet,ply_vel,pos);
                spawed.GetComponent<Grenade_script>().damage = damage/1440;
                spawed.GetComponent<Grenade_script>().hold_time = hold_time;
                break;
            default:
                def_spawn(ang_offset,spd_offset,bulletSpd,rotate,bullet,ply_vel,pos);
                break;
        }
    }
    private GameObject def_spawn(float ang_offset,float spd_offset,float bulletSpd,float ang,GameObject bullet,Vector2 ply_vel,Vector3 pos){
        GameObject bullet_spawned = Instantiate(bullet, pos, quaternion.RotateZ(ang));
        float angp = (float)(UnityEngine.Random.Range(-ang_offset,ang_offset)/180.0 * Math.PI);
        float spdp = (float)(UnityEngine.Random.Range(-spd_offset,spd_offset)/3.0);
        bullet_spawned.GetComponent<Rigidbody2D>().velocity = ply_vel;
        bullet_spawned.GetComponent<Rigidbody2D>().velocity += (float)(Math.Cos(ang+angp) * (bulletSpd+spdp)) * Vector2.right + (float)(Math.Sin(ang+angp) * (bulletSpd+spdp)) * Vector2.up;
        bullet_spawned.GetComponent<NetworkObject>().Spawn();
        return bullet_spawned;
    }
}
