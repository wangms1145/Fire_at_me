using System;
using System.Threading.Tasks;
using TMPro;
using Unity.Services.Lobbies;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.UI;

public class JoinLobbyByCodeInputWindow : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private Image backgroundImage;
    [SerializeField] public TMP_InputField tMP_InputField;
    [SerializeField] private Button cancelButton;
    [SerializeField] private Button okButton;
    [SerializeField] private TestLobby testlobby;
    [SerializeField] JoinLobbyByCodeInputWindow inputWindow;

    public bool avoidNextClose = false;

    void Start()
    {
        testlobby = FindAnyObjectByType<TestLobby>();
        inputWindow = FindObjectOfType<JoinLobbyByCodeInputWindow>();
    }

    //Hide the window at start
        private bool flag = true;
        private void LateUpdate() 
        {
            if(flag)
            {
                gameObject.SetActive(false);
                flag = false;
            }
        }



    public void onCancel()
    {
        tMP_InputField.text = "";

        this.gameObject.SetActive(false);
    }

    public async void onOk()
    {        
        Debug.Log(tMP_InputField.text);


        try{
        await testlobby.JoinLobbyByCode(tMP_InputField.text, this.gameObject.GetComponent<JoinLobbyByCodeInputWindow>());
        }
        catch(Exception e)
        {   
            Debug.Log("Join Lobby By Code Error: "+e);
            inputWindow.gameObject.SetActive(true);
        }
        if (!avoidNextClose)
        {
        gameObject.SetActive(false);   
        avoidNextClose = false;         
        }

    }

}
