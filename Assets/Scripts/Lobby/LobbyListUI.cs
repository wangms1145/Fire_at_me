using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LobbyListUI : MonoBehaviour
{


    public static LobbyListUI Instance { get; private set; }



    [SerializeField] private Transform lobbySingleTemplate;
    [SerializeField] private Transform container;
    [SerializeField] private Button refreshButton;
    [SerializeField] private Button createLobbyButton;

    private void Awake() {
        Instance = this;

        lobbySingleTemplate.gameObject.SetActive(false);

        // refreshButton.onClick.AddListener(RefreshButtonClick);
        // createLobbyButton.onClick.AddListener(CreateLobbyButtonClick);
    }

    private void Start() {
        //TestLobby.Instance.OnLobbyListChanged += TestLobby_OnLobbyListChanged;
        TestLobby.Instance.OnJoinedLobby += TestLobby_OnJoinedLobby;
        //TestLobby.Instance.OnLeftLobby += TestLobby_OnLeftLobby;
        TestLobby.Instance.OnKickedFromLobby += TestLobby_OnKickedFromLobby;
    }

    private void TestLobby_OnKickedFromLobby(object sender, TestLobby.LobbyEventArgs e) {
        Show();
    }

    // private void TestLobby_OnLeftLobby(object sender, EventArgs e) {
    //     Show();
    // }

    private void TestLobby_OnJoinedLobby(object sender, TestLobby.LobbyEventArgs e) {
        Hide();
    }

    // private void TestLobby_OnLobbyListChanged(object sender, TestLobby.OnLobbyListChangedEventArgs e) {
    //     UpdateLobbyList(e.lobbyList);
    // }

    // private void UpdateLobbyList(List<Lobby> lobbyList) {
    //     foreach (Transform child in container) {
    //         if (child == lobbySingleTemplate) continue;

    //         Destroy(child.gameObject);
    //     }

    //     foreach (Lobby lobby in lobbyList) {
    //         Transform lobbySingleTransform = Instantiate(lobbySingleTemplate, container);
    //         lobbySingleTransform.gameObject.SetActive(true);
    //         LobbyListSingleUI lobbyListSingleUI = lobbySingleTransform.GetComponent<LobbyListSingleUI>();
    //         lobbyListSingleUI.UpdateLobby(lobby);
    //     }
    // }

    // private void RefreshButtonClick() {
    //     TestLobby.Instance.RefreshLobbyList();
    // }

    // private void CreateLobbyButtonClick() {
    //     LobbyCreateUI.Instance.Show();
    // }

    private void Hide() {
        gameObject.SetActive(false);
    }

    private void Show() {
        gameObject.SetActive(true);
    }
}
