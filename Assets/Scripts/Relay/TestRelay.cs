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
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;


public class TestRelay : MonoBehaviour
{

    private TestLobby testLobby;



    //Singleton
    public static TestRelay _instance;
    void Awake()
    {
        ManageSingleton();
    }
    void ManageSingleton()
    {
        if (_instance != null)
        {
            gameObject.SetActive(false);
            Destroy(gameObject);
        }
        else
        {
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
        try
        {
            Allocation allocation = await RelayService.Instance.CreateAllocationAsync(testLobby.maxPlayer - 1);

            string joinCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);

            Debug.Log("joincode: " + joinCode);

            RelayServerData relayServerData = new RelayServerData(allocation, "dtls");

            NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(relayServerData);

            NetworkManager.Singleton.StartHost();

            return joinCode;
        }
        catch (RelayServiceException e)
        {
            Debug.Log("RelayServiceException" + e);
            return null;
        }


    }


    [ContextMenu("JoinRelay")]
    public async void JoinRelay(string joinCode)
    {
        try
        {
            Debug.Log("Join realy with join code:" + joinCode);
            JoinAllocation joinAllocation = await RelayService.Instance.JoinAllocationAsync(joinCode);

            RelayServerData relayServerData = new RelayServerData(joinAllocation, "dtls");
            NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(relayServerData);



            NetworkManager.Singleton.StartClient();
        }
        catch (RelayServiceException e)
        {
            Debug.Log("RelayServiceException" + e);
        }
    }
    


public bool IsRelayRunning()
{
    return NetworkManager.Singleton != null && 
           (NetworkManager.Singleton.IsHost || NetworkManager.Singleton.IsClient);
}

public void ShutdownRelay()
{
    if (NetworkManager.Singleton != null && 
        (NetworkManager.Singleton.IsClient || NetworkManager.Singleton.IsHost))
    {
        NetworkManager.Singleton.Shutdown();
    }
}




public async Task<string> SwitchMapAsHost(string sceneName)
{
    if (testLobby == null || !testLobby.IsLobbyHost())
    {
        Debug.LogWarning("Only the host can switch maps/Relay.");
        return null;
    }

    // 1. Close old Relay
    if (IsRelayRunning())
        ShutdownRelay();

    // 2. Load new scene locally
    UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);

    try
    {
        // 3. Create new Relay
        Allocation allocation = await RelayService.Instance.CreateAllocationAsync(testLobby.maxPlayer - 1);
        string joinCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);
        Debug.Log("New Relay Join Code: " + joinCode);

        // 4. Configure transport & start hosting
        NetworkManager.Singleton.GetComponent<UnityTransport>()
            .SetRelayServerData(new RelayServerData(allocation, "dtls"));
        NetworkManager.Singleton.StartHost();

        // 5. Update lobby keys for clients
        testLobby.hostLobby = await Lobbies.Instance.UpdateLobbyAsync(testLobby.hostLobby.Id, new UpdateLobbyOptions
        {
            Data = new Dictionary<string, DataObject>
            {
                { "Key_Map_Chosen", new DataObject(DataObject.VisibilityOptions.Member, sceneName) }, // map name
                { "Key_Relay_Code", new DataObject(DataObject.VisibilityOptions.Member, joinCode) }   // relay code
            }
        });

        return joinCode;
    }
    catch (RelayServiceException e)
    {
        Debug.LogError(e);
        return null;
    }
}

    // public async void SwitchMap(string scene_name)
    // {
    //     // 1. Close old Relay/network
    //     if (NetworkManager.Singleton.IsClient || NetworkManager.Singleton.IsHost)
    //     {
    //         NetworkManager.Singleton.Shutdown();
    //     }

    //     // (Optional) Load new map before creating Relay
    //     UnityEngine.SceneManagement.SceneManager.LoadScene(scene_name);

    //     // 2. Create new Relay allocation
    //     try
    //     {
    //         Allocation allocation = await RelayService.Instance.CreateAllocationAsync(maxConnections: 10);
    //         string joinCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);
    //         Debug.Log("New Relay Join Code: " + joinCode);

    //         // 3. Configure transport with new Relay data
    //         var relayData = allocation;
    //         NetworkManager.Singleton.GetComponent<Unity.Netcode.Transports.UTP.UnityTransport>()
    //             .SetRelayServerData(relayData.RelayServer.IpV4,
    //                                 (ushort)relayData.RelayServer.Port,
    //                                 relayData.AllocationIdBytes,
    //                                 relayData.Key,
    //                                 relayData.ConnectionData);

    //         // 4. Start hosting again
    //         NetworkManager.Singleton.StartHost();
    //     }
    //     catch (RelayServiceException e)
    //     {
    //         Debug.LogError(e);
    //     }
    // }


}
