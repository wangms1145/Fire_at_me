using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class ShotButton : MonoBehaviour
{
    [Serializable]
    public class ButtonClickedEvent : UnityEvent {}
    [FormerlySerializedAs("onHit")]
    [SerializeField]
    private ButtonClickedEvent m_OnClick = new ButtonClickedEvent();
    public ButtonClickedEvent onHit
    {
        get { return m_OnClick; }
        set { m_OnClick = value; }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnHitFunc(){
        m_OnClick.Invoke();
    }
    public void debugFunc(){
        Debug.Log("EventT");
    }
}
