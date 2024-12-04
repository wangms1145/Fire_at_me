using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class playerSound : NetworkBehaviour
{
    public PlayerScript varibles;
    public playerLogic player_logic;
    [Tooltip("玩家音频源")]
    public AudioSource audSource;
    [Tooltip("摔落音频")]
    public AudioClip fall;
    [Tooltip("死亡音频")]
    public AudioClip died;
    [Tooltip("掉虚空音效")]
    public AudioClip zhuiji;
    [Tooltip("失真效果器")]
    public AudioDistortionFilter filter;
    void Start(){
        varibles = GetComponent<PlayerScript>();
        player_logic = GetComponent<playerLogic>();
    }
    public void sound(){
        
    }
    public void fallsound(){
        filter.distortionLevel = Mathf.InverseLerp(-10,-30,player_logic.ys);
        audSource.PlayOneShot(fall);
    }
    public void onStartDeath(){
        if(transform.position.y < varibles.diedYpos){
            filter.distortionLevel = 0;
            audSource.PlayOneShot(zhuiji);
        }
        else{
            filter.distortionLevel = 0;
            audSource.PlayOneShot(died);
        }
    }
}
