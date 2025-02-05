using System;
using System.Collections;
using System.Collections.Generic;
//using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;
using UnityEngine.UI;
public class healthBar_script : MonoBehaviour
{
    [SerializeField] private playerLogic player;
    [SerializeField] public Image bar;
    private float amount,max,curr_hp;
    // Start is called before the first frame update
    void Start()
    {
        player = GetComponentInParent<Transform>().GetComponentInParent<playerLogic>();
        bar = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        max = player.GetMaxHealth();
        max = max > 0 ? max : 1;
        curr_hp = player.GetHealth();
        
        amount = Math.Clamp(curr_hp/max,0,1);
        bar.fillAmount = amount;
    }
}
