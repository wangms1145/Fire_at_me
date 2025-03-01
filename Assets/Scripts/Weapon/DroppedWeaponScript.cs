using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class DroppedWeaponScript : NetworkBehaviour
{
    public DroppedWeapon weapon;
    public int spr_tick = 30;
    public GameObject WeaponList;
    private NetworkVariable<int> net_id = new NetworkVariable<int>(
        0,
        NetworkVariableReadPermission.Everyone,
        NetworkVariableWritePermission.Owner
    );
    private NetworkVariable<int> net_mag = new NetworkVariable<int>(
        0,
        NetworkVariableReadPermission.Everyone,
        NetworkVariableWritePermission.Owner
    );
    private float timer = 0;
    // Start is called before the first frame update
    void Start()
    {
        //if(weapon.spr != null)GetComponent<SpriteRenderer>().sprite = weapon.spr;
    }

    // Update is called once per frame
    void Update()
    {
        if(!IsOwner)return;
        timer += Time.deltaTime;
        if(timer > 1f/spr_tick){
            sprRPC(weapon.id);
            net_id.Value = weapon.id;
            net_mag.Value = weapon.mag_now;
        }
        //if(weapon.spr != null)GetComponent<SpriteRenderer>().sprite = weapon.spr;
        if(transform.position.y < -50) GetComponent<Rigidbody2D>().simulated = false;
    }
    public void del()
    {
        delRPC();
    }
    [Rpc(SendTo.Owner)]
    private void delRPC(){
        GetComponent<NetworkObject>().Despawn();
        Destroy(gameObject);
    }
    [Rpc(SendTo.ClientsAndHost)]
    private void sprRPC(int id){
        GetComponent<SpriteRenderer>().sprite = WeaponList.GetComponent<Weapon_Script>().weapon[id].spr;
    }
    public int Getmag(){
        return net_mag.Value;
    }
    public int Getid(){
        return net_id.Value;
    }
}
