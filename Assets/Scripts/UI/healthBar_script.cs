using System;
using System.Collections;
using System.Collections.Generic;
//using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;
using UnityEngine.UI;
public class healthBar_script : MonoBehaviour
{
    [SerializeField] private Sprite left;
    [SerializeField] private Sprite mid;
    [SerializeField] private Sprite right;
    [SerializeField] private playerLogic player;
    [SerializeField] private GameObject heart;
    [SerializeField] private float width;
    private List<GameObject> hearts = new List<GameObject>();
    private float amount,max,curr_hp,num;
    // Start is called before the first frame update
    void Start()
    {
        player = GetComponentInParent<Transform>().GetComponentInParent<playerLogic>();
        num = player.GetMaxHealth() / 100;
        for(int i = 0; i< num; i++){
            GameObject heartins = Instantiate(heart, transform.position + Vector3.right * i * width,transform.rotation);
            heartins.transform.parent = gameObject.transform;
            hearts.Add(heartins);
            if(i == 0){
                heartins.GetComponent<Image>().sprite = left;
            }
            else if(i == num-1){
                heartins.GetComponent <Image>().sprite = right;
            }
            else{
                heartins.GetComponent<Image>().sprite = mid;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        max = player.GetMaxHealth();
        max = max > 0 ? max : 1;
        curr_hp = player.GetHealth();
        GameObject[] hearts_arr = hearts.ToArray();
        for(int i = 0;i<hearts.Count;i++){
            amount = Math.Clamp((curr_hp-i*100f)/100f,0,1);
            hearts_arr[i].GetComponent<Image>().fillAmount = amount;
        }
    }
}
