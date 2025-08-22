using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Gamestar_animation : MonoBehaviour 
{
    public float duration = 2f;
    public Text cg;
    private Color color;
    private float timer = 0f;

    void Start()
    {
        //cg = GetComponent<Text>();
        color= cg.color;
        color.a = 0f;
        cg.color = color; // start invisible
    }

    void Update()
    {
        if (timer < duration)
        {
            timer += Time.deltaTime;
            color.a = Mathf.Clamp01(timer / duration); // fade in
            cg.color = color;
        }
    }
}
