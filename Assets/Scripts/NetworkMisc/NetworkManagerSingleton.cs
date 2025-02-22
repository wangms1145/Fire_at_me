using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class NetworkManagerSingleton : MonoBehaviour
{
    private static NetworkManager _instance;
    void Awake()
    {
        ManageSingleton();
    }
    void ManageSingleton(){
        if(_instance != null){
            gameObject.SetActive(false);
            Destroy(gameObject);
        }
        else{
            _instance = GetComponent<NetworkManager>();
        }
    }
}
