using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitcher : MonoBehaviour
{
    public string lobbySceneName = "Lobby"; // Change this to your actual lobby scene name

    public void loadLobby()
    {
        Debug.Log("Successful click");
        SceneManager.LoadScene(lobbySceneName);
    }
}
