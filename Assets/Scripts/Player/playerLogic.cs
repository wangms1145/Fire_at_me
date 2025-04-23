using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using static UnityEngine.ParticleSystem;
using Unity.Netcode;
using System.Data;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering.Universal;
using Unity.VisualScripting;
public class playerLogic : NetworkBehaviour
{
    public PlayerScript varibles;
    public Rigidbody2D myRigidbody;
    //ray casting box size
    [Tooltip("地面检测碰撞箱大小")]
    public Vector2 boxSize;
    //ray casting distance
    [Tooltip("碰撞箱距离")]
    public float castDistance;
    //ray casting layermark
    [Tooltip("地面图层")]
    public LayerMask groundLayer;
    [Tooltip("游玩时物理")]
    public PhysicsMaterial2D inGame_material;
    [Tooltip("死人物理")]
    public PhysicsMaterial2D died_material;
    [Tooltip("特效最大速度")]
    public float max_eff_spd;
    [Tooltip("特效最小速度")]
    public float min_eff_spd;
    [Tooltip("血量")]
    [SerializeField]
    private float health;
    [Tooltip("最大血量")]
    [SerializeField]
    private float max_health;
    [SerializeField]
    private float health_tick;
    [HideInInspector]
    public bool groundFlag;
    [HideInInspector]
    public float ys = 0;
    public float weight;
    public bool heavy;
    public float heavy_time_max;
    public float heavy_time;
    public float heavy_time_cost_start;
    public float heavy_time_rate;
    private int i;
    private float timer = 0;

    [SerializeField] private GameObject thisWeaponWheel;
    [SerializeField] private NetworkVariable<float> weapon_turn = new(0f,NetworkVariableReadPermission.Everyone,NetworkVariableWritePermission.Owner);
    [SerializeField] private NetworkVariable<float> healthNet = new(0f,NetworkVariableReadPermission.Everyone,NetworkVariableWritePermission.Owner);
    [SerializeField] private NetworkVariable<float> healthMaxNet = new(0f,NetworkVariableReadPermission.Everyone,NetworkVariableWritePermission.Owner);
    [SerializeField] private NetworkVariable<Vector2> Velocity = new(new Vector2(0,0),NetworkVariableReadPermission.Everyone,NetworkVariableWritePermission.Owner);
    private SlidebarForImage slideBar;
    [SerializeField] private List<GameObject> spawns = new List<GameObject>();
    [SerializeField] private GameObject lowSpawn;

    [Rpc(SendTo.ClientsAndHost)]
    public void ChangeWeaponClientRpc(int ind){
        //if(GetComponentInParent<PlayerScript>().IsOwner){return;}
        GetComponentInChildren<Weapon_Script>().ChangeWeapon(ind);

    }
    public void RotateWeapon(float ang){
        weapon_turn.Value = ang;
    }
    public float GetWeaponAng(){
        return weapon_turn.Value;
    }
    void Start(){
        if(SceneManager.GetActiveScene().name.Contains("Start")) GetComponent<Light2D>().enabled = false;
        varibles = GetComponent<PlayerScript>();
        myRigidbody = GetComponent<Rigidbody2D>();
        myRigidbody.sharedMaterial = inGame_material;
        thisWeaponWheel = transform.GetChild(4).gameObject;
        health = max_health;
        heavy_time = heavy_time_max;
        slideBar = GetComponentInChildren<PlayerUI>().GetComponentInChildren<SlidebarForImage>();
        respawnRigidbodyRPC();
    }
    public bool isGrounded(){
        if(Physics2D.BoxCast(transform.position,boxSize,0,Vector2.down,castDistance,groundLayer)){
            return true;
        }
        else{
            return false;
        }
    }
    public void logic(){
        if(IsHost){
            timer += Time.deltaTime;
            if(timer > 1f/health_tick){
                //Debug.Log(IsOwner);
                healthNet.Value = health;
                healthMaxNet.Value = max_health;
                timer = 0;
                //Velocity.Value = myRigidbody.velocity;
            }
        }
        if(!IsOwner) return;
        if(heavy_time < heavy_time_max && !Input.GetKey(KeyCode.Mouse1)){
            myRigidbody.mass = weight;
            heavy = false;
            heavy_time += heavy_time_rate * Time.deltaTime;
            
        }
        else if(Input.GetKey(KeyCode.Mouse1) && heavy_time > 0f){
            myRigidbody.mass = weight * 10;
            heavy = true;
            if(Input.GetKeyDown(KeyCode.Mouse1))heavy_time -= heavy_time_cost_start;
            heavy_time -= Time.deltaTime;
        }
        else{
            myRigidbody.mass = weight;
            heavy = false;
        }
        heavy_time = Mathf.Clamp(heavy_time,0f,heavy_time_max);
        slideBar.setAmount(heavy_time/heavy_time_max);
        if(myRigidbody.sharedMaterial.Equals(inGame_material) == false)myRigidbody.sharedMaterial = inGame_material;
        Color col = Color.white;
        col.a = Mathf.InverseLerp(min_eff_spd,max_eff_spd,myRigidbody.velocity.magnitude);
        transform.GetChild(0).rotation = quaternion.RotateZ(varibles.angSpd);
        TrailModule a = transform.GetChild(0).GetComponent<ParticleSystem>().trails;
        a.colorOverTrail = col;
        transform.GetChild(1).rotation = quaternion.RotateZ(varibles.angSpd);
        a = transform.GetChild(1).GetComponent<ParticleSystem>().trails;
        a.colorOverTrail = col;
        if(myRigidbody.velocity.magnitude > 0.1){
            if(GetComponentInChildren<Weapon_Script>().firing()){
                GetComponent<SpriteRenderer>().flipX = GetComponentInChildren<Weapon_Script>().flip();
            }
            else if(myRigidbody.velocity.x < 0){
                GetComponent<SpriteRenderer>().flipX = true;
            }
            else{
                GetComponent<SpriteRenderer>().flipX = false;
            }
        }
        if(transform.position.y < varibles.diedYpos || Input.GetKeyDown(KeyCode.G) || health < 0){
            varibles.isAlive = false;
            float ang = UnityEngine.Random.Range(-180, 180);
            myRigidbody.velocity += Vector2.up * (float)Math.Sin(ang)*5 + Vector2.right * (float)Math.Cos(ang)*5;
            myRigidbody.angularVelocity = (float)(UnityEngine.Random.Range(-15, 15)/3.0);
            myRigidbody.sharedMaterial = died_material;
            GetComponent<playerSound>().onStartDeath();//死亡音效
        }

        if(i >= 5){ys = myRigidbody.velocity.y;i = 0;}
        i++;
        if(isGrounded() && !groundFlag){
            GetComponent<playerSound>().fallsound();//摔落音效
        }
        groundFlag = isGrounded();

        if(Input.GetKey(KeyCode.T))
        {
            thisWeaponWheel.SetActive(true);
        }

    }
    public void ifNotOwner(){
        if(IsOwner)return;
        //GetComponent<Rigidbody2D>().velocity = Velocity.Value;
    }
    public void onDeath(){
        if(transform.position.y < varibles.diedYpos-30){
            disablePhysicsRPC();
        }
        if(Input.GetKey(KeyCode.R)){
            respawnRigidbodyRPC();
            health = max_health;
            heavy_time = heavy_time_max;
            varibles.isAlive = true;
        }
    }
    [Rpc(SendTo.Server)]
    private void respawnRigidbodyRPC(){
        myRigidbody.simulated = true;
        myRigidbody.velocity = Vector2.zero;
        myRigidbody.position = Vector2.zero;
        myRigidbody.rotation = 0;
        updateSpawn();
        if(lowSpawn != null)
            myRigidbody.position = lowSpawn.transform.position;
        
    }
    [Rpc(SendTo.Server)]
    private void disablePhysicsRPC(){
        myRigidbody.simulated = false;
    }
    private void OnDrawGizmos(){
        Gizmos.DrawWireCube(transform.position - Vector3.up * castDistance,boxSize);
    }
    public float GetHealth(){
        return health;
    }
    public float GetMaxHealth(){
        return max_health;
    }
    public float GetHealthNet(){
        return healthNet.Value;
    }
    public float GetMaxHealthNet(){
        return healthMaxNet.Value;
    }

    public void SetHealth(float health){
        if(varibles.IsOwner)this.health = health;
    }
    public void damage(float damage){
        damageRpc(damage);
    }
    [Rpc(SendTo.ClientsAndHost,RequireOwnership = false)]
    private void damageRpc(float damage){
        if(!IsOwner) return;
        health -= damage;
    }
    private void updateSpawn(){
        spawns.Clear();
        foreach(GameObject spawn in GameObject.FindGameObjectsWithTag("SpawnPoint")){
            spawns.Add(spawn);
            if(lowSpawn == null){
                lowSpawn = spawn;
            }
            else if(lowSpawn.transform.position.y > spawn.transform.position.y){
                lowSpawn = spawn;
            }
        }
    }
}
