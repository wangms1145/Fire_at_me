using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEditor.PackageManager;
using UnityEngine.LowLevel;
using System.Net;





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
    public GameObject weapon_ls;
    [Tooltip("武器（所有武器都在这里）")]
    public WeaponClass[] weapon;
    [Tooltip("角色渲染器")]
    public SpriteRenderer mySprite;
    public float recoil_ani;
    [Tooltip("子弹生成器代码")]
    public Bullet_gene_scr bullet;
    [Tooltip("音效播放器")]
    public AudioSource audSource;
    [Tooltip("JSON文件名")]
    [SerializeField] private String jsName;
    [Tooltip("武器旋转同步速率")]
    [SerializeField] private int rotateTick;
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
    private bool fire_flag = true;
    private playerLogic plyLogic;
    private float tick_timer = 0f;

    // Start is called before the first frame update

    
    public void Change(int ind){
        if(!GetComponentInParent<PlayerScript>().IsOwner){return;}
        ChangeWeapon(ind);
        GetComponentInParent<playerLogic>().ChangeWeaponClientRpc(ind);
        Debug.Log("ServerRpc called by owner on" +GetComponentInParent<PlayerScript>().OwnerClientId);
    }

    // Call This method when ever you need to change the attibutes.
    public void ChangeWeapon(int ind){// There will be much more attributes then this
        WeaponClass wp = weapon[ind];
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
        SpriteRenderer sec_wp =  gameObject.GetComponentsInChildren<SpriteRenderer>()[2];
        Transform sec_wp_transform = gameObject.GetComponentsInChildren<Transform>()[2];
        if(weapon[now_ind].duo_hold){
            sec_wp.sprite = gameObject.GetComponent<SpriteRenderer>().sprite;
            sec_wp_transform.localPosition = weapon[now_ind].Sec_pos;
            sec_wp.enabled = true;
            if(weapon[now_ind].bullet_count % 2 == 1){
                Debug.LogError("双持武器弹夹容量必须是偶数，请检查Element"+now_ind);
                Debug.Break();
            }
        }
        else{
            sec_wp.enabled = false;
        }
    }


    void Start()
    {
        if(name.Equals("Weapon_list"))return;
        plyLogic = GetComponentInParent<playerLogic>();
        Change(0);
        myRigidbody.simulated = false;
        scale.x = 0.3f;
        scale.z = 1;
        for(int i = 0;i<weapon.Length;i++){
            weapon[i].mag_c = weapon[i].bullet_count;
            if(weapon[i].infinite)weapon[i].bullet_count = 10;
        }
        can_shoot = true;
    }
    // Update is called once per frame
    void Update()
    {
        if(name.Equals("Weapon_list"))return;
        weapon = weapon_ls.GetComponent<Weapon_Script>().weapon;
        if(!GetComponentInParent<PlayerScript>().IsOwner){
            transform.rotation = quaternion.RotateZ(plyLogic.GetWeaponAng());
            return;
        }
        if(ply.isAlive){
            tick_timer += Time.deltaTime;
            
            myRigidbody.simulated = false;

            int sign;
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



            bool sec = weapon[now_ind].duo_hold && weapon[now_ind].mag_c % 2 == 1;


            //蓄力
            if(!weapon[now_ind].delay_fire && !weapon[now_ind].hold_to_fire){
                bool fire = auto ? Input.GetKey(KeyCode.Mouse0) : Input.GetKeyDown(KeyCode.Mouse0);
                if(fire && can_shoot && Time.time > shoot_last_time + firing_time){
                    fire_wp(sign,sec);
                }
            }
            else if(weapon[now_ind].delay_fire && !weapon[now_ind].hold_to_fire){
                bool fire = Input.GetKey(KeyCode.Mouse0);
                if(fire && can_shoot && Time.time > shoot_last_time + firing_time){
                    fire_timer += Time.deltaTime;
                    if(fire_timer >= weapon[now_ind].time && can_shoot && Time.time > shoot_last_time + firing_time){
                        fire_wp(sign,sec);
                        fire_timer = 0;
                    }
                }
                else{
                    fire_timer = 0;
                }
                
            }
            else{
                bool fire = Input.GetKey(KeyCode.Mouse0);
                bool fire_up = Input.GetKeyUp(KeyCode.Mouse0);
                if(fire && fire_flag && can_shoot && Time.time > shoot_last_time + firing_time){
                    fire_timer += Time.deltaTime;
                    Debug.Log(fire_timer);
                    
                    transform.localPosition = Vector3.up * UnityEngine.Random.Range(-1f,1f)/30f + Vector3.right * UnityEngine.Random.Range(-1f,1f)/30f;
                    float offset = fire_timer/weapon[now_ind].time * 0.2f;
                    transform.localPosition -= Vector3.right * (float)Mathf.Cos(ang)*offset + Vector3.up * (float)Mathf.Sin(ang)*offset;
                }
                if(fire_timer >= weapon[now_ind].time){
                    fire_timer = weapon[now_ind].time;
                    if(weapon[now_ind].auto_release && can_shoot && Time.time > shoot_last_time + firing_time){
                        weapon[now_ind].hold_time = fire_timer;
                        fire_wp(sign,sec);
                        fire_timer = 0;
                        if(!auto)fire_flag = false;
                    }
                }
                if(fire_up && can_shoot && Time.time > shoot_last_time + firing_time && fire_flag){
                    weapon[now_ind].hold_time = fire_timer;
                    fire_wp(sign,sec);
                    fire_timer = 0;
                    if(!auto)fire_flag = false;
                }
                if(!auto && fire_up)fire_flag = true;
                //weapon[now_ind].hold_time = fire_timer;
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
            
            
            if(Input.GetKeyDown(KeyCode.F)){j++;Change(j);}
            if(j>=weapon.Length - 1){j = -1;}

            transform.localScale = scale;
            if(tick_timer > 1f/rotateTick){
                plyLogic.RotateWeapon(ang + ang_rec * sign);
            }
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
//        Debug.Log(weapon[now_ind].hold_time);
        if(!weapon[now_ind].infinite)weapon[now_ind].mag_c--;
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
    public int GetIndex(){
        return now_ind;
    }
    public int GetMag(){
        return weapon[now_ind].bullet_count;
    }
    public int GetLeft(){
        return weapon[now_ind].mag_c;
    }
    public WeaponClass GetWeapon(){
        return weapon[now_ind];
    }
}
