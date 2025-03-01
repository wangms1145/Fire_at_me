using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.Netcode;
using UnityEngine;

public class WeaponSpawnerScript : MonoBehaviour
{
    public Vector2[] pos;
    public int num_weapons;
    public float spawn_time;
    [SerializeField]private WeaponClass[] weapon_list;
    [SerializeField]private GameObject weapon;
    private float spawn_probablity_sum = 0,timer = 0;
    [SerializeField]private GameObject droppedWeapon;
    // Start is called before the first frame update
    void Awake()
    {
        if(GameObject.FindGameObjectWithTag("NetworkManager").GetComponent<NetworkManager>().IsClient){
            gameObject.SetActive(false);
            Destroy(gameObject);
        }
    }
    void Start()
    {
        
        weapon_list = weapon.GetComponent<Weapon_Script>().weapon;
        foreach(WeaponClass weapon in weapon_list){
            spawn_probablity_sum += weapon.spawn_probablity;
        }
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if(timer > spawn_time){
            timer = 0;
            for(int i = 0; i<num_weapons;i++){
                float rand = UnityEngine.Random.Range(0,spawn_probablity_sum);
                int j = 0;
                while(rand > 0){
                    rand -= weapon_list[j].spawn_probablity;
                    j++;
                }
                j--;
                int ind = UnityEngine.Random.Range(0,pos.Length);
                GameObject spn_wp = Instantiate(droppedWeapon,pos[ind],quaternion.RotateZ(0));
                DroppedWeapon spawned = spn_wp.GetComponent<DroppedWeaponScript>().weapon;
                //spawned.spr = weapon_list[j].spr;
                spawned.id = j;
                spawned.mag_now = weapon_list[j].bullet_count;
                spn_wp.GetComponent<NetworkObject>().Spawn();
            }
        }
    }
}
