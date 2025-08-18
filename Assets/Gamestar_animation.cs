using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gamestar_animation : MonoBehaviour 
{
    public float duration = 2f;
    private CanvasGroup cg;
    private float timer = 0f;

    void Start()
    {
        cg = GetComponent<CanvasGroup>();
        cg.alpha = 0f; // start invisible
    }

    void Update()
    {
        if (timer < duration)
        {
            timer += Time.deltaTime;
            cg.alpha = Mathf.Clamp01(timer / duration); // fade in
        }
    }
}
