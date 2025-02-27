using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class meteorSpawner : MonoBehaviour
{
    [SerializeField] private GameObject meteor;
    [SerializeField] private Sprite[] ls;
    [SerializeField] private Vector3 hori;
    [SerializeField] private Vector3 vert;
    [SerializeField] private Vector3 depth;
    [SerializeField] private Vector3 center;
    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0;i<64;i++){
            float pos = UnityEngine.Random.Range(0f,Mathf.PI * 2);
            transform.position = center + hori * Mathf.Cos(pos) + vert * Mathf.Sin(pos) + depth * Mathf.Sin(pos);
            GameObject meteor_ins = Instantiate(meteor,transform.position,transform.rotation);
            meteor_ins.GetComponent<SpriteRenderer>().sprite = ls[UnityEngine.Random.Range(0,ls.Length)];
            meteor_ins.GetComponent<meteorScript>().center = center;
            meteor_ins.GetComponent<meteorScript>().hori = hori;
            meteor_ins.GetComponent<meteorScript>().vert = vert;
            meteor_ins.GetComponent<meteorScript>().depth = depth;
            meteor_ins.GetComponent<meteorScript>().pos = pos;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
