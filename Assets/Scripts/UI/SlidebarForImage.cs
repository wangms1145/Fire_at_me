using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlidebarForImage : MonoBehaviour
{
    private float amount;
    [SerializeField]
    private Image image;
    // Start is called before the first frame update
    void Start()
    {
        //image = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        image.fillAmount = amount;
    }
    public void setAmount(float amount){
        this.amount = amount;
    }
}
