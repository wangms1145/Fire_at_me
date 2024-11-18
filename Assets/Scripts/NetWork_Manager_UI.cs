using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;

public class Network_Manager_UI : MonoBehaviour
{
[SerializeField] private Button serverBtn;
[SerializeField] private Button HostBtn;
[SerializeField] private Button ClientBtn;

    // Start is called before the first frame update
    private void Awake()
    {
        serverBtn.onClick.AddListener( () => { NetworkManager.Singleton.StartServer() ; } );
        HostBtn.onClick.AddListener( () => { NetworkManager.Singleton.StartHost() ; } );
        ClientBtn.onClick.AddListener( () => { NetworkManager.Singleton.StartClient() ; } );
    }

    // Update is called once per frame

}
