using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class playerCamera : NetworkBehaviour
{
    public PlayerScript varibles;
    public playerLogic player_logic;
    [Tooltip("相机代码")]
    public Cam_script cam_script;
    private bool isPaused = false; // Tracks pause state

    void Start()
    {
        varibles = GetComponent<PlayerScript>();
        player_logic = GetComponent<playerLogic>();

        if (IsOwner)
        {
            GameObject cam = Camera.main.gameObject;
            cam_script = cam.GetComponent<Cam_script>();
            GameObject vc = GameObject.Find("VC player camera");
            vc.GetComponent<VC_Script>().ply = GetComponent<PlayerScript>();
            vc.GetComponent<VC_Script>().myVCam.Follow = transform.GetChild(3);
            GameObject aim = GameObject.FindGameObjectWithTag("Aim");
            if (aim != null) aim.GetComponent<debug>().ply = GetComponent<PlayerScript>();
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }

    public void cam()
    {
        if (IsOwner && !isPaused) // Stop camera movement when paused
        {
            varibles.mouX = cam_script.mousePosition.x;
            varibles.mouY = cam_script.mousePosition.y;
            varibles.sX = transform.position.x;
            varibles.sY = transform.position.y;
            varibles.disY = (varibles.mouY - transform.position.y) * varibles.mouse_mult;
            varibles.disX = (varibles.mouX - transform.position.x) * varibles.mouse_mult;
        }
    }

    public void TogglePause()
    {
        isPaused = !isPaused;
    }
}
