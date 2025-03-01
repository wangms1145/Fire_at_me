using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Unity.Netcode;
using UnityEngine;

public class BagManagerRpc : NetworkBehaviour
{
    [SerializeField] private GameObject droppedWeaponPrefab;
    [SerializeField] private PlayerScript ply;
    [SerializeField] private Weapon_Script weapon_Script;
    // Start is called before the first frame update
    void Start()
    {
        weapon_Script = GetComponentInChildren<Weapon_Script>();
        ply = GetComponent<PlayerScript>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void throwWeapon(Vector3 pos,int id,Vector2 m_pointer){
        throwWeaponRPC(pos,id,m_pointer);
    }
    [Rpc(SendTo.Server)]
    private void throwWeaponRPC(Vector3 pos,int id,Vector2 m_pointer){
        GameObject throwedWeapon = Instantiate(droppedWeaponPrefab, pos, Quaternion.identity);
        throwedWeapon.GetComponent<Rigidbody2D>().velocity = m_pointer.normalized * 10;
        throwedWeapon.GetComponent<DroppedWeaponScript>().weapon.id = id;
        //throwedWeapon.GetComponent<DroppedWeaponScript>().weapon.spr = weapon_Script.weapon[id].spr;
        throwedWeapon.GetComponent<DroppedWeaponScript>().weapon.mag_now = weapon_Script.weapon[id].mag_c;
        throwedWeapon.GetComponent<NetworkObject>().Spawn();
    }
}
