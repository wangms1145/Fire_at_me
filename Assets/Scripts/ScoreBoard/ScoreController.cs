using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
//using Unity.VisualScripting;
using UnityEngine;
public class ScoreData
{
    public GameObject Player;
    public GameObject ScoreBoard;
    public int Score;
    public ScoreData(GameObject player, GameObject scoreBoard, int score)
    {
        Player = player;
        ScoreBoard = scoreBoard;
        Score = score;
    }
}
public class CmpScore : Comparer<ScoreData>
{
    // Compares by Length, Height, and Width.
    public override int Compare(ScoreData x, ScoreData y)
    {
        return x.Score - y.Score;
    }
}
public class ScoreController : MonoBehaviour
{
    public GameObject Score;

    public List<ScoreData> scoreDatas = new List<ScoreData>();
    public bool changed = false;
    private bool player_changed = false;
    private float timer;
    // Start is called before the first frame update
    void Start()
    {
        updatePlayer();
        if (player_changed)
        {
            scoreDatas.Sort(new CmpScore());
            for (int i = 0; i < scoreDatas.Count; i++)
            {
                scoreDatas[i].ScoreBoard.GetComponent<ScoreAnim>().changeTo(i+1);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;
        if (timer < 0)
        {
            timer = 0.5f - timer;
            updatePlayer();
            if (player_changed)
        {
            scoreDatas.Sort(new CmpScore());
            for (int i = 0; i < scoreDatas.Count; i++)
            {
                scoreDatas[i].ScoreBoard.GetComponent<ScoreAnim>().changeTo(i+1);
            }
        }
        }

    }
    void updatePlayer()
    {

        foreach (GameObject pl in GameObject.FindGameObjectsWithTag("Player"))
        {
            bool flag = true;
            for (int i = 0; i < scoreDatas.Count; i++)
            {
                if (scoreDatas[i].Player.Equals(pl))
                {
                    flag = false;
                }
            }
            if (flag)
            {
                GameObject score = Instantiate(Score, transform);
                scoreDatas.Add(new ScoreData(pl, score, 0));
                player_changed = true;
            }
        }
        for (int i = 0; i < scoreDatas.Count; i++)
        {
            bool flag = true;
            foreach (GameObject pl in GameObject.FindGameObjectsWithTag("Player"))
            {
                if (scoreDatas[i].Player.Equals(pl))
                {
                    flag = false;
                }
            }
            if (flag)
            {
                Destroy(scoreDatas[i].ScoreBoard);
                scoreDatas.RemoveAt(i);
                player_changed = true;
                i--;
            }
        }

    }
}
