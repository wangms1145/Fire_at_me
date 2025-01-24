using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerReachManager : MonoBehaviour
{
    public double WeaponReachDistance;
    [HideInInspector]
    public GameObject[] ReachableWeapons;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        ReachableWeapons = WeaponInDis(GameObject.FindGameObjectsWithTag("DroppedWeapon"));
        //Debug.Log("" + ReachableWeapons.Length);
    }
    private bool WpDisCheck(GameObject wp){
        Vector2 dif = wp.transform.position-transform.position;
        dif.x = Mathf.Pow(dif.x,2);
        dif.y = Mathf.Pow(dif.y,2);
        return Mathf.Sqrt(dif.x+dif.y)<=WeaponReachDistance;
    }
    private GameObject[] WeaponInDis(GameObject[] wps){
        Stack<GameObject> ans = new Stack<GameObject>();
        foreach(GameObject wp in wps){
            if(WpDisCheck(wp)){
                ans.Push(wp);  
            }
        }
        return ans.ToArray();
    }
}
