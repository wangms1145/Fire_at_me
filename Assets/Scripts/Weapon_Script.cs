using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif
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
    [Tooltip("玩家代码")]
    public PlayerScript ply;
    [Tooltip("武器（所有武器都在这里）")]
    public WeaponClass[] weapon;
    [Tooltip("角色渲染器")]
    public SpriteRenderer mySprite;
    public float recoil_ani;
    [Tooltip("子弹生成器代码")]
    public Bullet_gene_scr bullet;
    [Tooltip("音效播放器")]
    public AudioSource audSource;
    private float ang,ang_rec;
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
    private float fire_timer = 0f;

    // Start is called before the first frame update
    void Start()
    {
        Change(weapon[0],0);
        myRigidbody.simulated = false;
        scale.x = 0.3f;
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
            int sign = 0;
            ang = Mathf.Atan2(ply.disY,ply.disX);
            float dis_m = (float)Math.Sqrt(Math.Pow(ply.disX,2) + Math.Pow(ply.disY,2));
            gameObject.GetComponentsInChildren<Transform>(false)[1].localPosition = Vector2.right * dis_m * 3.3333f;
            
            if(ply.disX<0){
                scale.y = -0.3f;
                sign = -1;
            }
            else{
                scale.y = 0.3f;
                sign = 1;
            }
            SpriteRenderer sec_wp =  gameObject.GetComponentsInChildren<SpriteRenderer>()[2];
            Transform wec_wp_transform = gameObject.GetComponentsInChildren<Transform>()[2];
            if(weapon[now_ind].duo_hold){
                sec_wp.sprite = gameObject.GetComponent<SpriteRenderer>().sprite;
                wec_wp_transform.localPosition = weapon[now_ind].Sec_pos;
                sec_wp.enabled = true;
                if(weapon[now_ind].bullet_count % 2 == 1){
                    Debug.LogError("双持武器弹夹容量必须是偶数");
                    Debug.Break();
                }
            }
            else{
                sec_wp.enabled = false;
            }
            bool sec = weapon[now_ind].duo_hold && weapon[now_ind].mag_c % 2 == 1;
            if(!weapon[now_ind].delay_fire && !weapon[now_ind].hold_to_fire){
                bool fire = auto ? Input.GetKey(KeyCode.Mouse0) : Input.GetKeyDown(KeyCode.Mouse0);
                if(fire && can_shoot && Time.time > shoot_last_time + firing_time){
                    fire_wp(sign,sec);
                }
            }
            else if(weapon[now_ind].delay_fire && !weapon[now_ind].hold_to_fire){
                bool fire = Input.GetKey(KeyCode.Mouse0);
                if(fire){
                    fire_timer += Time.deltaTime;
                    if(fire_timer > weapon[now_ind].time){
                        fire_wp(sign,sec);
                    }
                }
                else{
                    fire_timer = 0;
                }
                
            }
            else{

            }
            ang_rec += (0-ang_rec)*weapon[now_ind].rec_acc;
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
            
            
            if(Input.GetKeyDown(KeyCode.F)){j++;Change(weapon[j],j);}
            if(j>=weapon.Length - 1){j = -1;}
            transform.localScale = scale;
            if(!reload)transform.rotation = quaternion.RotateZ(ang + ang_rec * sign);
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
    private void fire_wp(int sign,bool sec){
        weapon[now_ind].mag_c--;
        ang_rec = Math.Clamp(ang_rec, 0,weapon[now_ind].ang_rec);
        transform.rotation = quaternion.RotateZ(ang + ang_rec * sign);
        ply.shoot = true; 
        shoot_last_time = Time.time;
        ply.ang = (float)(ang + ang_rec + Math.PI);
        bullet.SpawnBullet(weapon[now_ind],ang + ang_rec * sign,ply.GetComponent<Rigidbody2D>().velocity,sec);
        shoot = true;
        audSource.PlayOneShot(weapon[now_ind].fire_audio);
        ang_rec += weapon[now_ind].ang_rec/5;
    }
}


