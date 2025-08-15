using UnityEngine;
using Unity.Services.Core;

using Unity.Services.Authentication;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using UnityEditor.SearchService;
using UnityEngine.SceneManagement;
using System.Transactions;
using UnityEditor;

public class TestLobby : MonoBehaviour
{

    bool isAlreadyInRelay = false;
    public Lobby hostLobby;
    private string playerName;


    //creating lobbies
    public string createLobbyName = "This_creat_Lobby";
    public int maxPlayer = 2;
    public bool isCreatedLobbyPrivate = false;

    private float heartBeatTimer = 1.1f;
    private float lobbyPollTimer = 1.1f;

    //JoinLobby by code


    public event EventHandler<LobbyEventArgs> OnJoinedLobby;
    public event EventHandler<LobbyEventArgs> OnJoinedLobbyUpdate;
    public event EventHandler<LobbyEventArgs> OnKickedFromLobby;
    public event EventHandler<LobbyEventArgs> OnLobbyGameModeChanged;
    public class LobbyEventArgs : EventArgs
    {
        public Lobby lobby;
    }



    public static TestLobby Instance;
    void Awake()
    {
        ManageSingleton();
    }
    void ManageSingleton()
    {
        if (Instance != null)
        {
            gameObject.SetActive(false);
            Destroy(gameObject);
        }
        else
        {
            Instance = GetComponent<TestLobby>();
        }
    }


    public string joinLobbyCode { get; set; }

    private void Update()
    {
        HandleLobbyHeartBeat();
        HandleLobbyPolling();
    }


    private void HandleLobbyHeartBeat()
    {
        if (hostLobby != null)
        {
            heartBeatTimer -= Time.deltaTime;
            if (heartBeatTimer <= 0)
            {
                float heartBeatTimerMax = 15;
                heartBeatTimer = heartBeatTimerMax;

                LobbyService.Instance.SendHeartbeatPingAsync(hostLobby.Id);
            }
        }
    }


    //     private async void HandleLobbyPolling()
    //     {

    //         //Debug.Log("HandleLobbyPolling");

    //         if (hostLobby != null)
    //         {
    //             Debug.Log("Hostlobby != null");
    //             lobbyPollTimer -= Time.deltaTime;
    //             if (lobbyPollTimer < 0f)
    //             {
    //                 Debug.Log("Lobby Pull Time < 0");
    //                 float lobbyPollTimerMax = 1.1f;
    //                 lobbyPollTimer = lobbyPollTimerMax;

    //                 hostLobby = await LobbyService.Instance.GetLobbyAsync(hostLobby.Id);

    //                 OnJoinedLobbyUpdate?.Invoke(this, new LobbyEventArgs { lobby = hostLobby });

    //                 // if (!IsPlayerInLobby()) {
    //                 //     // Player was kicked out of this lobby
    //                 //     Debug.Log("Kicked from Lobby!");

    //                 //     OnKickedFromLobby?.Invoke(this, new LobbyEventArgs { lobby = joinedLobby });

    //                 //     joinedLobby = null;
    //                 // }

    //                 if (hostLobby.Data["Key_Start_Game"].Value != "0" && !isAlreadyInRelay)
    //                 {
    //                     Debug.Log("SceneManager.LoadScene(\"Start 1\")     ||     " + IsLobbyHost());
    //                     if (!IsLobbyHost())
    //                     {
    //                         Debug.Log("Load Scene");

    //                         /*





    // */
    //                         SceneManager.LoadScene("Start 1");


    //                         /*





    // */
    //                         TestRelay._instance.JoinRelay(hostLobby.Data["Key_Start_Game"].Value);
    //                         isAlreadyInRelay = true;
    //                     }
    //                 }
    //             }
    //         }
    //     }
private string lastRelayCode = "";
private string lastMapName = "";

private async void HandleLobbyPolling()
{

    if (hostLobby == null) return;

    lobbyPollTimer -= Time.deltaTime;
    if (lobbyPollTimer <= 0f)
    {
        Debug.Log("lobby pul timer<0");
        lobbyPollTimer = 1.1f;
        hostLobby = await LobbyService.Instance.GetLobbyAsync(hostLobby.Id);
        OnJoinedLobbyUpdate?.Invoke(this, new LobbyEventArgs { lobby = hostLobby });

        string relayCode = hostLobby.Data["Key_Start_Game"]?.Value;
        string mapName = hostLobby.Data["Key_Map_Chosen"]?.Value;



        Debug.Log(" Testing: if (!string.IsNullOrEmpty(relayCode) && relayCode != lastRelayCode)");
        Debug.LogWarning("relay code" + relayCode + " last relay code" + lastRelayCode);
        
        if (!string.IsNullOrEmpty(relayCode) && relayCode != lastRelayCode)
            {
                Debug.Log(" Testing Success: if (!string.IsNullOrEmpty(relayCode) && relayCode != lastRelayCode)");
                lastRelayCode = relayCode;

                if (!IsLobbyHost())
                {
                    // 1. Shutdown old Relay

                    if (TestRelay._instance != null && TestRelay._instance.IsRelayRunning())
                        TestRelay._instance.ShutdownRelay();
                    Debug.Log("Relay shut down");

                    await Task.Delay(100); // short delay for shutdown

                    // 3. Load new scene if it changed
                    if (!string.IsNullOrEmpty(mapName) && mapName != lastMapName)
                    {
                        lastMapName = mapName;
                        SceneManager.LoadScene(mapName);
                    }
                    Debug.Log("map change");


                    // 2. Join new Relay
                    TestRelay._instance.JoinRelay(relayCode);
                    Debug.Log("Relay connected");

                }
            }
    }
}



    public bool IsLobbyHost()
    {
        return hostLobby != null && hostLobby.HostId == AuthenticationService.Instance.PlayerId;
    }

    private async void Start()
    {

        playerName = "someplayer" + UnityEngine.Random.Range(0, 114514);
        Debug.Log(playerName);

        await UnityServices.InitializeAsync();
        AuthenticationService.Instance.SignedIn += () =>
        {
            Debug.Log("Signed in: " + AuthenticationService.Instance.PlayerId);

        };

        await AuthenticationService.Instance.SignInAnonymouslyAsync();

        DontDestroyOnLoad(gameObject);
    }




    [ContextMenu("CreateLobby")]
    public async void CreateLobby()
    {
        try
        {

            CreateLobbyOptions createLobbyOptions = new CreateLobbyOptions
            {
                IsPrivate = isCreatedLobbyPrivate,
                Player = GetPlayer(),
                Data = new Dictionary<string, DataObject>{
                    { "Key_Start_Game" , new DataObject(DataObject.VisibilityOptions.Member,"0")},
                    { "Key_Map_Chosen" , new DataObject(DataObject.VisibilityOptions.Member,"Start 1")}
                }
            };
            Lobby lobby = await LobbyService.Instance.CreateLobbyAsync(createLobbyName, maxPlayer, createLobbyOptions);

            hostLobby = lobby;

            Debug.Log("createdLobby!  Lobby_Name:" + lobby.Name + "  Max_Player:" + lobby.MaxPlayers + "  ID:" + lobby.Id + "  Code:" + lobby.LobbyCode);

            PrintPlayers(hostLobby);

        }
        catch (LobbyServiceException e)
        {
            Debug.LogError("Loby Creation error:" + e);
        }


        /*



        */
        SceneManager.LoadScene("Start 1");
        /*





        */

        try
        {
            string relayCode = await TestRelay._instance.CreateRelay();

            hostLobby = await Lobbies.Instance.UpdateLobbyAsync(hostLobby.Id, new UpdateLobbyOptions
            {
                Data = new Dictionary<string, DataObject>
            {
                { "Key_Start_Game" , new DataObject(DataObject.VisibilityOptions.Member, relayCode)},
                { "Key_Map_Chosen" , new DataObject(DataObject.VisibilityOptions.Member, "Start 1")}
            }
            });

            Debug.Log("relay code: " + relayCode);
        }
        catch (LobbyServiceException e)
        {
            Debug.LogError("LobbyServiceException: testlobby->createlobby()\n" + e);
        }

    }


    [ContextMenu("ListLobbies")]
    public async void ListLobbies()
    {
        try
        {
            QueryLobbiesOptions queryLobbiesOptions = new QueryLobbiesOptions()
            {
                Count = 25,
                Filters = new List<QueryFilter>{
                    new QueryFilter(QueryFilter.FieldOptions.AvailableSlots,"0",QueryFilter.OpOptions.GT)
                },
                Order = new List<QueryOrder>(){
                    new QueryOrder(false,QueryOrder.FieldOptions.Created)
                }

            };

            QueryResponse queryResponse = await Lobbies.Instance.QueryLobbiesAsync();

            Debug.Log("Lobbies Found:" + queryResponse.Results.Count);
            foreach (Lobby lobby in queryResponse.Results)
            {
                Debug.Log("Lobby name: " + lobby.Name + "   Max_Players:" + lobby.MaxPlayers);

            }
        }
        catch (LobbyServiceException e)
        {
            Debug.LogError("ListLobbyError" + e);
        }
    }




    [ContextMenu("JoinLobby")]
    public async void JoinLobby()
    {
        try
        {
            QueryResponse queryResponse = await Lobbies.Instance.QueryLobbiesAsync();
            await Lobbies.Instance.JoinLobbyByIdAsync(queryResponse.Results[0].Id);
        }

        catch (LobbyServiceException e)
        {
            Debug.LogError("ListLobbyError" + e);
        }
    }



    [ContextMenu("JoinLobbyByCode")]
    public async Task JoinLobbyByCode(string code, JoinLobbyByCodeInputWindow inputWindow)
    {
        try
        {
            JoinLobbyByCodeOptions joinLobbyByCodeOptions = new JoinLobbyByCodeOptions()
            {
                Player = GetPlayer()
            };

            Lobby joinedLobby = await Lobbies.Instance.JoinLobbyByCodeAsync(code, joinLobbyByCodeOptions);
            Debug.Log("Joined Lobby with code:" + code);

            PrintPlayers(joinedLobby);

            hostLobby = joinedLobby;
        }
        catch (LobbyServiceException e)
        {
            Debug.Log("Join Lobby By Code Error in Input Window" + e);
            inputWindow.avoidNextClose = true;
            inputWindow.tMP_InputField.text = e.Message;
        }

    }



    [ContextMenu("QuickJoinLobby")]
    public async void QuickJoinLobby()
    {
        try
        {
            QuickJoinLobbyOptions quickJoinLobbyOptions = new QuickJoinLobbyOptions()
            {
                Player = GetPlayer()
            };
            Lobby joinedLobby = await LobbyService.Instance.QuickJoinLobbyAsync(quickJoinLobbyOptions);

            PrintPlayers(joinedLobby);

            hostLobby = joinedLobby;


        }
        catch (LobbyServiceException e)
        {
            Debug.Log("Quick join error: " + e);
        }
    }



    public void PrintPlayers(Lobby lobby)
    {
        Debug.Log("Player in lobby" + lobby);
        foreach (Player player in lobby.Players)
        {
            Debug.Log(player.Id + "  " + player.Data["playerName"].Value);
        }
    }

    private Player GetPlayer()
    {
        Player player = new Player
        {
            Data = new Dictionary<string, PlayerDataObject>
                            {
                            {"playerName" , new PlayerDataObject(PlayerDataObject.VisibilityOptions.Member, playerName)}
                            }
        };
        return player;
    }


    


    




}

