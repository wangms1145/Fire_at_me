using System.Collections;
using System.Collections.Generic;
//using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;
//using UnityEngine;
using UnityEngine.UI;

public class HealthOutScrtpt : MonoBehaviour
{
    [SerializeField] private Sprite left;
    [SerializeField] private Sprite mid;
    [SerializeField] private Sprite right;
    [SerializeField] private playerLogic player;
    [SerializeField] private GameObject Outline;
    [SerializeField] private float width;
    private float amount,max,curr_hp,num;
    // Start is called before the first frame update
    void Start()
    {
        player = GetComponentInParent<Transform>().GetComponentInParent<playerLogic>();
        num = player.GetMaxHealth() / 100;
        for(int i = 0; i< num; i++){
            GameObject heart = Instantiate(Outline, transform.position + Vector3.right * i * width,transform.rotation);
            heart.transform.parent = gameObject.transform;
            if(i == 0){
                heart.GetComponent<Image>().sprite = left;
            }
            else if(i == num-1){
                heart.GetComponent <Image>().sprite = right;
            }
            else{
                heart.GetComponent<Image>().sprite = mid;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
