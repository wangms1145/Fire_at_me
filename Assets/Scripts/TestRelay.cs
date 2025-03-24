using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Networking.Transport.Relay;
using System;
using System.Threading.Tasks;


public class TestRelay : MonoBehaviour
{

    private TestLobby testLobby;

    

    //Singleton
    public static TestRelay _instance;
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
            _instance = GetComponent<TestRelay>();
        }
    }




    private async void Start()
    {
        testLobby = FindFirstObjectByType<TestLobby>();

        await UnityServices.InitializeAsync();

        AuthenticationService.Instance.SignedIn += () =>
        {
            Debug.Log("Signed in " + AuthenticationService.Instance.PlayerId);

        };

        DontDestroyOnLoad(gameObject);

        //await AuthenticationService.Instance.SignInAnonymouslyAsync ();
    }


    [ContextMenu("CreateRelay")]
    public async Task<string> CreateRelay()
    {
        try{
            Allocation allocation = await RelayService.Instance.CreateAllocationAsync(testLobby.maxPlayer-1);

            string joinCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);

            Debug.Log("joincode: " + joinCode);

            RelayServerData relayServerData = new RelayServerData(allocation,"dtls");

            NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(relayServerData);

            NetworkManager.Singleton.StartHost();

            return joinCode;
        }
        catch(RelayServiceException e)
        {
            Debug.Log("RelayServiceException" + e);
            return  null;
        }

        
    }


    [ContextMenu("JoinRelay")]
    public async void JoinRelay(string joinCode)
    {
        try{
            Debug.Log("Join realy with join code:" + joinCode);
        JoinAllocation joinAllocation = await RelayService.Instance.JoinAllocationAsync(joinCode);

                    RelayServerData relayServerData = new RelayServerData(joinAllocation,"dtls");
            NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(relayServerData);



            NetworkManager.Singleton.StartClient();
        }
        catch(RelayServiceException e){
            Debug.Log("RelayServiceException" + e);
        }
    }

}
