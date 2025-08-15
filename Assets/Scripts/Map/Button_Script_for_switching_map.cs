using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonBinder : MonoBehaviour
{
    public Button myButton;
    public TestRelay relayInstance;
    [SerializeField] public string mapName;

    void Start()
    {

        myButton = GetComponent<Button>();
        relayInstance = FindObjectOfType<TestRelay>();
        if (relayInstance != null)
        {
            myButton.onClick.AddListener(() => relayInstance.SwitchMap(mapName));
        }
        
    }
}

