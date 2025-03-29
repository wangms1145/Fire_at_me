using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitcher : MonoBehaviour
{
    public string lobbySceneName = "Lobby"; // Change this to your actual lobby scene name

    public void LoadLobby()
    {
        SceneManager.LoadScene(lobbySceneName);
    }
}
