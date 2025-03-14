using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;


public class RelayManger : MonoBehaviour
{

    private TestLobby testLobby;

    

    //Singleton
    private static RelayManger _instance;
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
            _instance = GetComponent<RelayManger>();
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
    private async void CreateRelay()
    {
        try{
            Allocation allocation = await RelayService.Instance.CreateAllocationAsync(testLobby.maxPlayer-1);

            string joinCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);

            Debug.Log("joincode: " + joinCode);

            NetworkManager.Singleton.GetComponent<UnityTransport>().SetHostRelayData
            (
                allocation.RelayServer.IpV4,
                (ushort)allocation.RelayServer.Port,
                allocation.AllocationIdBytes,
                allocation.Key,
                allocation.ConnectionData
            );

            NetworkManager.Singleton.StartHost();
        }
        catch(RelayServiceException e)
        {
            Debug.Log("RelayServiceException" + e);
        }
    }


    [ContextMenu("JoinRelay")]
    private async void JoinRelay(string joinCode)
    {
        try{
            Debug.Log("Join realy with join code:" + joinCode);
        JoinAllocation joinAllocation = await RelayService.Instance.JoinAllocationAsync(joinCode);

        NetworkManager.Singleton.GetComponent<UnityTransport>().SetClientRelayData(
            joinAllocation.RelayServer.IpV4,
            (ushort)joinAllocation.RelayServer.Port,
            joinAllocation.AllocationIdBytes,
            joinAllocation.Key,
            joinAllocation.ConnectionData,
            joinAllocation.HostConnectionData
        );

            NetworkManager.Singleton.StartClient();
        }
        catch(RelayServiceException e){
            Debug.Log("RelayServiceException" + e);
        }
    }

}
