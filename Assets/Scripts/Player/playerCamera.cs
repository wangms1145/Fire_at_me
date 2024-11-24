using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerCamera : MonoBehaviour
{
    public PlayerScript varibles;
    public playerLogic player_logic;
    [Tooltip("相机代码")]
    public Cam_script cam_script;
    void Start(){
        varibles = GetComponent<PlayerScript>();
        player_logic = GetComponent<playerLogic>();
    }
    public void cam(){
        varibles.mouX = cam_script.mousePosition.x;
        varibles.mouY = cam_script.mousePosition.y;
        varibles.sX = transform.position.x;
        varibles.sY = transform.position.y;
        varibles.disY = (varibles.mouY - transform.position.y) * varibles.mouse_mult;
        varibles.disX = (varibles.mouX - transform.position.x) * varibles.mouse_mult;
    }
}
