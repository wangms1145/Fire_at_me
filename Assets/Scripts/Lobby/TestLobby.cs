
using UnityEngine;
using Unity.Services.Core;

using Unity.Services.Authentication;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;

public class TestLobby : MonoBehaviour
{

    public Lobby hostLobby;
    private string playerName;


    //creating lobbies
    string createLobbyName = "This_creat_Lobby";
    int maxPlayer = 2;
    bool isCreatedLobbyPrivate = false;

    private float heartBeatTimer;

    //JoinLobby by code



    public string joinLobbyCode{ get; set;}

    private void Update()
    {
        HandleLobbyHeartBeat();
    }


    private void HandleLobbyHeartBeat()
    {
        if (hostLobby != null)
        {
            heartBeatTimer -= Time.deltaTime;
            if(heartBeatTimer <= 0)
            {
                float heartBeatTimerMax = 15;
                heartBeatTimer = heartBeatTimerMax;

                LobbyService.Instance.SendHeartbeatPingAsync(hostLobby.Id);
            }
        }
    }


    // Start is called before the first frame update
    private async void Start()
    {

        playerName = "someplayer" + UnityEngine.Random.Range(0,114514);
        Debug.Log(playerName);

        await UnityServices.InitializeAsync();
        AuthenticationService.Instance.SignedIn += () =>{
            Debug.Log("Signed in: " + AuthenticationService.Instance.PlayerId);

        };
        await AuthenticationService.Instance.SignInAnonymouslyAsync();

    }

    public async void CreateLobby()
    {
        try{

            CreateLobbyOptions createLobbyOptions= new CreateLobbyOptions
            {
                IsPrivate = isCreatedLobbyPrivate,
               Player = GetPlayer()
            };
            Lobby lobby = await LobbyService.Instance.CreateLobbyAsync(createLobbyName, maxPlayer,createLobbyOptions);

            hostLobby = lobby; 

            Debug.Log("createdLobby!  Lobby_Name:"+ lobby.Name+"  Max_Player:"+lobby.MaxPlayers + "  ID:"+lobby.Id + "  Code:" + lobby.LobbyCode);

            PrintPlayers(hostLobby);

        }
        catch(LobbyServiceException e)
        {
            Debug.LogError("Loby Creation error:" + e);
        }

    }

    public async void ListLobbies(){
        try
        {
            QueryLobbiesOptions queryLobbiesOptions = new QueryLobbiesOptions(){
                Count = 25,
                Filters = new List<QueryFilter>{
                    new QueryFilter(QueryFilter.FieldOptions.AvailableSlots,"0",QueryFilter.OpOptions.GT)
                },
                Order = new List<QueryOrder>(){
                    new QueryOrder(false,QueryOrder.FieldOptions.Created)
                }
                
            };

            QueryResponse queryResponse = await Lobbies.Instance.QueryLobbiesAsync();

            Debug.Log("Lobbies Found:"+ queryResponse.Results.Count);
            foreach(Lobby lobby in queryResponse.Results)
            {
                Debug.Log("Lobby name: "+lobby.Name+"   Max_Players:"+ lobby.MaxPlayers);

            }
        }
        catch(LobbyServiceException e)
        {
            Debug.LogError("ListLobbyError" +e);
        }
    }

    public async void JoinLobby()
    {
        try
        {
            QueryResponse queryResponse = await Lobbies.Instance.QueryLobbiesAsync();
            await Lobbies.Instance.JoinLobbyByIdAsync(queryResponse.Results[0].Id);
        }

        catch(LobbyServiceException e)
        {
            Debug.LogError("ListLobbyError" +e);
        }
    }
     
     public async Task JoinLobbyByCode( string code , InputWindow inputWindow)
     {
        try{
            JoinLobbyByCodeOptions joinLobbyByCodeOptions= new JoinLobbyByCodeOptions(){
                Player = GetPlayer()
            };

            Lobby joinedLobby = await Lobbies.Instance.JoinLobbyByCodeAsync(code,joinLobbyByCodeOptions);
            Debug.Log("Joined Lobby with code:" + code);

            PrintPlayers(joinedLobby);
        }
        catch (LobbyServiceException e)
        {
            Debug.Log("Join Lobby By Code Error in Input Window" + e);
            inputWindow.avoidNextClose = true;
        }

     }

     public async void QuickJoinLobby()
     {
        try 
        {
            QuickJoinLobbyOptions quickJoinLobbyOptions= new QuickJoinLobbyOptions(){
                Player = GetPlayer()
            };
            await LobbyService.Instance.QuickJoinLobbyAsync(quickJoinLobbyOptions);
        }
        catch(LobbyServiceException e)
        { 
            Debug.Log("Quick join error: "+ e);
        }
     }

    public  void PrintPlayers( Lobby lobby)
    {
        Debug.Log("Player in lobby" + lobby);
        foreach (Player player in lobby.Players)
        {
            Debug.Log(player.Id + "  " + player.Data["playerName"].Value);
        }
    }

    private Player GetPlayer()
    {
        Player  player = new Player
                    {
                        Data = new Dictionary<string,PlayerDataObject>
                            {
                            {"playerName" , new PlayerDataObject(PlayerDataObject.VisibilityOptions.Member, playerName)}
                            }
                    };
        return player;
    }




}

