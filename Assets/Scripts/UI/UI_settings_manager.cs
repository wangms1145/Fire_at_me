using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public GameObject settingsUI; // Assign your settings menu panel in the Inspector
    public GameObject mainMenuUI; // Assign your main menu panel in the Inspector
    private bool isPaused = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ToggleMenu();
        }
    }

    public void ToggleMenu()
    {
        isPaused = !isPaused;
        settingsUI.SetActive(isPaused);
        mainMenuUI.SetActive(isPaused);
    }
}
