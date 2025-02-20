using System;
using System.Threading.Tasks;
using TMPro;
using Unity.Services.Lobbies;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.UI;

public class InputWindow : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private Image backgroundImage;
    [SerializeField] private TMP_InputField tMP_InputField;
    [SerializeField] private Button cancelButton;
    [SerializeField] private Button okButton;
    [SerializeField] private TestLobby testlobby;
    [SerializeField] InputWindow inputWindow;

    public bool avoidNextClose = false;

    void Start()
    {
        testlobby = FindAnyObjectByType<TestLobby>();
        inputWindow = FindObjectOfType<InputWindow>();
        this.gameObject.SetActive(false);
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
        await testlobby.JoinLobbyByCode(tMP_InputField.text, this.gameObject.GetComponent<InputWindow>());
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
        else
        {
            tMP_InputField.text = "Code doen't exsit";
        }




    }

}
