using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class heartScript : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private GameObject txt;
    [SerializeField] private playerLogic ply;
    private float health;
    void Start()
    {
        txt = transform.GetChild(1).gameObject;
        ply=GetComponentInParent<playerLogic>();
    }

    // Update is called once per frame
    void Update()
    {
        if(ply.IsOwner){
            gameObject.SetActive(false);
            return;
        }
        health = ply.GetNetHealth()/100;
        health = Mathf.Round(health*10)/10f;
        txt.GetComponent<TextMeshPro>().text = health + "X";
    }
}
