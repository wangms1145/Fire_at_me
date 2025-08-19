using System.Collections;
using System.Collections.Generic;
using Cainos.LucidEditor;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;

public class ScoreAnim : MonoBehaviour
{
    public Animator anim;
    private float timer;
    public int test_score;
    public void Start()
    {
        anim = GetComponent<Animator>();
    }
    public void Update()
    {
        //debug_ani();
    }
    public void changeTo(int num)
    {
        anim.SetInteger("Score", num);
    }
    [ContextMenu("manual_test")]
    public void manual_test()
    {
        changeTo(test_score);
    }
    public void debug_ani()
    {
        timer -= Time.deltaTime;
        if (timer < 0)
        {
            timer = 0.8f;
            changeTo(Random.Range(1, 5));
        }

    }
}
