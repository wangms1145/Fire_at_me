using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Timeline;

public class Weapon_Script : MonoBehaviour
{
    /************************************************************
    * Read This Before You Code On This Script!                 *
    *                                                           *
    * This is our first weapon code.                            *
    * Our weapon System should be here.                         *
    * Including the weapon choosing System.                     *
    * There will be an array of images and attibutes here       *
    *************************************************************/
    public PlayerScript ply;
    public WeaponClass[] weapon;
    public SpriteRenderer mySprite;
    public float recoil_ani;
    public Bullet_gene_scr bullet;
    public AudioSource audSource;
    private float ang;
    private bool flag;
    private Vector3 scale;
    private int now_ind;
    private float time;
    private bool reload;
    private int last_wp;
    private float fl;
    private bool can_shoot,shoot;
    private bool auto;
    public Rigidbody2D myRigidbody;
    private float shoot_last_time;
    private float firing_time;
    private int j = 0;

    // Start is called before the first frame update
    void Start()
    {
        Change(weapon[0],0);
        myRigidbody.simulated = false;
        scale.x = (float)-0.3;
        scale.z = 1;
        for(int i = 0;i<weapon.Length;i++){
            weapon[i].mag_c = weapon[i].bullet_count;
        }
        can_shoot = true;
    }

    // Call This method when ever you need to change the attibutes.
    void Change(WeaponClass wp,int ind){// There will be much more attributes then this
        now_ind = ind;
        ply.recoil = wp.recoil;
        auto = wp.automatic;
        ply.time_last_shoot = Time.time + wp.arm_time - wp.firing_time;
        firing_time = wp.firing_time;//In seconds
        mySprite.sprite = wp.spr;
        reload = false;
        time = 0;
        shoot = false;
        audSource.clip = wp.fire_audio;
    }
    // Update is called once per frame
    void Update()
    {
        if(ply.isAlive){
            myRigidbody.simulated = false;

            ang = Mathf.Atan(ply.disY/ply.disX);
            if(ply.disX<0){
                ang += Mathf.PI;
                scale.y = (float)-0.3;
            }
            else{
                scale.y = (float)0.3;
            }
            bool fire = auto ? Input.GetKey(KeyCode.Mouse0) : Input.GetKeyDown(KeyCode.Mouse0);
            if(fire && can_shoot && Time.time > shoot_last_time + firing_time){
                weapon[now_ind].mag_c--;
                ply.shoot = true; 
                shoot_last_time = Time.time;
                bullet.SpawnBullet(weapon[now_ind],ang);
                shoot = true;
                audSource.PlayOneShot(weapon[now_ind].fire_audio);
            }
            if(weapon[now_ind].mag_c <= 0 && !reload){
                can_shoot=false;
                SetReload();
            }
            else if(!reload || (weapon[now_ind].reload_type == 1 && weapon[now_ind].mag_c > 0)){
                can_shoot = true;
            }
            else{
                can_shoot = false;
            }
            if(Input.GetKeyDown(KeyCode.R) && weapon[now_ind].mag_c < weapon[now_ind].bullet_count && !reload){
                SetReload();
            }
            if(reload && (weapon[now_ind].reload_type == 0)){
                time+=Time.deltaTime;
                if(now_ind != last_wp){
                    CancelReload();
                }
                last_wp = now_ind;
                if(time > weapon[now_ind].reloading_time){
                    weapon[now_ind].mag_c = weapon[now_ind].bullet_count;
                    CancelReload();
                }
            }
            else if(reload && (weapon[now_ind].reload_type == 1)){
                time+=Time.deltaTime;
                //Debug.Log(time);
                if(now_ind != last_wp || shoot){
                    CancelReload();
                }
                last_wp = now_ind;
                if(time > weapon[now_ind].reloading_time && weapon[now_ind].mag_c < weapon[now_ind].bullet_count){
                    weapon[now_ind].mag_c++;
                    time = 0;
                }
                if(weapon[now_ind].mag_c == weapon[now_ind].bullet_count && fl == 9999){
                    fl = time;
                }
                if(time > (fl + weapon[now_ind].arm_time)){
                    CancelReload();
                }
            } 


            if(recoil_ani != 0){
                transform.localPosition -= Vector3.right * (float)Math.Cos(ang)*recoil_ani + Vector3.up * (float)Math.Sin(ang)*recoil_ani;
                recoil_ani = 0;
            }
            transform.localPosition += (Vector3.zero - transform.localPosition)*30*Time.deltaTime;
            flag = true;
            
            
            if(Input.GetKeyDown(KeyCode.F)){Change(weapon[j],j);j++;}
            if(j>weapon.Length - 1){j = 0;}
            transform.localScale = scale;
            if(!reload)transform.rotation = quaternion.RotateZ(ang);
            else if(weapon[now_ind].reload_type == 0) transform.rotation = quaternion.RotateX(10*Time.time);
            else transform.rotation = quaternion.RotateZ(1*weapon[now_ind].mag_c);
            shoot = false;
        }
        else{
            if(flag){
                reload = false;
                time = 0;
                for(int i = 0;i<weapon.Length;i++){
                    weapon[i].mag_c = weapon[i].bullet_count;
                }
                can_shoot = true;
                flag = false;
                myRigidbody.simulated = true;
                float ang = UnityEngine.Random.Range(-180, 180);
                myRigidbody.velocity = Vector2.up * (float)Math.Sin(ang)*5 + Vector2.right * (float)Math.Cos(ang)*5;
                myRigidbody.angularVelocity = (float)(UnityEngine.Random.Range(-15, 15)/3.0);
            }
            if(transform.position.y < ply.diedYpos-30){
                myRigidbody.simulated = false;
            }
        }
    }
    private void SetReload(){
        reload = true;
        fl = 9999;
        time = 0;
        if(weapon[now_ind].reload_type == 1){
            time = (float)-0.5;
        }
    }
    private void CancelReload(){
        reload = false;
        time = 0;
    }
}
