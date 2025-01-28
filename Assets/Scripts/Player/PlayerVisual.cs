using System.Collections;
using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerVisual : NetworkBehaviour {
    
    public PlayerScript varibles;
    public Image healthBar;


    void Start()
    {
        varibles = GetComponent<PlayerScript>();
        healthBar = GetComponentInChildren<Transform>().GetComponentInChildren<Image>();
    }


    public void changePlayerHealthBarVisual()
    {

    }



}

