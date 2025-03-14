using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.UI;

public class CreateLobbyInputWindow : MonoBehaviour
{

    [SerializeField] private TestLobby testLobby;
    [SerializeField] private TMP_InputField lobby_name_input;

    [SerializeField] private Button max_Player_2_Button;
    [SerializeField] private Button max_Player_3_Button;
    [SerializeField] private Button max_Player_4_Button;
    [SerializeField] private Button set_Lobby_Private;
    [SerializeField] private Button set_Lobby_Public;


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





    //Set to Default selection when appears
        void OnEnable()
    {
        SetMaxPlayer_4();
        SetLobby_Public();
    }





    // set lobby name
        public void SetLobbyName()
        {
            if(lobby_name_input.text != "")
            {
                testLobby.createLobbyName = lobby_name_input.text;
            }
            else
            {
                testLobby.createLobbyName = "Some_Lobby" + UnityEngine.Random.Range(1000, 9999);
            }
        }





     //for max player buttons
        public void SetMaxPlayer_2()
        {
            testLobby.maxPlayer = 2;
            Update_Max_Player_Button(2);
        }
        public void SetMaxPlayer_3()
        {
            testLobby.maxPlayer = 3;
            Update_Max_Player_Button(3);
        }
        public void SetMaxPlayer_4()
        {
            testLobby.maxPlayer = 4;
            Update_Max_Player_Button(4);
        }
        private void Update_Max_Player_Button(int maxPlayer)
    {

        if (maxPlayer == 2)
        {
            // Access the Image component of the button and set its color
            max_Player_2_Button.GetComponent<UnityEngine.UI.Image>().color = Color.cyan;
            max_Player_3_Button.GetComponent<UnityEngine.UI.Image>().color = Color.white;
            max_Player_4_Button.GetComponent<UnityEngine.UI.Image>().color = Color.white;
        }
        else if (maxPlayer == 3)
        {
            // Access the Image component of the button and set its color
            max_Player_3_Button.GetComponent<UnityEngine.UI.Image>().color = Color.cyan;
            max_Player_2_Button.GetComponent<UnityEngine.UI.Image>().color = Color.white;
            max_Player_4_Button.GetComponent<UnityEngine.UI.Image>().color = Color.white;
        }
        else if (maxPlayer == 4)
        {
            // Access the Image component of the button and set its color
            max_Player_4_Button.GetComponent<UnityEngine.UI.Image>().color = Color.cyan;
            max_Player_2_Button.GetComponent<UnityEngine.UI.Image>().color = Color.white;
            max_Player_3_Button.GetComponent<UnityEngine.UI.Image>().color = Color.white;
        }

    }





    //for set lobby Public||Private
        public void SetLobby_Public()
        {
            testLobby.isCreatedLobbyPrivate = false;
            Update_Privacy_Button(false);
        }
        public void SetLobby_Private()
        {
            testLobby.isCreatedLobbyPrivate = true;
            Update_Privacy_Button(true);         
        }
        private void Update_Privacy_Button(bool value)
        {
            if (value)
            {
                set_Lobby_Private.GetComponent<UnityEngine.UI.Image>().color = Color.cyan;
                set_Lobby_Public.GetComponent<UnityEngine.UI.Image>().color = Color.white;
            }
            else 
            {
                set_Lobby_Public.GetComponent<UnityEngine.UI.Image>().color = Color.cyan;
                set_Lobby_Private.GetComponent<UnityEngine.UI.Image>().color = Color.white;

            }
        }







}
